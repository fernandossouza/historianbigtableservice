using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using historianbigtableservice.Service;
using historianbigtableservice.Service.Interface;
using historianbigtableservice.Model;
using System;

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

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int thingId, [FromQuery]long startDate, [FromQuery]long endDate)
        {
            try{

                 var (a,b) = await _historianService.GetHistorian(thingId,startDate,endDate);

            if(a!=null)
            {
                return Ok(a);
            }

            if(string.IsNullOrEmpty(b))
            {
                return NotFound();
            }

            return NoContent();


            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
           
           
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TagInput tag)
        {
            if (ModelState.IsValid)
            {
               var (a,b) = await _historianService.AddHistorian(tag);

                if(a==true)
            {
                return Ok(tag);
            }

            if(string.IsNullOrEmpty(b))
            {
                return NotFound();
            }

            return NoContent();

            }
            return BadRequest(ModelState);

        }


    }
}