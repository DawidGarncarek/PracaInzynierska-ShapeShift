using BlazorServerApp.Pages;
using BlazorServerAppDB.Data.Chat;
using BlazorServerAppDB.Data.Exercises;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Data
{
    public class ChatService
    {
        private readonly ShapeShiftChatContext _context;
        public ChatService(ShapeShiftChatContext context)
        {
            _context = context;
        }

        public async Task SendMessageAsync(string messageText, string userName)
        {
            var newMessage = new UserChatMessages
            {
                MessageText = messageText,
                Date = DateTime.Now,
                UserName = userName
            };

            _context.UserChatMessages.Add(newMessage);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserChatMessages>> GetMessagesAsync()
        {
            return await _context.UserChatMessages
                .OrderByDescending(m => m.Date)
                .ToListAsync();
        }

    }
}
