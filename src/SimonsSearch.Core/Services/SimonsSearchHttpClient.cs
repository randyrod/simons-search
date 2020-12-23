using System;
using System.Net.Http;
using SimonsSearch.Core.Interfaces;

namespace SimonsSearch.Core.Services
{
    public class SimonsSearchHttpClient : ISimonsSearchHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly object _initHttpClientLock = new object();
        private bool _httpClientIsInitialized;

        public SimonsSearchHttpClient(HttpClient client)
        {
            _httpClient = client;
        }

        public HttpClient GetHttpClient()
        {
            if (_httpClientIsInitialized)
            {
                return _httpClient;
            }

            lock (_initHttpClientLock)
            {
                if (!_httpClientIsInitialized)
                {
                    _httpClient.BaseAddress = new Uri("https://simonsvoss-homework.herokuapp.com/");
                }
            }

            _httpClientIsInitialized = true;

            return _httpClient;
        }
    }
}