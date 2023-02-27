namespace IPInformationProvider.API.Interfaces
{
    public interface IIPs
    {
        string IPAddress { get; set; }
        string CountryName { get; set; }
        string TwoLetterCode { get; set; }
        string ThreeLetterCode { get; set; }
        object SoftCopy(string iPAddress,string twoLetterCode, string threeLetterCode, string countryName);
    }
}
