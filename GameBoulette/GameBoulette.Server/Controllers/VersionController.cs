using GameBoulette.Shared;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBoulette.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionController : ControllerBase
    {

        private readonly ILogger<VersionController> _logger;

        public VersionController(ILogger<VersionController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            var version = Environment.GetEnvironmentVariable(Constants.Constant.Settings.VersionKey);
            _logger.LogInformation(version ?? "not_defined");
            return version ?? "not_defined";
        }
    }
}
