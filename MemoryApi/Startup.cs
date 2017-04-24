using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace MemoryApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    //prevents the serializer from changing the field names to camel case
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .AddXmlDataContractSerializerFormatters();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime lifetime)
        {
            app.UseMvcWithDefaultRoute();
        }
    }
}
