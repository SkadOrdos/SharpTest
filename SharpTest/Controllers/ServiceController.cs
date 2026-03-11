using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebSharp.Services;

namespace WebSharp.Controllers
{
    [ApiController]
    [EnableCors()]
    [Route("Service")]
    [Produces("application/json")]
    public class ServiceController : ControllerBase
    {
        private readonly ILogger<ServiceController> _logger;
        private readonly IFileService fileService;

        public ServiceController(IFileService fService, ILogger<ServiceController> logger)
        {
            fileService = fService;
            _logger = logger;
        }


        [HttpGet("GetDigits")]
        public ActionResult GetDjigits()
        {
            return Ok(fileService.Load());
        }

        [HttpGet("GetQuantille")]
        public ActionResult GetQuantille(double tau)
        {
            var service = (FileService)fileService;
            return Ok($"Quantille: {service.GetQuantille(tau).Result}; Data: [{String.Join(",", service.ArrayCache.Data)}]");
        }

        [HttpPost("Update")]
        public ActionResult Update(TargetsDTO targets)
        {
            if (targets.Targets == null) return BadRequest(nameof(Update));

            if (MessageDispatcher.Instance.DoAction(new TagretsMessage(targets.Targets.ToArray())) == DispatcherStatus.Completed)
                return Ok("Successful");

            return Accepted();
        }
    }
}
