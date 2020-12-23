using System.Net.Http;

namespace SimonsSearch.Core.Interfaces
{
    public interface ISimonsSearchHttpClient
    {
        HttpClient GetHttpClient();
    }
}