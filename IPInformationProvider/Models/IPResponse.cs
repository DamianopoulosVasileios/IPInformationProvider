namespace IPInformationProvider.API.Models
{
    public class IPResponse
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
    }
}