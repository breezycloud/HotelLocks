using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using ProRFL.UI.Data;
using HotelLocks.Shared.Models;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private HttpClient _httpClient;
    private NavigationManager _navigation;
    public CustomAuthenticationStateProvider(HttpClient httpClient, NavigationManager navigation)
    {
        _httpClient = httpClient;
        _navigation = navigation;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var auth = await GetTokenAsync();
        string token = auth[0].ToString();
        string role = auth[1].ToString();

        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(role))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        
        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Name, "Master"),
            new Claim(ClaimTypes.Role, role),            
        };

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
    }
    
    public async Task SetTokenAsync(UserCredential response)
    {
        if (response.Token is null || string.IsNullOrEmpty(response.Token))
        {
            DeleteTokenAsync();
        }
        else
        {
            string[] res = new string[] { response.Token, response.Role! };
            await WriteTokenAsync(res);
        }

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<string[]> GetTokenAsync()
    {
        return await File.ReadAllLinesAsync(AppSetting.Token!);
    }
    
    public async Task WriteTokenAsync(string[] res)
    {
        await File.WriteAllLinesAsync(AppSetting.Token!, res);
    }

    public void DeleteTokenAsync() =>
        File.Delete(AppSetting.Token!);

    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes)!;
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp!.Value!.ToString()!));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    public void LogOutNotfiy()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}

