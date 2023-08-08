using System.ComponentModel.Design;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
    /// <summary>
    /// Provides extensions to <seealso cref="HttpClient"/>. Primarily platform compatible methods for APIs that are not available for .NET 4.8 and .NET Standard versions of <seealso cref="HttpClient"/>. As well as some synchronous APIs
    /// </summary>
    public static class NetExtensions
    {
        /// <summary>
        /// Provides an implementation of HTTPClient.PatchAsync for Framework 4.8 and NetStandard
        /// </summary>
        /// <param name="client">HttpClient instance</param>
        /// <param name="url">Request URI</param>
        /// <param name="content">Request payload</param>
        /// <returns>Response message</returns>
        public static async Task<HttpResponseMessage> NetPatchAsync(this HttpClient client, string url, HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, url)
            {
                Content = content
            };
            return await client.SendAsync(request);
        }


        /// <summary>
        /// Provides a synchronous implementation of <seealso cref="NetPatchAsync(HttpClient, string, HttpContent)"/> for Framework 4.8 and NetStandard
        /// </summary>
        /// <param name="client">HttpClient instance</param>
        /// <param name="url">Request URI</param>
        /// <param name="content">Request payload</param>
        /// <returns>Response message</returns>
        public static HttpResponseMessage NetPatch(this HttpClient client, string url, HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, url)
            {
                Content = content
            };

            return FireMessageSync(client, request);
        }

        /// <summary>
        /// Platform compatible method to synchronously send a post request
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static HttpResponseMessage NetPost(this HttpClient client, string url, HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            return FireMessageSync(client, request);
        }

        /// <summary>
        /// Platform compatible method to synchronously fire a message.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private static HttpResponseMessage FireMessageSync(HttpClient client, HttpRequestMessage request)
        {
            // I am not a fan of compiler macros,
            // but the inconsistent support across .net versions doesn't leave me much choice
#if NET48 || NETSTANDARD2_0

            // Why does net 4.8 and NetStandard not even have a synchronous send method, only async?

            // Call into another method to make use of compatibility apparent in any potential stack-traces
            return CompatibleHandleMessage(client, request);
#else
            return client.Send(request);
#endif
        }

        /// <summary>
        /// Provides a platform compatible method to synchronously read a response as a stream
        /// </summary>
        public static Stream NetReadAsStream(this HttpContent content)
        {
#if NET5_0_OR_GREATER
            return content.ReadAsStream();
#else
            var task = content.ReadAsStreamAsync();

            task.Wait();

            if (task.IsFaulted)
            {
                throw task.Exception;
            }

            return task.Result;
#endif
        }


        /// <summary>
        /// Synchronously sends a request using <seealso cref="HttpClient.SendAsync(HttpRequestMessage)"/>, blocking the calling thread until it is completed
        /// </summary>
        /// <param name="client">HttpClient instance</param>
        /// <param name="request">Message to send</param>
        /// <returns>Http response</returns>
        private static HttpResponseMessage CompatibleHandleMessage(HttpClient client, HttpRequestMessage request)
        {
            var task = client.SendAsync(request);

            task.Wait();

            if (task.IsFaulted)
            {
                throw task.Exception;
            }

            return task.Result;
        }
    }
}
