using IPInformationProvider.API.Interfaces;
using System.Data.SqlClient;

namespace IPInformationProvider.API.Endpoints   
{
    public class IPReportEndpoint : IIPReportEndpoint
    {
        private readonly IIPResponse _response;

        public IPReportEndpoint(IIPResponse response)
        {
            _response = response;
        }
        public async Task<IEnumerable<IIPResponse>> GetReport(string[]? twoLetterCountryCodes = null)
        {
            var results = new List<IIPResponse>();
            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
            {
                await connection.OpenAsync();
                string sql = "SELECT tbl.CountryName,Count(*) as AddressesCount,UpdatedAt FROM IPs tbl ";
                string where = (twoLetterCountryCodes == null) ? " " : " where tbl.TwoLetterCode in (@TwoLetterCode) ";
                string groupBy = " GROUP BY tbl.CountryName";

                string finalQuery = sql + where + groupBy;

                using (var command = new SqlCommand(finalQuery, connection))
                {
                    if (twoLetterCountryCodes != null)
                    {
                        command.Parameters.AddWithValue("@TwoLetterCode", string.Join(",", twoLetterCountryCodes).TrimEnd(','));
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var CountryName = reader.GetString(reader.GetOrdinal("CountryName"));
                            var AddressesCount = reader.GetInt32(reader.GetOrdinal("AddressesCount"));
                            var UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"));
                            results.Add((IIPResponse)_response.SoftCopy(CountryName, AddressesCount, UpdatedAt));
                        }
                    }
                }
            }
            return results;
        }
    }
}
