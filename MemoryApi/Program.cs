using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Server;

namespace MemoryApi
{
    public class Program
    {
        public static IConfigurationRoot Configuration;

        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            bool console = !string.IsNullOrEmpty(Configuration["Console"]);
            var urls = Configuration["Url"];
            if (string.IsNullOrEmpty(urls)) throw new InvalidOperationException(
                "The configuration does not contain the 'Url' section.");

            var host = new WebHostBuilder()
                .UseWebListener(options =>
                {
                    //remove these two lines to suppress the memory leak...
                    options.ListenerSettings.Authentication.Schemes = AuthenticationSchemes.NTLM;
                    options.ListenerSettings.Authentication.AllowAnonymous = false;
                })
                .UseUrls(urls.Split(','))
                .UseStartup<Startup>()
                .Build();

            if (console) host.Run();
            else host.RunAsService();
        }
    }
}
