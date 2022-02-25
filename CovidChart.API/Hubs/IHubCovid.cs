using CovidChart.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidChart.API.Hubs
{
    public interface IHubCovid
    {
        public Task ReceiveCovidList(List<CovidChartPivot> covidChartPivot);
    }
}
