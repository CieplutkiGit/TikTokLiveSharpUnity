﻿using Newtonsoft.Json.Linq;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TikTokLiveSharp.Client.Proxy;
using TikTokLiveSharp.Client.Requests;
using TikTokLiveSharp.Errors.Permissions;
using TikTokLiveSharp.Models;
using TikTokLiveSharp.Utils;

namespace TikTokLiveSharp.Networking
{
    internal class TikTokHTTPClient
    {
        #region Properties
        private static int concurrentClients = 0;

        private int clientNum;
        #endregion

        #region Constructors
        internal TikTokHTTPClient(TimeSpan timeout, RotatingProxy proxyHandler = null)
        {
            TikTokHttpRequest.Timeout = timeout;
            TikTokHttpRequest.WebProxy = proxyHandler;
            concurrentClients++;
            clientNum = concurrentClients;
        }

        ~TikTokHTTPClient()
        {
            concurrentClients--;
        }
        #endregion

        #region Methods
        #region Internal
        internal async Task<WebcastResponse> GetDeserializedMessage(string path, Dictionary<string, object> parameters, bool signURL = false)
        {
            var get = await GetRequest(Constants.TIKTOK_URL_WEBCAST + path, parameters, signURL);
            return Serializer.Deserialize<WebcastResponse>(await get.ReadAsStreamAsync());
        }

        internal async Task<JObject> GetJObjectFromWebcastAPI(string path, Dictionary<string, object> parameters, bool signURL = false)
        {
            var get = await GetRequest(Constants.TIKTOK_URL_WEBCAST + path, parameters, signURL);
            return JObject.Parse(await get.ReadAsStringAsync());
        }

        internal async Task<string> GetLivestreamPage(string uniqueID, bool signURL = false)
        {
            var get = await GetRequest($"{Constants.TIKTOK_URL_WEB}@{uniqueID}/live/", signURL: signURL);
            return await get.ReadAsStringAsync();
        }

        internal async Task<JObject> PostJObjecttToWebcastAPI(string path, Dictionary<string, object> parameters, JObject json, bool signURL = false)
        {
            var post = await PostRequest(Constants.TIKTOK_URL_WEBCAST + path, json.ToString(Newtonsoft.Json.Formatting.None), parameters, signURL);
            return JObject.Parse(await post.ReadAsStringAsync());
        }
        #endregion

        #region Private
        private async Task<HttpContent> GetRequest(string url, Dictionary<string, object> parameters = null, bool signURL = false)
        {
            var request = BuildRequest(signURL ? await GetSignedUrl(url, parameters) : url, signURL ? null : parameters);
            return await request.Get();
        }

        private async Task<HttpContent> PostRequest(string url, string data, Dictionary<string, object> parameters = null, bool signURL = false)
        {
            var request = BuildRequest(signURL ? await GetSignedUrl(url, parameters) : url, signURL ? null : parameters);
            return await request.Post(new StringContent(data, Encoding.UTF8));
        }

        private ITikTokHttpRequest BuildRequest(string url, Dictionary<string, object> parameters = null)
        {
            return new TikTokHttpRequest(url).SetQueries(parameters);
        }

        private async Task<string> GetSignedUrl(string url, Dictionary<string, object> parameters = null)
        {
            string getParams = parameters != null ? $"?{string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"))}" : string.Empty;
            ITikTokHttpRequest request = new TikTokHttpRequest(Constants.TIKTOK_SIGN_API)
                .SetQueries(new Dictionary<string, object>()
                {
                    { "client", "ttlive-net" },
                    { "uuc", clientNum },
                    { "url", url + getParams }
                });
            HttpContent content = await request.Get();
            try
            {
                JObject jObj = JObject.Parse(await content.ReadAsStringAsync());
                string s2 = jObj["signedUrl"].Value<string>();
                string signedUrl = jObj["signedUrl"].Value<string>();
                string userAgent = jObj["User-Agent"].Value<string>();
                TikTokHttpRequest.CurrentHeaders.Remove("User-Agent");
                TikTokHttpRequest.CurrentHeaders.Add("User-Agent", userAgent);
                return signedUrl;
            }
            catch (Exception e)
            {
                throw new InsufficientSigningException("Insufficent values have been supplied for signing. Likely due to an update. Post an issue on GitHub.", e);
            }
        }
        #endregion
        #endregion
    }
}