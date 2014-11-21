using Microsoft.AspNet.SignalR;

namespace RealTimeHost
{
    public sealed class ProductHub : Hub
    {
        public void NotifyProductCreated(string productName)
        {
            Clients.All.productAdded(productName);
        }
    }
}