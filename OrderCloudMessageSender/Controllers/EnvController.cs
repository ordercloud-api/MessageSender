using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderCloudMessageSender.Common;

namespace OrderCloudMessageSender.Controllers
{
    public class EnvController : Controller
    {
	    private readonly IAppSettings _appSettings;

	    public EnvController(IAppSettings appSettings)
	    {
		    _appSettings = appSettings;
	    }
		[Route("env"), HttpGet]
		public object GetEnv()
		{
			return new {BUILD_NUMBER = _appSettings.BUILD_NUMBER, BUILD_COMMIT_ID = _appSettings.BUILD_COMMIT_ID};
		}
    }
}
