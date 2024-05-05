using BlazorServerApp.Pages;
using BlazorServerAppDB.Data.CalculatorBMI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace BlazorServerApp.Data
{
    public class CalculatorBMIService
    {
        private readonly ShapeShiftContext _context;
        public CalculatorBMIService(ShapeShiftContext context)
        {
            _context = context;
        }
        public async Task<List<BmiResult>> GetForecastAsync(string strCurrentUser)
        {
            return await _context.BmiResult
                 .Where(x => x.UserName == strCurrentUser)
                 .AsNoTracking().ToListAsync();
        }

        public Task<BmiResult>
            CreateForecastAsync(BmiResult objBmiresult)
        {
            _context.BmiResult.Add(objBmiresult);
            _context.SaveChanges();
            return Task.FromResult(objBmiresult);
        }

        public Task<bool>
           DeleteForecastAsync(BmiResult objBmiresult)
        {
            var ExistingObjBmiResult =
                _context.BmiResult
                .Where(x => x.Id == objBmiresult.Id)
                .FirstOrDefault();
            if (ExistingObjBmiResult != null)
            {
                _context.BmiResult.Remove(ExistingObjBmiResult);
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
