using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace realtimechat.Hubs
{
    public class ChatHub:Hub
    {
        public async Task MessageSender(string user, string message)
        {
            try
            {
                var previousUser = Context.GetHttpContext().Session.GetString("user");
                Context.GetHttpContext().Session.SetString("user", user);
            }
            catch (System.Exception e)
            {

                throw;
            }
            await Clients.All.SendAsync("MessageReceiver", user,
           message);
        }

    }
}
