using ConsoleDallE.HttpServices;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ConsoleDallE.Services
{
    //----------------------------------------------------------------
    // OpenIAHttpService: Classe que atua como
    // serviço HTTP para interagir com a API do OpenAI.
    // É responsável por gerar imagens e fazer o download
    // de imagens usando a API do OpenAI.
    //----------------------------------------------------------------

    public class OpenIAHttpService : IOpenIAProxy
    {
        // Cliente HttpClient usado para enviar solicitações HTTP.
        private readonly HttpClient _httpClient;

        // ID da assinatura usado para autenticação na API do OpenAI.
        private readonly string _subscriptionId;

        // Chave de API usada para autenticação na API do OpenAI.
        private readonly string _apiKey;  
        
        //Construtor que injeta IConfiguration
        public OpenIAHttpService(IConfiguration configuration)
        {
            var openApiUrl = configuration["OpenAi:Url"] ?? throw new ArgumentException(nameof(configuration));

            // Inicializa o cliente HttpClient
            // com a URL base da API do OpenAI.
            _httpClient = new HttpClient { BaseAddress = new Uri(openApiUrl) };

            _subscriptionId = configuration["OpenAi:SubscriptionId"];
            _apiKey = configuration["OpenAi:ApiKey"];
        }

        // Gera imagens com base em uma solicitação
        // prompt usando a API do OpenAI.
        public async Task<GenerateImageResponse> GenerateImages(GenerateImageRequest prompt, 
                                                                CancellationToken cancellation = default)
        {
            using var rq = new HttpRequestMessage(HttpMethod.Post, "/v1/images/generations");

            var jsonRequest = JsonSerializer.Serialize(prompt, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            rq.Content = new StringContent(jsonRequest);
            rq.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var apiKey = _apiKey;
            rq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var subscriptionId = _subscriptionId;
            rq.Headers.TryAddWithoutValidation("OpenAI-Organization", subscriptionId);

            var response = await _httpClient.SendAsync(rq, HttpCompletionOption.ResponseHeadersRead, cancellation);

            response.EnsureSuccessStatusCode();

            var content = response.Content;

            var jsonResponse = await content.ReadFromJsonAsync<GenerateImageResponse>(cancellationToken: cancellation);

            return jsonResponse;
        }

        // Faz o download de uma imagem usando uma URL fornecida.
        public async Task<byte[]> DownloadImage(string url)
        {
            var buffer = await _httpClient.GetByteArrayAsync(url);

            return buffer;
        }
    }
}
