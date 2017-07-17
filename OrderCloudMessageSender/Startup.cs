using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace OrderCloudMessageSender
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
	        loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
			
	        app.Use(async (context, next) =>
	        {
				string body = await new StreamReader(context.Request.Body).ReadToEndAsync();
		        var keyBytes = Encoding.UTF8.GetBytes(Configuration["OCHashKey"]);
		        var dataBytes = Encoding.UTF8.GetBytes(body);
		        var hmac = new HMACSHA256(keyBytes);
		        var hmacBytes = hmac.ComputeHash(dataBytes);
		        var hash = Convert.ToBase64String(hmacBytes);

		        if (context.Request.Headers["X-oc-hash"] != hash)
		        {
			        context.Response.StatusCode = 401;
			        await context.Response.WriteAsync("the sender is not validated. OCHashKey in appsettings.json must match the shared key in the OrderCloud Message sender config.");
			        //throw new Exception("The signature does not match. The sender is not verified.");
		        }
		        else
		        {
			        context.Request.Body = new MemoryStream(dataBytes);
			        await next();
				}
	        });

            app.UseMvc();
        }
	}
}
