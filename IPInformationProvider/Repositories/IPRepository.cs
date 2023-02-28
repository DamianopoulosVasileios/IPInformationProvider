using IPInformationProvider.API.DBContext;
using IPInformationProvider.API.Interfaces;
using IPInformationProvider.API.Models;
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
        public async Task<IEnumerable<IP>> GetAllAsync()
        {
            var result = await _context.IP.ToListAsync();
            return result;
        }
        public async Task<IP[]> GetPartialAsync(int start, int batch)
        {
            var totalCount = await _context.IP.CountAsync();
            if (totalCount > start)
            {
                var takeCount = totalCount - start;
                var records = await _context.IP.Skip(start).Take(takeCount).ToListAsync();
                return records.ToArray();
            }

            return Array.Empty<IP>();
        }
        public async Task<IP?> GetAsync(string ip)
        {
            var result = await _context.IP
                .Where(x => x.IPAddress == ip)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> InsertAsync(IEnumerable<IP> iPAddress)
        {
            foreach (var ip in iPAddress)
            {
                try
                {
                    await _context.IP.AddAsync(ip);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while inserting to database");
                }
            }
            return true;
        }
        public async Task<bool> UpdateAsync(IEnumerable<IP> iPAddress)
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
