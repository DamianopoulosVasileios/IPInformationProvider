namespace IPInformationProvider.API.Interfaces
{
    public interface IIPResponse
    {
        string CountryName { get; set; }
        int AddressesCount { get; set; }
        DateTime LastAddressUpdated { get; set; }
        public object SoftCopy(string countryName, int addressesCount, DateTime lastAddressUpdated);
    }
}
