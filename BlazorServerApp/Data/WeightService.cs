using BlazorServerApp.Pages;
using BlazorServerAppDB.Data.CalculatorBMI;
using BlazorServerAppDB.Data.Weight;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace BlazorServerApp.Data
{
    public class WeightService
    {
        private readonly ShapeShiftSecondContext _context;
        public WeightService(ShapeShiftSecondContext context)
        {
            _context = context;
        }
        public async Task<List<Weight>>
            GetWeightAsync(string strCurrentUser)
        { 
            return await _context.Weight
                 .Where(x => x.UserName == strCurrentUser)
                 .AsNoTracking().ToListAsync();
        }

        public Task<Weight>
            CreateWeightAsync(Weight objWeightResult)
        {
            _context.Weight.Add(objWeightResult);
            _context.SaveChanges();
            return Task.FromResult(objWeightResult);
        }

        public Task<bool>
           DeleteWeightAsync(Weight objWeightResult)
        {
            var ExistingobjWeightResult =
                _context.Weight
                .Where(x => x.Id == objWeightResult.Id)
                .FirstOrDefault();
            if (ExistingobjWeightResult != null)
            {
                _context.Weight.Remove(ExistingobjWeightResult);
                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public async Task<Weight?> GetLatestWeightAsync(string userName)
        {
            var latestWeight = await _context.Weight
                                     .Where(w => w.UserName == userName)
                                     .OrderByDescending(w => w.WeightDate)
                                     .FirstOrDefaultAsync();
            if (latestWeight == null)
            {
                return null;
            }

            if (latestWeight.UserWeight == null)
            {
                return await _context.Weight
                                     .Where(w => w.UserName == userName)
                                     .OrderByDescending(w => w.WeightDate)
                                     .Skip(1)
                                     .FirstOrDefaultAsync();
            }
            return latestWeight;
        }

        public async Task<Weight?> GetLatestGoalWeightAsync(string userName)
        {
            return await _context.Weight
                         .Where(w => w.UserName == userName)
                         .OrderByDescending(w => w.GoalWeight)
                         .FirstOrDefaultAsync();
        }

        public async Task<Weight?> GetWeightFrom7DaysAgoAsync(string userName)
        {
            var date7DaysAgo = DateTime.Now.Date.AddDays(-7);
            return await _context.Weight
                .Where(w => w.UserName == userName && w.WeightDate.HasValue && w.WeightDate.Value.Date == date7DaysAgo)
                .FirstOrDefaultAsync();
        }

        public async Task<Weight?> GetWeightFrom30DaysAgoAsync(string userName)
        {
            var date30DaysAgo = DateTime.Now.Date.AddDays(-30);
            return await _context.Weight
                .Where(w => w.UserName == userName && w.WeightDate.HasValue && w.WeightDate.Value.Date == date30DaysAgo)
                .FirstOrDefaultAsync();
        }

    }
}