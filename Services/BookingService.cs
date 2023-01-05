using ProRFL.UI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProRFL.UI.Services
{
    public interface IBookingService
    {
        ValueTask<Room[]> GetRooms();
        ValueTask<Room[]> GetActiveRooms();

    }
    internal class BookingService : IBookingService
    {
        private readonly HttpClient _http;

        public BookingService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri(File.ReadAllText(AppSetting.Url!));
        }

        public async ValueTask<Room[]> GetActiveRooms()
        {
            var rooms = await _http.GetFromJsonAsync<Room[]>("locks/activate");
            return rooms!;
        }

        public async ValueTask<Room[]> GetRooms()
        {
            Room[] rooms = Array.Empty<Room>();
            var item = await _http.GetStringAsync("locks/rooms");
            var document = JsonDocument.Parse(item);
            var element = document.RootElement;
            if (element.EnumerateObject().Count() > 1)
                rooms = element.Deserialize<Room[]>()!;
            else
                rooms = Array.Empty<Room>();
            return rooms!;
        }
    }
}
