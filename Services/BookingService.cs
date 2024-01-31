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
        Room[]? rooms = new Room[0];
        JsonSerializerOptions Options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };
        public BookingService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri(File.ReadAllText(AppSetting.Url!));
        }

        public async ValueTask<Room[]?> GetActiveRooms()
        {
            try
            {
                var response = await _http.GetFromJsonAsync<Room[]>("locks/activate")!;
                if (response == null)
                    return rooms!;
                rooms = response;
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
                var response = await _http.GetFromJsonAsync<Room[]>("locks/rooms")!;
                if (response == null)
                    return rooms!;
                rooms = response;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return rooms;
        }
    }
}
