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
    public class HealthController : ControllerBase
    {

        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            _logger.LogInformation("All system operational Commander !");
            return "All system operational Commander !";
        }
    }
}
