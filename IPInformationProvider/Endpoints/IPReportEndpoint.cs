using IPInformationProvider.API.Interfaces;
using IPInformationProvider.API.Models;
using System.Data.SqlClient;

namespace IPInformationProvider.API.Endpoints
{
    public class IPReportEndpoint : IIPReportEndpoint
    {
        private readonly IConfiguration _configuration;

        public IPReportEndpoint(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<IPResponse>> GetReport(string[]? twoLetterCountryCodes = null)
        {
            var cn = _configuration.GetValue<string>("ConnectionStrings:DbConnectionString");

            var results = new List<IPResponse>();
            using (var connection = new SqlConnection(cn))
            {
                await connection.OpenAsync();
                string sql = "SELECT tbl.CountryName,Count(*) as AddressesCount,UpdatedAt FROM IP tbl ";
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
                            results.Add(new IPResponse(CountryName, AddressesCount, UpdatedAt));
                        }
                    }
                }
            }
            return results;
        }
    }
}
