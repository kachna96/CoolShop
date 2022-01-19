using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

namespace CoolShop.WebApi.IntegrationTests.Extensions;

internal static class HttpClientException
{
    /// <summary>
    /// Patch as json async because it is not included in .net sdk
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="client"></param>
    /// <param name="requestUri"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
    {
        Guard.Against.Null(client, nameof(client));
        Guard.Against.NullOrEmpty(requestUri, nameof(requestUri));
        Guard.Against.Null(value, nameof(value));

        var content = new ObjectContent<T>(value, new JsonMediaTypeFormatter());
        var request = new HttpRequestMessage(HttpMethod.Patch, requestUri) { Content = content };

        return await client.SendAsync(request);
    }
}
