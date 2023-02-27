using IPInformationProvider.API.Interfaces;
using IPInformationProvider.API.Static;

namespace IPInformationProvider.API.Endpoints
{
    public class IPInformationEndpoint : IIPInformationEndpoint
    {
        private readonly IIPCaching _cache;
        private readonly IIPRepository _repository;
        private readonly IIPs _iP;

        public IPInformationEndpoint(IIPCaching cache,IIPRepository repository,IIPs iP)
        {
            _cache = cache;
            _repository = repository;
            _iP = iP;
        }

        #region Cache
        public IIPs? GetFromCache(string iPAddress)
        {
            var result = _cache.GetFromCache(iPAddress);
            return result;
        }
        public void InsertOneOrManyToCache(IEnumerable<IIPs> iPs, bool invalidateCache = false)
        {
            _cache.InsertOneOrManyToCache(iPs, invalidateCache);
        }
        #endregion

        #region Database
        public async Task<IIPs?> GetFromDatabaseAsync(string iPAddress)
        {
            var result = await _repository.GetAsync(iPAddress);
            return result;
        }
        public async Task<IEnumerable<IIPs>> GetPartialFromDatabaseAsync(int start,int batch)
        {
            return await _repository.GetPartialAsync(batch, batch += 100);
        }
        public async Task<bool> InsertToDatabaseAsync(IIPs iP)
        {
            return await _repository.InsertAsync(new List<IIPs>() { iP });
        }
        #endregion

        #region IP2C
        public async Task<IIPs> GetFromIP2CAsync(string iPAddress)
        {
            using (HttpClient client = new())
            {
                if (!ValidateIPAddress.ValidateIPv4(iPAddress))
                    throw new Exception("Invalid IP Address");

                return await GetContentFromIP2C(client, iPAddress);
            }
        }
        private async Task<IIPs> GetContentFromIP2C(HttpClient client, string iPAddress)
        {
            HttpResponseMessage response = await client.GetAsync(IPInformationProviderURL.URL + iPAddress);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(result))
                {
                    var ipData = result.Split(';');
                    if (ipData.Length != 4) //1;GR;GRC;Greece
                    {
                        throw new Exception("Invalid Response from IP2C");
                    }

                    return (IIPs)_iP.SoftCopy(
                        iPAddress,
                        ipData[1],
                        ipData[2],
                        ipData[3]);
                }
                throw new Exception("IP Address Not Found in IP2C");
            }
            else
            {
                throw new HttpRequestException($"Failed to get response from {IPInformationProviderURL.URL}. StatusCode={response.StatusCode}");
            }
        }
        #endregion

    }
}
