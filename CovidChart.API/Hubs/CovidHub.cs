using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CovidChart.API.Hubs
{
    public class CovidHub:Hub<IHubCovid>
    {
        private readonly CovidService _covidService;

        public CovidHub(CovidService covidService)
        {
            _covidService = covidService;
        }

        public async Task GetCovidList()
        {
            await Clients.All.ReceiveCovidList(_covidService.GetCovidChartList());
        }
    }
}
