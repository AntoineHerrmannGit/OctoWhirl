using Microsoft.AspNetCore.Mvc;
using OctoWhirl.BusinessServices.Data.Core;
using OctoWhirl.Core.Models.Requests;
using OctoWhirl.Exceptions;
using OctoWhirl.Models.Technicals;
using Technicals.Extensions;

namespace OctoWhirl.BusinessServices.Data.App.Controllers
{
    [ApiVersion("1")]
    [Route("data")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IBusinessDataService _dataService;

        public DataController(IBusinessDataService dataService) 
        { 
            _dataService = dataService;
        }

        [HttpPost("spots")]
        [ProducesResponseType(typeof(List<CandleSerie>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSpots(GetCandlesRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.StartDate.IsNullOrDefault())
                throw new ArgumentNullOrDefaultException(nameof(request.StartDate));

            if (request.EndDate.IsNullOrDefault())
                throw new ArgumentNullOrDefaultException(nameof(request.EndDate));

            return Ok(await _dataService.GetCandles(request));
        }
    }
}
