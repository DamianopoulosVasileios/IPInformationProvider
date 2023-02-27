using IPInformationProvider.API.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace IPInformationProvider.API.Models
{
    public class IPs : IIPs
    {
        [Required]
        public string IPAddress { get; set; }
        [Required]
        public string CountryName { get; set; }
        [Required]
        public string TwoLetterCode { get; set; }
        [Required]
        public string ThreeLetterCode { get; set; }

        public IPs(string iPAddress,string countryName, string twoLetterCode, string threeLetterCode)
        {
            IPAddress = iPAddress;
            CountryName = countryName;
            TwoLetterCode = twoLetterCode;
            ThreeLetterCode = threeLetterCode;
        }

        public object SoftCopy (string iPAddress, string twoLetterCode, string threeLetterCode, string countryName)
        {
            IPAddress = iPAddress;
            TwoLetterCode = twoLetterCode;
            ThreeLetterCode = threeLetterCode;
            CountryName = countryName;
            return this;
        }
    }
}
