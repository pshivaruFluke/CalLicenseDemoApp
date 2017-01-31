﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CalLicenseDemo.DatabaseContext;
using CalLicenseDemo.Model;
using LicenseKey;

namespace CalLicenseDemo.Logic
{
    class LicenseLogic
    {
        public UserModel User { get; set; }

        public string ErrorMessage { get; set; }

        private readonly LicenseAppDBContext _dbContext;

        public LicenseLogic()
        {
            _dbContext = new LicenseAppDBContext();
        }

        public string GenerateLicense(string subscriptionType)
        {
            bool status;
            status = CreateUserInfo();
            string userUniqueId = UniqueuserIdentifier();
            string licenseKey = GetlicenseKey();
            if (status)
                status = UpdateSubScriptionDetails(userUniqueId, licenseKey, subscriptionType);
            if (status)
                return licenseKey;
            else
                return string.Empty;

        }

        public string GetlicenseKey()
        {
            LicenseKey.GenerateKey keygeneration = new GenerateKey();
            keygeneration.LicenseTemplate = "vvvvppppxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxx" +
                                            "-xxxxxxxxxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxx";
            keygeneration.MaxTokens = 2;
            keygeneration.AddToken(0, "v", LicenseKey.GenerateKey.TokenTypes.NUMBER, "1");
            keygeneration.AddToken(1, "p", LicenseKey.GenerateKey.TokenTypes.NUMBER, "2");
            keygeneration.UseBase10 = false;
            keygeneration.UseBytes = false;
            keygeneration.CreateKey();
            return keygeneration.GetLicenseKey();
        }


        public bool UpdateSubScriptionDetails(string userUniqueId, string licenseKey, string subscriptiontype)
        {
            License lic = new License();
            lic.IsAvailable = false;
            lic.LicenseKey = licenseKey;
            lic.LicenseTypeId = Convert.ToInt32(subscriptiontype);

            UserLicense userLic = new UserLicense();
            userLic.UserKey = userUniqueId;
            userLic.ActivationDate = DateTime.Today;

            try
            {
                lic = _dbContext.License.Add(lic);
                userLic.LicenseId = lic.LicenseId;
                _dbContext.UserLicense.Add(userLic);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public string UniqueuserIdentifier()
        {
            string processId = string.Empty, motherBoard = string.Empty, volumeSerial = string.Empty;
            ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            ManagementObjectCollection proDetails = mos.Get();
            foreach (ManagementObject o in proDetails)
                processId = o.Properties["ProcessorId"].Value.ToString();

            mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            ManagementObjectCollection moc = mos.Get();
            foreach (ManagementObject mo in moc)
                motherBoard = (string)mo["SerialNumber"];

            string drive = "C";
            ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
            dsk.Get();
            volumeSerial = dsk["VolumeSerialNumber"].ToString();
            try
            {
                string _id = String.Concat("LicenceModule", processId, motherBoard, volumeSerial);
                byte[] _byteIds = Encoding.UTF8.GetBytes(_id);

                //Use MD5 to get the fixed length checksum of the ID string
                MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();
                byte[] _checksum = _md5.ComputeHash(_byteIds);

                //Convert checksum into 4 ulong parts and use BASE36 to encode both
                string _part1Id = Base36.Encode(BitConverter.ToUInt32(_checksum, 0));
                string _part2Id = Base36.Encode(BitConverter.ToUInt32(_checksum, 4));
                string _part3Id = Base36.Encode(BitConverter.ToUInt32(_checksum, 8));
                string _part4Id = Base36.Encode(BitConverter.ToUInt32(_checksum, 12));

                //Concat these 4 part into one string
                return string.Format("{0}-{1}-{2}-{3}", _part1Id, _part2Id, _part3Id, _part4Id);

            }
            catch (Exception)
            {

            }
            return null;
        }

        public bool CreateUserInfo()
        {
            try
            {
                if (_dbContext.User.ToList().Any(u => u.Email == User.Email && u.Password == User.Password))
                {
                    ErrorMessage = "Email id is lready registered";
                    return false;
                }
                Team _team =
                    _dbContext.Team.ToList()
                        .FirstOrDefault(
                            t => t.Name == User.Organization.Name && t.GroupEmail == User.Organization.GroupEmail);
                if (_team != null)
                {
                    User.Organization = _team;
                    User.TeamID = _team.TeamId;
                }
                else
                {
                    _team = _dbContext.Team.Add(User.Organization);
                    User.TeamID = _team.TeamId;
                    User = _dbContext.User.Add(User);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public List<LicenseType> GetSubscriptionDetails()
        {
            return _dbContext.LicenseType.ToList();
        }

        public bool ValidateLicense()
        {

            return true;
        }


    }
}
