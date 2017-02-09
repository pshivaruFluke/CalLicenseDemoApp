using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalLicense.Core.Model
{
    public  class License
    {
        public License()
        {
            LicenseId = 0;
            LicenseKey = string.Empty;
            IsAvailable = false;
        }

        [Key]
        public int LicenseId { get; set; }

        public string LicenseKey { get; set; }
        public bool IsAvailable { get; set; }
        public  int LicenseTypeId { get; set; }
        [ForeignKey("LicenseTypeId")]
        public virtual LicenseType LicenseType { get; set; }
    }
}