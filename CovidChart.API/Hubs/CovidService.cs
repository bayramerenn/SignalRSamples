using CovidChart.API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidChart.API.Hubs
{
    public class CovidService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<CovidHub> _hubContext;
        public CovidService(AppDbContext context, IHubContext<CovidHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public IQueryable<Covid> GetList()
        {
            return _context.Covids.AsQueryable();
        }
        public async Task SendCovid(Covid covid)
        {
            await _context.Covids.AddAsync(covid);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveCovidList", GetCovidChartList());
        }
        public List<CovidChartPivot> GetCovidChartList()
        {
           
            var covidChartPivot = _context.CovidChartPivot.FromSqlRaw("SELECT CovidDate =CAST(CovidDate as NVARCHAR(20))," +
                "[Istanbul] = ISNULL([1], 0)," +
                "[Ankara] = ISNULL([2], 0)," +
                "[Izmir] = ISNULL([3], 0)," +
                "[Giresun] = ISNULL([4], 0)," +
                "[Antalya] = ISNULL([5], 0) FROM " +
                    "(SELECT ECity,[Count],CovidDate = CAST(CovidDate AS DATE) FROM dbo.Covids) AS covidT " +
                    "PIVOT" +
                    "(SUM([Count]) FOR ECity IN([1],[2],[3],[4],[5])) AS pTable " +
                    "ORDER BY pTable.CovidDate").ToList();

            return covidChartPivot;
            
        }
    }
}
