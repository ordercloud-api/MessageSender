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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrderCloudMessageSender.Common;

namespace OrderCloudMessageSender
{
    public class Startup
    {
		private IServiceProvider _provider;
        public Startup(IHostingEnvironment env)
        {
	        
			
        }

        public void ConfigureServices(IServiceCollection services)
        {
	        services.AddTransient<JsonExceptionMiddleware>();
			_provider = IoC.RegisterServices(services);
			services.AddMvc();
        }
		
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
			app.Use(async (context, next) =>
			{
				var messageConfigReader = _provider.GetService<IConfigService>();
				var configid = context.Request.Query["configid"];
				var config = await messageConfigReader.GetMessageConfigAsync(configid);

				string body = await new StreamReader(context.Request.Body).ReadToEndAsync();
				var keyBytes = Encoding.UTF8.GetBytes(config.OcHashKey);
				var dataBytes = Encoding.UTF8.GetBytes(body);
				var hmac = new HMACSHA256(keyBytes);
				var hmacBytes = hmac.ComputeHash(dataBytes);
				var hash = Convert.ToBase64String(hmacBytes);
				
				if (context.Request.Headers["X-oc-hash"] != hash)
				{
					var error = "the sender is not validated. OCHashKey must match the shared key in the OrderCloud Message sender config.";
					var log = _provider.GetService<IMessageLog>();
					await log.LogAsync(configid, "mandrill", error, null);
					context.Response.StatusCode = 401;
					await context.Response.WriteAsync(error);
				}
				else
				{
					context.Request.Body = new MemoryStream(dataBytes);
					await next();
				}
			});

			app.UseExceptionHandler(new ExceptionHandlerOptions
	        {
		        ExceptionHandler = _provider.GetService<JsonExceptionMiddleware>().Invoke
	        });
			app.UseMvc();
        }
	}
}
