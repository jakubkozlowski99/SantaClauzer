using Newtonsoft.Json;

namespace SantaClauzer.Web;

public class ApiClient(HttpClient httpClient)
{
    public Task<T> GetFromJsonAsync<T>(string path)
    {
        return httpClient.GetFromJsonAsync<T>(path);
    }

    public async Task<T1> PostAsync<T1, T2>(string path, T2 data)
    {
        var res = await httpClient.PostAsJsonAsync(path, data);

        if (res != null && res.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<T1>(await res.Content.ReadAsStringAsync());
        }

        return default;
    }

    public async Task<T1> PutAsync<T1, T2>(string path, T2 data)
    {
        var res = await httpClient.PutAsJsonAsync(path, data);
        if (res != null && res.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<T1>(await res.Content.ReadAsStringAsync());
        }
        return default;
    }

    public async Task<T1> DeleteAsync<T1>(string path)
    {
        var res = await httpClient.DeleteAsync(path);
        if (res != null && res.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<T1>(await res.Content.ReadAsStringAsync());
        }
        return default;
    }
}
