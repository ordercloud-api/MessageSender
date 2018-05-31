using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderCloud.AzureStorage;

namespace OrderCloudMessageSender.Common
{
	public static class IoC
	{
		public static IServiceProvider RegisterServices(IServiceCollection services)
		{
			var builder = new ConfigurationBuilder();
			builder.AddEnvironmentVariables();
			var settings = builder.Build().Get<AppSettings>();
			services.AddSingleton(new OrderCloud.AzureStorage.TableService(settings.StorageConnection));
			services.AddSingleton<IAppSettings>(settings);
			services.AddTransient<IMessageLog, AzureTableLogService>();
			services.AddTransient<IConfigService, AzureTableConfigService>();
			services.AddTransient<IMandrillSend, MandrillSend>();
			services.AddSingleton<BlobService>(new BlobService(settings.StorageConnection));
			var provider = services.BuildServiceProvider();
			return provider;
		}
	}
}
