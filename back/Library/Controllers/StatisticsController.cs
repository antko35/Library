using Library.Application.Use_Cases.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("Statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly GetUserStat _getUserStat;
        public StatisticsController(GetUserStat getUserStat)
        {
            _getUserStat = getUserStat;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _getUserStat.Execute();
            return Ok();
        }
    }
}
