using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRSample.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRSample.API.Hubs
{
    public class MyHub:Hub
    {
        private readonly AppDbContext _context;

        public MyHub(AppDbContext context)
        {
            _context = context;
        }

        public static List<string> Names { get; set; } = new List<string>();
        private static int ClientCount { get; set; }
        public static int TeamCount { get; set; } = 7;
        public async Task SendName(string name)
        {
            if (Names.Count >= TeamCount)
            {
                await Clients.Caller.SendAsync("Error", $"Takim en fazla {TeamCount} kisi olabilir");
            }
            else
            {
                Names.Add(name);
                await Clients.All.SendAsync("ReceiveName", name);
            }
            
        }
        public async Task GetNames()
        {
            await Clients.All.SendAsync("ReceiceNames",Names);
        }

        public override async Task OnConnectedAsync()
        {
            ClientCount++;
            await Clients.All.SendAsync("ReceiveClientCount",ClientCount);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ClientCount--;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendProduct(Product product)
        {
            await Clients.All.SendAsync("ReceiveProduct",product);
        }
        //groups
        public async Task AddToGroup(string teamName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, teamName);
        }
        public async Task RemoveToGroup(string teamName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId,teamName);
        }
        public async Task SendNameByGroup(string name,string teamName)
        {
            var team = _context.Teams.Where(x => x.Name == teamName).FirstOrDefault();
            if (team != null)
            {
                team.Users.Add(new User { Name = name });
            }
            else
            {
                var newTeam = new Team { Name = teamName };
                newTeam.Users.Add(new User { Name = name});
                _context.Teams.Add(newTeam);
            }
            await _context.SaveChangesAsync();
            await Clients.Group(teamName).SendAsync("ReceiveMessageByGroup", name, teamName);
        }
        public async Task GetNamesByGroup()
        {
            var teams = _context.Teams.Include(x => x.Users).Select(x => new
            {
                teamName = x.Name,
                Users = x.Users.ToList()
            });
            await Clients.All.SendAsync("ReciveNamesByGroup",teams);
        }
    }
}
