using Microsoft.Extensions.Options;
using System.Text.Json;
using UmbracoBridge.Models;

namespace UmbracoBridge.DelegatingHandlers
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private readonly UmbracoCMSSettings _options;
        public AuthenticationHandler(IOptions<UmbracoCMSSettings> options)
        {
            _options = options.Value;
        }
        async protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Token? result = await GetAccessToken(cancellationToken);

            request.Headers.Add("Authorization", $"{result.TokenType} {result.AccessToken}");

            return await base.SendAsync(
                request,
                cancellationToken);
        }

        private async Task<Token?> GetAccessToken(CancellationToken cancellationToken)
        {
            var requestModel = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}security/back-office/token");
            var collection = SetNameValueCollection();
            var content = new FormUrlEncodedContent(collection);
            requestModel.Content = content;

            var httpResponseMessage = await base.SendAsync(
                requestModel,
                cancellationToken);

            httpResponseMessage.EnsureSuccessStatusCode();

            var responseToken = await httpResponseMessage.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Token>(responseToken);
            return result;
        }

        private List<KeyValuePair<string, string>> SetNameValueCollection()
        {
            return new List<KeyValuePair<string, string>>
            {
                new("grant_type", _options.GrantType),
                new("client_id", _options.ClientId),
                new("client_secret", _options.ClientSecret)
            };
        }
    }
}
