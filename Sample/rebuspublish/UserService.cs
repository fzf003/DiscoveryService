using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;
namespace resbuspublish
{

    public class ServiceHub:Hub
    {
        public  Task GetService(string name)
        {
            
           
           return this.Clients.All.SendAsync("GetUser", this.Context.ConnectionId);
        }
        
    }


   
}