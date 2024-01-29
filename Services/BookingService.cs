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
        ValueTask<Room[]?> GetRooms();
        ValueTask<Room[]?> GetActiveRooms();

    }
    internal class BookingService : IBookingService
    {
        private readonly HttpClient _http;
        Room[]? rooms = [];
        public BookingService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri(File.ReadAllText(AppSetting.Url!));
        }

        public async ValueTask<Room[]?> GetActiveRooms()
        {
            try
            {
                var stream = await _http.GetStreamAsync("locks/activate")!;
                if (stream!.Length > 0) { return rooms; }
                var document = JsonDocument.Parse(stream!);
                var element = document.RootElement;
                if (element.EnumerateObject().Count() > 0)
                    rooms = JsonSerializer.Deserialize<Room[]>(element);
                return rooms!;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return rooms;
        }

        public async ValueTask<Room[]?> GetRooms()
        {            
            try
            {
                var stream = await _http.GetStreamAsync("locks/rooms")!;
                if (stream!.Length > 0) { return rooms; }
                var document = JsonDocument.Parse(stream!);
                var element = document.RootElement;
                if (element.EnumerateObject().Count() > 0)
                    rooms = JsonSerializer.Deserialize<Room[]>(element);
                return rooms!;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return rooms;
        }
    }
}
