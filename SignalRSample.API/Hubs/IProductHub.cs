using SignalRSample.API.Models;
using System.Threading.Tasks;

namespace SignalRSample.API.Hubs
{
    public interface IProductHub
    {
        Task ReceiveProduct(Product product);
    }
}
