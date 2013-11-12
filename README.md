.NET library for loader.io
==============
loader.io is a .NET library for use with the [loader.io](http://loader.io/) simple and powerful cloud-based load testing tool.  Using loader.io, you can quickly, easily, and freely simulate large numbers of connections to your web apps and ensure performance.

## Usage
First, you need to get the library down from the internets and onto your local hard drive. The preferred method to accomplish this is to install [NuGet](http://nuget.org/) and then via any of its many interfaces install the [LoaderIo Package](https://www.nuget.org/packages/Loaderio).

If you want to build the library yourself, you can fork or clone it and build it with Visual Studio 2013. Drop the file *Loaderio.dll* into your application's bin directory and you're almost good to go.

```CSharp
using Loaderio;

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

    var options = new TestOptions();
    options.TestType = TestType.NonCycling;
    options.Duration = 60;
    options.Initial = 0;
    options.Total = 60;

    options.Urls = new UrlOptions[] { url };
    
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

```