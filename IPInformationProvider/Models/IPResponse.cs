using IPInformationProvider.API.Interfaces;

namespace IPInformationProvider.API.Models
{
    public class IPResponse : IIPResponse
    {
        public string CountryName { get; set; }
        public int AddressesCount { get; set; }
        public DateTime LastAddressUpdated { get; set; }

        public IPResponse(string countryName, int addressesCount, DateTime lastAddressUpdated)
        {
            CountryName = countryName;
            AddressesCount = addressesCount;
            LastAddressUpdated = lastAddressUpdated;
        }
        public object SoftCopy(string countryName, int addressesCount, DateTime lastAddressUpdated)
        {
            CountryName = countryName;
            AddressesCount = addressesCount;
            LastAddressUpdated = lastAddressUpdated;
            return this;
        }
    }
}