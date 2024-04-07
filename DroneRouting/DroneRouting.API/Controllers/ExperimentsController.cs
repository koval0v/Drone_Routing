using DroneRouting.Experiments;
using DroneRouting.Experiments.Models;
using DroneRouting.Tasks.Models;
using Microsoft.AspNetCore.Mvc;

namespace DroneRouting.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExperimentsController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ExperimentStatistics>> ReturnExperimentStatistics(TasksSettings settings)
        {
            var results = await new ExperimentsGenerator(settings).AnalyseExperimentResults();
            return Ok(results);
        }
    }
}