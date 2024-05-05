using BlazorServerApp.Pages;
using BlazorServerAppDB.Data.ContactForm;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Data
{
    public class FormService
    {
        private readonly ShapeShiftFormContext _context;
        public FormService(ShapeShiftFormContext context)
        {
            _context = context;
        }

        public async Task AddMessageAsync(ContactMessage message)
        {
            _context.ContactMessage.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
