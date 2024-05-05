using BlazorServerApp.Pages;
using BlazorServerAppDB.Data.Calories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Data
{
    public class CaloriesService
    {
        private readonly ShapeShiftCaloriesContext _context;
        public CaloriesService(ShapeShiftCaloriesContext context)
        {
            _context = context;
        }
        public async Task<List<UserCaloriesResult>> GetUserCaloriesAsync(string strCurrentUser)
        {
            return await _context.UserCaloriesResult
                 .Where(x => x.UserName == strCurrentUser)
                 .Include(x => x.Product)
                 .AsNoTracking().ToListAsync();
        }

        public Task<UserCaloriesResult>
            CreateForecastAsync(UserCaloriesResult objUserCaloriesResult)
        {
            _context.UserCaloriesResult.Add(objUserCaloriesResult);
            _context.SaveChanges();
            return Task.FromResult(objUserCaloriesResult);
        }

        public Task<bool>
           DeleteCaloriesAsync(UserCaloriesResult objUserCaloriesResult)
        {
            var ExistingobjUserCaloriesResult =
                _context.UserCaloriesResult
                .Where(x => x.Id == objUserCaloriesResult.Id)
                .FirstOrDefault();
            if (ExistingobjUserCaloriesResult != null)
            {
                _context.UserCaloriesResult.Remove(ExistingobjUserCaloriesResult);
                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

    }
}
