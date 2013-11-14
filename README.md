.NET library for loader.io
==============
loader.io is a .NET library for use with the [loader.io](http://loader.io/) service, a simple and powerful cloud-based load testing platform.  Using loader.io, you can quickly, easily, and freely simulate large numbers of connections to your web apps and ensure performance.

## Usage
First, you'll need to retrieve the library and save it to your local hard drive. The preferred method to accomplish this is to install [NuGet](http://nuget.org/) and then via any of its many interfaces install the [loader.io package](https://www.nuget.org/packages/Loaderio).

If you'd prefer to build the library yourself, you can fork or clone it and build it within Visual Studio 2013. Drop the file *Loaderio.dll* into your application's bin directory and you're just about ready to start load testing.

Use [docs.loader.io](http://docs.loader.io) to get more description of the whole API. Especially, options for <code>CreateTest</code> are described in [appropriate documentation section](http://docs.loader.io/api/v2/post/tests.html#test-options).

This example shows usage of all API-calls.

```CSharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Loaderio;

namespace foobar
{
    class Program
    {
        static async void CreateApplication()
        {
            var client = new LoaderioClient("LOADERIO_API_KEY");
            var result = await client.CreateApplication("YOUR_DOMAIN.COM");

            if (result.IsSuccess)
            {
                Console.WriteLine("AppId: {0}", result.AppId);
            }
            else
            {
                result.Errors.ForEach(Console.WriteLine);
            }
        }

        static async void CreateTest()
        {
            var client = new LoaderioClient("LOADERIO_API_KEY");
            var url = new UrlOptions();
            url.Url = "http://YOUR_DOMAIN.COM/URL";

            var url1 = new UrlOptions { Url = "http://YOUR_DOMAIN.COM/PATH1" };
            var url2 = new UrlOptions
            {
                Url = "http://YOUR_DOMAIN.COM/PATH2",
                RequestType = HttpMethod.POST,
                Credentials = new Credentials { Login = "user", Password = "pass" },
                Headers = new Dictionary<string, string> {{ "Content-Type", "application/json" }},
                RequestParams = new Dictionary<string, string> {{ "q", "test" }}
                // or use:
                // RawPostBody = "name=John"
                // or use:
                // PayloadFileUrl = "http://YOUR-DOMAIN.COM/PATH-TO-PAYLOAD-FILE.json"
            };
            var options = new TestOptions
            {
                // obligatory part
                TestType = TestType.NonCycling,
                Duration = 60,
                Initial = 0,
                Total = 100,
                Urls = new UrlOptions[] { url1, url2 },
                // optional part
                Timeout = 60,
                ErrorThreshold = 50,
                Callback = "http://YOUR-CALLBACK-DOMAIN.COM/PATH",
                CallbackEmail = "TEAM@YOUR-EMAIL.COM",
                // if you want to run the test a little bit later:
                // ScheduledAt = DateTime.Now.AddMinutes(10),
                Name = "My KillerTest",
                Notes = "Run via .Net SDK"
            };
            
            var result = await client.CreateTest(options);

            if (result.IsSuccess)
            {
                Console.WriteLine("TestId: {0}", result.TestId);
                Console.WriteLine("Status: {0}", result.Status);
                Console.WriteLine("ResultId: {0}", result.ResultId);
            }
            else
            {
                result.Errors.ForEach(Console.WriteLine);
            } 
        }

        static async void RunTest()
        {
            var client = new LoaderioClient("LOADERIO_API_KEY");
            var result = await client.RunTest("TEST_ID");

            if (result.IsSuccess)
            {
                Console.WriteLine("TestId: {0}", result.TestId);
                Console.WriteLine("Status: {0}", result.Status);
                Console.WriteLine("ResultId: {0}", result.ResultId);
            }
            else
            {
                result.Errors.ForEach(Console.WriteLine);
            }
        }
    }
}

```
