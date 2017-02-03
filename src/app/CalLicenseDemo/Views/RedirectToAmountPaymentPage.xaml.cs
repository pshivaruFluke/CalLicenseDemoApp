﻿using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace CalLicenseDemo.Views
{
    /// <summary>
    ///     Interaction logic for RedirectToAmountPage.xaml
    /// </summary>
    public partial class RedirectToAmountPaymentPage : UserControl
    {
        /// <summary>
        ///     The backgroundworker object on which the time consuming operation shall be executed
        /// </summary>
        private readonly BackgroundWorker m_oWorker;

        public RedirectToAmountPaymentPage()
        {
            InitializeComponent();
            m_oWorker = new BackgroundWorker();
            m_oWorker.DoWork += m_oWorker_DoWork;
            m_oWorker.ProgressChanged += m_oWorker_ProgressChanged;
            m_oWorker.RunWorkerCompleted += m_oWorker_RunWorkerCompleted;
            m_oWorker.WorkerReportsProgress = true;
            m_oWorker.WorkerSupportsCancellation = true;
        }

        /// <summary>
        ///     On completed do the appropriate task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //If it was cancelled midway
            if (e.Cancelled)
                lblProgress.Content = "Transaction Cancelled.";
            else if (e.Error != null)
                lblProgress.Content = "Error while performing background action.";
            else
                lblProgress.Content = "Payment Success";
            buttonPayment.IsEnabled = true;
            buttonCancel.IsEnabled = false;
            statusBarPayment.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Notification is performed here to the progress bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Here you play with the main UI thread
            ProgressBarPayment.Value = e.ProgressPercentage;
            lblProgress.Content = "Payment is in Processing......" + ProgressBarPayment.Value + "%";
        }

        /// <summary>
        ///     Time consuming operations go here </br>
        ///     i.e. Database operations,Reporting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //time consuming operation
            for (var i = 0; i < 100; i++)
            {
                Thread.Sleep(100);
                m_oWorker.ReportProgress(i);

                //If cancel button was pressed while the execution is in progress
                //Change the state from cancellation ---> cancel'ed
                if (m_oWorker.CancellationPending)
                {
                    e.Cancel = true;
                    m_oWorker.ReportProgress(0);
                    return;
                }
            }

            //Report 100% completion on operation completed
            m_oWorker.ReportProgress(100);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //Window parentWindow = Window.GetWindow(this);
            //parentWindow.Close();

            if (m_oWorker.IsBusy)
                m_oWorker.CancelAsync();
        }

        private void buttonPayment_Click(object sender, RoutedEventArgs e)
        {
            buttonPayment.IsEnabled = false;
            buttonCancel.IsEnabled = true;
            ProgressBarPayment.Visibility = Visibility.Visible;
            lblProgress.Visibility = Visibility.Visible;
            statusBarPayment.Visibility = Visibility.Collapsed;
            //Start the async operation here
            m_oWorker.RunWorkerAsync();
        }
    }
}