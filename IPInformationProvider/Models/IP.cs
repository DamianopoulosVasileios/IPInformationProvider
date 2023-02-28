using System.ComponentModel.DataAnnotations;

namespace IPInformationProvider.API.Models
{
    public class IP
    {
        [Required]
        public string IPAddress { get; set; }
        [Required]
        public string CountryName { get; set; }
        [Required]
        public string TwoLetterCode { get; set; }
        [Required]
        public string ThreeLetterCode { get; set; }

        public IP(string iPAddress, string twoLetterCode, string threeLetterCode, string countryName)
        {
            IPAddress = iPAddress;
            TwoLetterCode = twoLetterCode;
            ThreeLetterCode = threeLetterCode;
            CountryName = countryName;
        }
    }
}
