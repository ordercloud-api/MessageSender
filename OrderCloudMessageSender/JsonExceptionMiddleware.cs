using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OrderCloudMessageSender.Common;

namespace OrderCloudMessageSender
{
	public class JsonExceptionMiddleware
	{
		private readonly IMessageLog _log;

		public JsonExceptionMiddleware(IMessageLog log)
		{
			_log = log;
		}
		public async Task Invoke(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			await _log.LogAsync("", "unhandled", context.Features.Get<IExceptionHandlerFeature>()?.Error.InnerException.ToString(), null);

			object getError(Exception ex)
			{
				if (ex == null) return null;
				
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				return new[] { new { ErrorCode = "UnhandledException", Message = ex.Message } };
				

			}
			using (var writer = new StreamWriter(context.Response.Body))
			{
				new JsonSerializer().Serialize(writer, getError(context.Features.Get<IExceptionHandlerFeature>()?.Error));
				await writer.FlushAsync().ConfigureAwait(false);
			}
		}
	}
}
