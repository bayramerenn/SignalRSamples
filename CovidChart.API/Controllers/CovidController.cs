using CovidChart.API.Hubs;
using CovidChart.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CovidChart.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CovidController : ControllerBase
    {
        private readonly CovidService _covidService;

        public CovidController(CovidService covidService)
        {
            _covidService = covidService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await _covidService.SendCovid(covid);
            var covidList = _covidService.GetList();
            return Ok(covidList);
        }
        [HttpGet]
        public IActionResult InitializeCovid()
        {
            Random random = new Random();
            Enumerable.Range(1, 10).ToList().ForEach(x =>
             {
                 foreach (ECity city in Enum.GetValues(typeof(ECity)))
                 {
                     var newCovid = new Covid
                     {
                         ECity = city,
                         Count = random.Next(100, 1000),
                         CovidDate = DateTime.Now.AddDays(x)
                     };
                     _covidService.SendCovid(newCovid).Wait();
                     Thread.Sleep(1000);
                 }
             });

            return Ok("datalar veritabanina kaydedilmistir");
        }

        [HttpGet("GetCovidList")]
        public IActionResult GetCovidList()
        {
            return Ok(_covidService.GetCovidChartList());
        }
    }
}
