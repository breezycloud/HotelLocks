using HotelLocks.Shared.Models;
using Microsoft.EntityFrameworkCore;
using ProRFL.UI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MudBlazor.CategoryTypes;

namespace ProRFL.UI.Services
{
    
    public interface ICardService
    {
        
        ValueTask<bool> Init();
        ValueTask<bool> InitializeUSB();
        ValueTask<(int, string)> CardInfo();
        ValueTask Buzzer();
        ValueTask<bool> ReadCard();
        ValueTask<bool> EraseCard();
        ValueTask<bool> GuestCard(GuestCard card);
        ValueTask AddCard(GuestCard card);
        ValueTask<GuestCard> GetCard(string lockno);
        Task<int> GetCardsIssued();
    }
    public class CardService : ICardService
    {
        private readonly HttpClient _http;
        private AppDbContext _context;
        int result;
        int dlsCoID = 8676;
        StringBuilder buffer = new();
        public CardService(HttpClient http, AppDbContext context)
        {
            _http = http;
            _http.BaseAddress = new Uri(File.ReadAllText(AppSetting.Url!));
            _context = context;
        }

        public async ValueTask<(int, string)> CardInfo()
        {
            if (!await ReadCard()) return await ValueTask.FromResult((4, "Unknow result"));
            string dataHexStr = buffer.ToString();
            buffer = new(16);
            result = Data.ProRFL.GetGuestLockNoByCardDataStr(dlsCoID, dataHexStr, buffer);
            if (result == 0) return await ValueTask.FromResult((0, "Success"));
            else if (result == 1) return await ValueTask.FromResult((1, "CardDataStr Invalid"));
            else if (result == 2) return await ValueTask.FromResult((2, "This is not a card in this hotel"));
            else if (result == 3) return await ValueTask.FromResult((3, "This is not a Guest Card"));
            return await ValueTask.FromResult((4, "Unknow result"));
        }

        public async ValueTask<bool> EraseCard()
        {
            if (!await ReadCard()) return await ValueTask.FromResult(false);
            StringBuilder CardHexStr = new(128);
            int result = Data.ProRFL.CardErase(Data.ProRFL.flagUSB, 0, CardHexStr);
            if (result == 0) return await ValueTask.FromResult(true);
            return await ValueTask.FromResult(false);
        }

        public async ValueTask<bool> GuestCard(GuestCard card)
        {
            //Flag of Deadbolt
            byte llock = Convert.ToByte(1);
            //Flag of public door
            byte pdoors = 0;
            //Time of Issue card, Just the current time,now
            string BDate = card.BDate.ToString("yyMMddHHmm");
            //Check Out
            string EDate = card.EDate.ToString("yyMMddHHmm");

            if (!await ReadCard()) return await ValueTask.FromResult(false);

            StringBuilder CardHexStr = new StringBuilder(128);
            result = Data.ProRFL.GuestCard(Data.ProRFL.flagUSB, dlsCoID, card.CardNo, card.dai, llock, pdoors, BDate, EDate, card.LockNo!, CardHexStr);
            if (result == 0) {
                await AddCard(card);
                return await ValueTask.FromResult(true); 
            }
            else return await ValueTask.FromResult(false);
        }

        public ValueTask<bool> Init()
        {
            throw new NotImplementedException();
        }

        public async ValueTask<bool> InitializeUSB()
        {
            try
            {
                result = Data.ProRFL.initializeUSB(Data.ProRFL.flagUSB);
                if (result == 0) return await ValueTask.FromResult(true);
                else return await ValueTask.FromResult(false);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return false;
        }

        public ValueTask Buzzer()
        {
            Data.ProRFL.Buzzer(Data.ProRFL.flagUSB, 10);
            return ValueTask.CompletedTask;
        }

        public async ValueTask<bool> ReadCard()
        {
            if (!await InitializeUSB()) return await ValueTask.FromResult(false);
            buffer = new(128);
            result = Data.ProRFL.ReadCard(Data.ProRFL.flagUSB, buffer);
            if (result != 0) return await ValueTask.FromResult(false);
            else
            {
                string dataHexStr = buffer.ToString();
                if (!dataHexStr.Contains("551501"))
                {
                    return await ValueTask.FromResult(false);
                }
            }
            return await ValueTask.FromResult(true);
        }

        public async ValueTask<GuestCard> GetCard(string lockno)
        {
            var card = await _context.GuestCards.Where(x => x.LockNo == lockno).FirstOrDefaultAsync();
            return card!;
        }

        public async ValueTask AddCard(GuestCard card)
        {
            _context.Update(card);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCardsIssued()
        {
            int TotalCards = 0;
            if (!File.Exists(AppSetting.Cards))
                return TotalCards;
            var file = await File.ReadAllTextAsync(AppSetting.Cards);
            TotalCards = int.Parse(file);
            return TotalCards;
        }
    }
}
