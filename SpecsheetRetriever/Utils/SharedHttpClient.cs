using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SpecsheetRetriever.Utils
{
    internal static class SharedHttpClient
    {
        /// <summary>
        /// Only one client instance per app life cycle.
        /// </summary>
        internal static HttpClient Client { get; } = new HttpClient();

        // Helper Methods

        internal static async Task<string?> GetContentString(string url, CancellationToken cToken)
        {
            var request = Client.GetAsync(url, cToken);
            var response = await request;
            if (cToken.IsCancellationRequested) { return null; }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var str = await response.Content.ReadAsStringAsync();
            return str;
        }

    }
}
