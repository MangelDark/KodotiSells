using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
      
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IInvoiceService _service;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IInvoiceService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result =  _service.GetAll();
            return Ok(result);
        }
    }
}
