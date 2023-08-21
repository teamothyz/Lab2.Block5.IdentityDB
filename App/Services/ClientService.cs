using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace App.Services
{
    public class ClientService
    {
        private readonly HttpClient _client;
        private readonly HttpContext _httpContext;

        public ClientService(HttpContext httpContext, HttpClient? client = null)
        {
            _httpContext = httpContext;
            _client = client ?? new HttpClient();
            var token = _httpContext.Request.Cookies["AccessToken"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<T?> Get<T>(string relativeUrl)
        {
            try
            {
                var res = await _client.GetAsync(relativeUrl);
                if ((int)res.StatusCode == 401) await _httpContext.SignOutAsync("CookieAuthentication");
                var content = await res.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch
            {
                return default;
            }
        }

        public async Task<T?> Post<T>(string relativeUrl, object? data)
        {
            try
            {
                var res = await _client.PostAsync(relativeUrl, GetBody(data));
                if ((int)res.StatusCode == 401) await _httpContext.SignOutAsync("CookieAuthentication");
                var content = await res.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch
            {
                return default;
            }
        }

        public async Task<T?> Patch<T>(string relativeUrl, object? data)
        {
            try
            {
                var res = await _client.PatchAsync(relativeUrl, GetBody(data));
                if ((int)res.StatusCode == 401) await _httpContext.SignOutAsync("CookieAuthentication");
                var content = await res.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch
            {
                return default;
            }
        }

        public async Task<T?> Delete<T>(string relativeUrl)
        {
            try
            {
                var res = await _client.DeleteAsync(relativeUrl);
                if ((int)res.StatusCode == 401) await _httpContext.SignOutAsync("CookieAuthentication");
                var content = await res.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch
            {
                return default;
            }
        }

        public async Task<string?> Get(string relativeUrl)
        {
            try
            {
                var res = await _client.GetAsync(relativeUrl);
                if ((int)res.StatusCode == 401) await _httpContext.SignOutAsync("CookieAuthentication");
                return await res.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            }
            catch
            {
                return default;
            }
        }

        public async Task<string?> Post(string relativeUrl, object? data)
        {
            try
            {
                var res = await _client.PostAsync(relativeUrl, GetBody(data));
                if ((int)res.StatusCode == 401) await _httpContext.SignOutAsync("CookieAuthentication");
                return await res.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            }
            catch
            {
                return default;
            }
        }

        public async Task<string?> Patch(string relativeUrl, object? data)
        {
            try
            {
                var res = await _client.PatchAsync(relativeUrl, GetBody(data));
                if ((int)res.StatusCode == 401) await _httpContext.SignOutAsync("CookieAuthentication");
                return await res.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            }
            catch
            {
                return default;
            }
        }

        public async Task<string?> Delete(string relativeUrl)
        {
            try
            {
                var res = await _client.DeleteAsync(relativeUrl);
                if ((int)res.StatusCode == 401) await _httpContext.SignOutAsync("CookieAuthentication");
                return await res.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            }
            catch
            {
                return default;
            }
        }

        private static StringContent? GetBody(object? data)
        {
            if (data == null) return null;
            var body = JsonConvert.SerializeObject(data);
            return new StringContent(body, Encoding.UTF8, "application/json");
        }
    }
}
