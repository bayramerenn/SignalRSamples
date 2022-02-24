using Microsoft.AspNetCore.SignalR;
using SignalRSample.API.Models;
using System.Threading.Tasks;

namespace SignalRSample.API.Hubs
{
    public class ProductHub : Hub<IProductHub>
    {
        public async Task SendProduct(Product product)
        {
            await Clients.All.ReceiveProduct(product);
        }
    }
}
