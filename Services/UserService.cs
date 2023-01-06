using HotelLocks.Shared.Models;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.ApplicationServices;
using ProRFL.UI.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProRFL.UI.Services;

public interface IUserService
{
    ValueTask<bool> RegisterUser(UserCredential user);
    ValueTask UpdateUser(UserCredential user);
    ValueTask<UserCredential> Login(UserCredential model);
    ValueTask<string> GenSHA512(string password);
}

public class UserService : IUserService
{
    private readonly HttpClient _http;  
    private UserCredential? loginResult;

    public UserService(HttpClient http)
    { 
        _http = http;
        _http.BaseAddress = new Uri(File.ReadAllText(AppSetting.Url!));
    }    
    public async ValueTask<string> GenSHA512(string password)
    {
        string jwt = "";
        string certificate = await File.ReadAllTextAsync(AppSetting.Cert!);
        try
        {
            var claim = new Claim[]
            {
                new Claim(ClaimTypes.Name, "Master"),
                new Claim(ClaimTypes.Role, "Master")
            };

            var token = new JwtSecurityToken(
                null,
                null,
                claim,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(certificate)),
                SecurityAlgorithms.HmacSha512Signature));

            jwt = new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        return await ValueTask.FromResult(jwt);
    }

    public async ValueTask<UserCredential> Login(UserCredential model)
    {
        loginResult = new();
        var user = new
        {
            email = model.Email,
            password = model.HashedPassword
        };
        try
        {
            using var request = await _http.PostAsJsonAsync("auth/login", user);
            request.EnsureSuccessStatusCode();
            var contents = await request.Content.ReadAsStringAsync();
            var document = JsonDocument.Parse(contents);
            var element = document.RootElement;
            var jsonElements = element.GetProperty("object").EnumerateObject();
            foreach (var item in jsonElements)
            {
                if (item.NameEquals("token"))
                    loginResult.Token = item.Value.ToString();
                else if (item.NameEquals("firstName"))
                    loginResult.FirstName = item.Value.ToString();
                else if (item.NameEquals("lastName"))
                    loginResult.LastName = item.Value.ToString();
            }
            return loginResult!;
        }
        catch (Exception ex)
        {
            Console.Write(ex);
        }
        return loginResult!;
    }

    public async ValueTask<bool> RegisterUser(UserCredential user)
    {
        using var request = await _http.PostAsJsonAsync("auth/main", user);
        request.EnsureSuccessStatusCode();
        return request.IsSuccessStatusCode;
    }
  

    public ValueTask UpdateUser(UserCredential user)
    {
        throw new NotImplementedException();
    }
}

