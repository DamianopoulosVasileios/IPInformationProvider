using IPInformationProvider.API.DBContext;
using IPInformationProvider.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IPInformationProvider.API.Repositories
{
    public class IPRepository : IIPRepository
    {
        private readonly IPDbContext _context;

        public IPRepository(IPDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<IIPs>> GetAllAsync()
        {
            var result = await _context.IPs.ToListAsync();
            return result;
        }
        public async Task<IEnumerable<IIPs>> GetPartialAsync(int start,int batch)
        {
            var totalCount = await _context.IPs.CountAsync();
            if (totalCount > start)
            {
                var takeCount = totalCount - start;
                var records = await _context.IPs.Skip(start).Take(takeCount).ToListAsync();
                return records;
            }

            return Enumerable.Empty<IIPs>();
        }
        public async Task<IIPs?> GetAsync(string id)
        {
            var result = await _context.IPs.FirstOrDefaultAsync(x => x.CountryName == id);
            return result;
        }

        public async Task<bool> InsertAsync(IEnumerable<IIPs> iPAddress)
        {
            foreach(var ip in iPAddress)
            {
                try
                {
                    await _context.IPs.AddAsync(ip);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while inserting to database");
                }
            }
            return true;
        }
        public async Task<bool> UpdateAsync(IEnumerable<IIPs> iPAddress)
        {
            foreach (var ip in iPAddress)
            {
                try
                {
                    await Task.Run(() =>
                    {
                        _context.Update(ip);
                        _context.SaveChanges();
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while updating database");
                }
            }
            return true;
        }

    }
}
