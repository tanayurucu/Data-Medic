using System.Net;
using System.Net.Http.Headers;
using System.Text;

using Newtonsoft.Json;

namespace DataMedic.Application.Sensors.Handlers;

public class HandlePrtgSensorCreatedDomainEvent : IHandlePrtgSensorCreatedDomainEvent
{
    private readonly HttpClient _httpClient;
    private readonly string baseUrl;
    private readonly string username1;
    private readonly string password1;

    public HandlePrtgSensorCreatedDomainEvent()
    {
        baseUrl = "";
        username1 = "";
        password1 = ".";
        var proxy = new HttpClientHandler
        {
            UseProxy = true,
            Proxy = new WebProxy("",
                int.Parse("8080"))
            {
                Credentials = new NetworkCredential(
                    "",
                    ""
                )
            }
        };
        _httpClient = new HttpClient(proxy);
        _httpClient.BaseAddress = new Uri(baseUrl);
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var byteArray = Encoding.ASCII.GetBytes($"{username1}:{password1}");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }

    public async Task Handle(string prtgSensorId, TimeSpan scanPeriod)
    {
        var response = await _httpClient.GetAsync($"/api/getsensordetails.json?id={prtgSensorId}&username={username1}&password={password1}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var sensordata = JsonConvert.DeserializeObject<dynamic>(content)?.sensordata;

        var result = sensordata?.statustext.ToString();
    }
    
}

public interface IHandlePrtgSensorCreatedDomainEvent
{
    public Task Handle(string prtgSensorId, TimeSpan scanPeriod);
}