using HotelLocks.Shared.Models;
using ProRFL.UI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProRFL.UI.Services
{
    
    public interface ICardService
    {
        
        ValueTask<bool> Init();
        ValueTask<bool> InitializeUSB();
        ValueTask<bool> ReadCard();
        ValueTask<bool> GuestCard(GuestCard card);
    }
    public class CardService : ICardService
    {
        private readonly HttpClient _http;
        int result;
        public CardService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri(File.ReadAllText(AppSetting.Url!));
        }

        public async ValueTask<bool> GuestCard(GuestCard card)
        {
            int dlsCoID = 0;
            //Flag of Deadbolt
            byte llock = Convert.ToByte(1);
            //Flag of public door
            byte pdoors = 0;
            //Guest Dai
            int dai = 0;
            //Time of Issue card, Just the current time,now
            string BDate = card.BDate.ToString("yyMMddHHmm");
            //Check Out
            string EDate = card.EDate.ToString("yyMMddHHmm");

            if (!await ReadCard()) return await ValueTask.FromResult(false);

            StringBuilder CardHexStr = new StringBuilder(128);
            result = Data.ProRFL.GuestCard(Data.ProRFL.flagUSB, dlsCoID, card.CardNo, dai, llock, pdoors, BDate, EDate, card.LockNo!, CardHexStr);
            if (result == 0) return await ValueTask.FromResult(true);
            else return await ValueTask.FromResult(false);
        }

        public ValueTask<bool> Init()
        {
            throw new NotImplementedException();
        }

        public async ValueTask<bool> InitializeUSB()
        {
            result = Data.ProRFL.initializeUSB(Data.ProRFL.flagUSB);
            if (result == 0) return await ValueTask.FromResult(true);
            else return await ValueTask.FromResult(false);
        }

        public async ValueTask<bool> ReadCard()
        {
            if (!await InitializeUSB()) return await ValueTask.FromResult(false);
            StringBuilder buffer = new(128);
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
    }
}
