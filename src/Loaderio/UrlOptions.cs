using System;
using System.Collections.Generic;
using System.Linq;

namespace Loaderio
{
    public enum HttpMethod
    {
        GET,
        POST,
        PUT,
        DELETE,
        PATCH
    }

    public class Credentials
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class UrlOptions
    {
        public string Url { get; set; }
        public HttpMethod? RequestType { get; set; }
        public Credentials Credentials { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> RequestParams { get; set; }
        public string RawPostBody { get; set; }
        public string PayloadFileUrl { get; set; }

        public void AddOptions(List<KeyValuePair<string, string>> list)
        {
            CheckObligatoryFields();
            StoreKeyValue(list, "urls[][url]", Url);
            if (RequestType.HasValue)
            {
                StoreKeyValue(list, "urls[][request_type]", RequestType.Value.ToString());
            }
            if (Credentials != null)
            {
                StoreKeyValue(list, "urls[][credentials][login]", Credentials.Login);
                StoreKeyValue(list, "urls[][credentials][password]", Credentials.Password);
            }
            if (Headers != null)
            {
                foreach (var header in Headers)
                {
                    StoreKeyValue(list, String.Format("urls[][headers][{0}]", header.Key), header.Value);
                }
            }
            if (RequestParams != null)
            {
                foreach (var param in RequestParams)
                {
                    StoreKeyValue(list, String.Format("urls[][request_params][{0}]", param.Key), param.Value);
                }
            }
            StoreKeyValue(list, "urls[][raw_post_body]", RawPostBody);
            StoreKeyValue(list, "urls[][payload_file_url]", PayloadFileUrl);
        }

        private void StoreKeyValue(List<KeyValuePair<string, string>> list, string key, string value)
        {
            if (value != null)
            {
                list.Add(new KeyValuePair<string, string>(key, value));
            }
        }

        private void CheckObligatoryFields()
        {
            if (Url == null)
            {
                throw new ArgumentException("UrlOptions.Url can't be null");
            }
        }

    }
}