using BlazorServerApp.Pages;
using BlazorServerAppDB.Data.Calories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Data
{
    public class CaloriesNeededService
    {
        private readonly ShapeShiftCaloriesContext _context;
        public CaloriesNeededService(ShapeShiftCaloriesContext context)
        {
            _context = context;
        }
        public async Task<List<UserCaloriesNeeded>> GetUserCaloriesAsync(string strCurrentUser)
        {
            return await _context.UserCaloriesNeeded
                 .Where(x => x.UserName == strCurrentUser)
                 .AsNoTracking().ToListAsync();
        }

        public Task<UserCaloriesNeeded>
            CreateCaloriesNeededAsync(UserCaloriesNeeded objCaloriesNeeded)
        {
            _context.UserCaloriesNeeded.Add(objCaloriesNeeded);
            _context.SaveChanges();
            return Task.FromResult(objCaloriesNeeded);
        }

        public Task<bool>
          DeleteCaloriesNeededAsync(UserCaloriesNeeded objCaloriesNeeded)
        {
            var ExistingobjCaloriesNeeded =
                _context.UserCaloriesNeeded
                .Where(x => x.Id == objCaloriesNeeded.Id)
                .FirstOrDefault();
            if (ExistingobjCaloriesNeeded != null)
            {
                _context.UserCaloriesNeeded.Remove(ExistingobjCaloriesNeeded);
                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public async Task<UserCaloriesNeeded?> GetLatestCaloriesNeededAsync(string userName)
        {
            return await _context.UserCaloriesNeeded
                         .Where(w => w.UserName == userName)
                         .OrderByDescending(w => w.CaloriesNeededResult)
                         .FirstOrDefaultAsync();
        }
    }
}
