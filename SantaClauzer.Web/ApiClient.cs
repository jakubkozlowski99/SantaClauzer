using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace SantaClauzer.Web;

public class ApiClient(HttpClient httpClient, ProtectedLocalStorage localStorage)
{
    public async Task SetAuthorizeHeader()
    {
        var token = (await localStorage.GetAsync<string>("authToken")).Value;
        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<T> GetFromJsonAsync<T>(string path)
    {
        await SetAuthorizeHeader();

        return await httpClient.GetFromJsonAsync<T>(path);
    }

    public async Task<T1> PostAsync<T1, T2>(string path, T2 data)
    {
        await SetAuthorizeHeader();

        var res = await httpClient.PostAsJsonAsync(path, data);

        if (res != null && res.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<T1>(await res.Content.ReadAsStringAsync());
        }

        return default;
    }

    public async Task<T1> PutAsync<T1, T2>(string path, T2 data)
    {
        await SetAuthorizeHeader();

        var res = await httpClient.PutAsJsonAsync(path, data);
        if (res != null && res.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<T1>(await res.Content.ReadAsStringAsync());
        }
        return default;
    }

    public async Task<T1> DeleteAsync<T1>(string path)
    {
        await SetAuthorizeHeader();

        var res = await httpClient.DeleteAsync(path);
        if (res != null && res.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<T1>(await res.Content.ReadAsStringAsync());
        }
        return default;
    }
}
