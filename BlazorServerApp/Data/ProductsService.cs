using BlazorServerApp.Pages;
using BlazorServerAppDB.Data.Calories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace BlazorServerApp.Data
{
    public class ProductsService
    {
        private readonly ShapeShiftCaloriesContext _context;
        public ProductsService(ShapeShiftCaloriesContext context)
        {
            _context = context;
        }
        public async Task<List<FoodProducts>> GetFoodProductsAsync()
        {
            return await _context.FoodProducts
                 .AsNoTracking().ToListAsync();
        }

    }
}
