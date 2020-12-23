using System.Threading.Tasks;
using Newtonsoft.Json;
using SimonsSearch.Core.Interfaces;
using SimonsSearch.Data;

namespace SimonsSearch.Core.Services
{
    public class SimonsSearchDataSeeder : ISimonsSearchDataSeeder
    {
        private readonly ISimonsSearchDataContext _simonsSearchDataContext;
        private readonly ISimonsSearchHttpClient _simonsSearchHttpClient;

        public SimonsSearchDataSeeder(ISimonsSearchDataContext simonsSearchDataContext, ISimonsSearchHttpClient simonsSearchHttpClient)
        {
            _simonsSearchDataContext = simonsSearchDataContext;
            _simonsSearchHttpClient = simonsSearchHttpClient;
        }

        public async Task SeedDataAsync()
        {
            var httpClient = _simonsSearchHttpClient.GetHttpClient();

            var result = await httpClient.GetAsync("/sv_lsm_data.json");

            if (!result.IsSuccessStatusCode)
            {
                return;
            }

            var content = await result.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return;
            }

            var parsedContent = JsonConvert.DeserializeObject<SimonsSearchDataSeederDto>(content);

            _simonsSearchDataContext.Init(parsedContent);
        }
    }
}