using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using historianbigtableservice.Service;
using historianbigtableservice.Service.Interface;
using historianbigtableservice.Model;

namespace historianbigtableservice.Controllers
{
    [Route("api/[controller]")]
    public class HistorianBigTableController : Controller
    {
        private readonly IHistorianService _historianService;

        public HistorianBigTableController(IHistorianService historianService)
        {
            _historianService = historianService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Tag tag)
        {
            if (ModelState.IsValid)
            {
               var (a,b) = await _historianService.addHistorian(tag);

            }
            return BadRequest(ModelState);

        }
    }
}