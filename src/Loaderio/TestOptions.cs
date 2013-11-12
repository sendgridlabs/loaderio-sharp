using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loaderio
{
    public enum TestType
    {
        NonCycling,
        Cycling
    }

    public class TestOptions
    {
        public TestType? TestType { get; set; }
        public UrlOptions[] Urls { get; set; }
        public int? Duration { get; set; }
        public int? Initial { get; set; }
        public int? Total { get; set; }
        public int? Timeout { get; set; }
        public int? ErrorThreshold { get; set; }
        public string Callback { get; set; }
        public string CallbackEmail { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }

        public void AddOptions(List<KeyValuePair<string, string>> list)
        {
            CheckObligatoryFields();
            // obligatory
            StoreKeyValue(list, "test_type", TestTypeToOption());
            foreach (var url in Urls)
            {
                url.AddOptions(list);
            }
            StoreKeyValue(list, "duration", Duration.ToString());
            StoreKeyValue(list, "initial", Initial.ToString());
            StoreKeyValue(list, "total", Total.ToString());
            // optional
            StoreKeyValue(list, "timeout", Timeout);
            StoreKeyValue(list, "error_threshold", ErrorThreshold);
            StoreKeyValue(list, "callback", Callback);
            StoreKeyValue(list, "callback_email", CallbackEmail);
            StoreKeyValue(list, "scheduled_at", ScheduledAt);
            StoreKeyValue(list, "name", Name);
            StoreKeyValue(list, "notes", Notes);
        }

        private string TestTypeToOption()
        {
            switch (TestType.Value)
            {
                case Loaderio.TestType.NonCycling:
                    return "non-cycling";
                case Loaderio.TestType.Cycling:
                    return "cycling";
                default:
                    throw new ArgumentException("Incorrect TestType", TestType.ToString());
            }
        }

        private void StoreKeyValue(List<KeyValuePair<string, string>> list, string key, string value)
        {
            if (value != null)
            {
                list.Add(new KeyValuePair<string, string>(key, value));
            }
        }

        private void StoreKeyValue(List<KeyValuePair<string, string>> list, string key, int? number)
        {
            if (number.HasValue)
            {
                StoreKeyValue(list, key, number.Value.ToString());
            }
        }

        private void StoreKeyValue(List<KeyValuePair<string, string>> list, string key, DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                var svalue = dateTime.Value.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
                StoreKeyValue(list, key, svalue);
            }
        }

        private void CheckObligatoryFields()
        {
            if (!TestType.HasValue)
            {
                throw new ArgumentException("TestOptions.TestType can't be null");
            }
            if (Urls == null)
            {
                throw new ArgumentException("TestOptions.Urls can't be null");
            }
            if (Urls.Length == 0)
            {
                throw new ArgumentException("TestOptions.Urls can't be empty");
            }
            if (!Duration.HasValue)
            {
                throw new ArgumentException("TestOptions.Duration can't be null");
            }
            if (!Initial.HasValue)
            {
                throw new ArgumentException("TestOptions.Initial can't be null");
            }
            if (!Total.HasValue)
            {
                throw new ArgumentException("TestOptions.Total can't be null");
            }
        }
    }
}