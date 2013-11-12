using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Loaderio
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }

        // Raw HTTP response data
        public HttpStatusCode _StatusCode { get; set; }
        public string _Body { get; set; }

        // CreateApplication result fields
        public string AppId {
            get { return app_id; }
        }
        public string app_id;

        // CreateTest, RunTest result fields
        public string TestId
        {
            get { return test_id; }
        }
        public string test_id;
        public string Status { get; set; }
        public string ResultId
        {
            get { return result_id; }
        }
        public string result_id;
    }

    public class LoaderioClient
    {
        private readonly string ApiKey;
        public string ServerUri { get; set; }

        public LoaderioClient(string apiKey)
        {
            ApiKey = apiKey;
            ServerUri = "https://api.loader.io/v2";
        }

        public async Task<Result> CreateApplication(string name)
        {
            var uri = ServerUri + "/apps";
            var values = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("app", name) };
            return await DoApiCall(HttpMethod.POST, uri, values);
        }

        public async Task<Result> CreateTest(TestOptions options)
        {
            var uri = ServerUri + "/tests";
            var preparedParams = PrepareParams(options);
            return await DoApiCall(HttpMethod.POST, uri, preparedParams);
        }

        public async Task<Result> RunTest(string TestId)
        {
            var uri = ServerUri + "/tests/" + TestId + "/run";
            return await DoApiCall(HttpMethod.PUT, uri, null);
        }

        private async Task<Result> DoApiCall(HttpMethod method, string uri, List<KeyValuePair<string, string>> values)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("loaderio-auth", ApiKey);
            FormUrlEncodedContent content = null;
            if (values != null)
            {
                content = new FormUrlEncodedContent(values);
            }
            HttpResponseMessage response = null;
            if (method == HttpMethod.POST)
            {
                response = await client.PostAsync(uri, content);
            }
            else if (method == HttpMethod.PUT)
            {
                response = await client.PutAsync(uri, content);
            }
            return await ParseJsonResponse(response);
        }

        private async Task<Result> ParseJsonResponse(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();
            Result result = null;
            try
            {
                result = new JavaScriptSerializer().Deserialize<Result>(json);
                result.IsSuccess = response.IsSuccessStatusCode;
            }
            catch (ArgumentException e)
            {
                result = new Result();
                result.IsSuccess = false;
                result.Errors = new List<string> { "Invalid JSON-response" };
            }
            result._Body = json;
            result._StatusCode = response.StatusCode;
            return result;
        }

        private List<KeyValuePair<string, string>> PrepareParams(TestOptions options)
        {
            var allParams = new List<KeyValuePair<string, string>>();
            options.AddOptions(allParams);
            return allParams;
        }
    }
}
