using BlazorServerApp.Pages;
using BlazorServerAppDB.Data.Price;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Data
{
    public class PriceService
    {
        private readonly ShapeShiftThirdContext _context;
        public PriceService(ShapeShiftThirdContext context)
        {
            _context = context;
        }
        public async Task<List<Price>> GetPriceAsync(string strCurrentUser)
        {
            return await _context.Price
                 .Where(x => x.UserName == strCurrentUser)
                 .AsNoTracking().ToListAsync();
        }

        public Task<Price>
            CreatePriceAsync(Price objPrice)
        {
            _context.Price.Add(objPrice);
            _context.SaveChanges();
            return Task.FromResult(objPrice);
        }
    }
}
