using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RtmHub.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using RtmHub.DataAccess;

namespace RtmHub.Hubs
{
    public static class UserHandler
    {
        public static List<ClientWrapper> client = new List<ClientWrapper>();
        public static Dictionary<ClientWrapper, ClientWrapper> dict = new Dictionary<ClientWrapper, ClientWrapper>();
        public static readonly HttpClient httpclient = new HttpClient();
    }

    public class ChatHub : Hub
    {
        public async Task SendMessage(string message)
        {
            ClientWrapper c = UserHandler.client.Find(x => x.Client.Connectionid == Context.ConnectionId);

            DataAccess.DataAccess da = new DataAccess.DataAccess();
            

            if (c.Client.Designation == "user")
            {
                if (UserHandler.dict.TryGetValue(c, out ClientWrapper a))
                {
                

                    Conversation converse = new Conversation();

                    converse.To = c.Client;
                    converse.From = a.Client;
                    converse.Text = message;
                    converse.TimeStamp = DateTime.Now.ToString();

                    MongoTicket ExistTicket = da.GetTicket(c.TicketId);
                    

                    if (ExistTicket == null)
                    {
                        MongoTicket ticket = new MongoTicket() { TicketId = c.TicketId, Conversations = new List<Conversation>() { converse} };
                        da.PostTicket(ticket);
                    }
                    else
                    {
                        ExistTicket.Conversations.Add(converse);
                        da.PutTicket(ExistTicket);
                    }

                    await Clients.Client(a.Client.Connectionid).SendAsync("ReceiveMessage",converse);
                    await Clients.Caller.SendAsync("ReceiveMessage", converse);
                }
            }
            else
            {
                foreach (KeyValuePair<ClientWrapper, ClientWrapper> kv in UserHandler.dict)
                {
                    if (kv.Value == c)
                    {
                        

                        Conversation converse = new Conversation();
                        converse.To =  c.Client;
                        converse.From = kv.Key.Client;
                        converse.Text = message;
                        converse.TimeStamp = DateTime.Now.ToString();

                        MongoTicket ExistTicket = da.GetTicket(c.TicketId);

                        if (ExistTicket == null)
                        {
                            MongoTicket ticket = new MongoTicket() { TicketId = c.TicketId, Conversations = new List<Conversation>() { converse } };
                            da.PostTicket(ticket);
                        }
                        else
                        {
                            ExistTicket.Conversations.Add(converse);
                            da.PutTicket(ExistTicket);
                        }

                        await Clients.Client(kv.Key.Client.Connectionid).SendAsync("ReceiveMessage",converse);
                        await Clients.Caller.SendAsync("ReceiveMessage",converse);

                    }
                }
            }
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Config(string userInput, string designation , string query)
        {

            ClientWrapper clientWrapper = new ClientWrapper();

            if (designation == "user")
            {

                var enduserrespone = await UserHandler.httpclient.GetAsync("http://35.221.125.153:8082/api/endusers/query?Email=" + userInput);
                var enduserresult = await enduserrespone.Content.ReadAsStringAsync();

                EndUser enduser = JsonConvert.DeserializeObject<EndUser>(enduserresult);

                if (enduser != null)
                {
                    Client c = new Client();
                    c.Clientemail = userInput;
                    c.Clientimageurl = enduser.Profile_img_url;
                    c.Clientname = enduser.Name;
                    c.Designation = designation;
                    c.Connectionid = Context.ConnectionId;
                    c.Query = query;

                    ChatDto bodyobj = new ChatDto();
                    bodyobj.Userid = enduser.endUserId;
                    bodyobj.Description = c.Query;
                    bodyobj.Customerhandle = "StackRoute";
                    bodyobj.Connectionid = c.Connectionid;
                    

                    HttpRequestMessage postMessage = new HttpRequestMessage(HttpMethod.Post, "http://35.221.125.153:8083/api/Tickets")
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(bodyobj), UnicodeEncoding.UTF8, "application/json")
                    };

                    var response = await UserHandler.httpclient.SendAsync(postMessage);
                    var responseString = await response.Content.ReadAsStringAsync();

                    string[] responsesplit = responseString.Split(',');
                    int ticketid = Convert.ToInt16((responsesplit[0].Split(':'))[1]);

                    clientWrapper.TicketId = ticketid;
                    clientWrapper.Client = c;

                    UserHandler.client.Add(clientWrapper);
                }

                else
                {
                    await Clients.Caller.SendAsync("ErrorMessage");
                }
            }
              
            if (designation == "agent")
            {

                var agentrespone = await UserHandler.httpclient.GetAsync("http://35.221.125.153:8082/api/agents/query?Email=" + userInput);
                var agentresult = await agentrespone.Content.ReadAsStringAsync();
                Agent agent = JsonConvert.DeserializeObject<Agent>(agentresult);

                if(agent != null) { 
                if (agent != null) { }
                Client c = new Client();
                c.Clientemail = userInput;
                c.Clientimageurl = agent.Profile_img_url;
                c.Clientname = agent.Name;
                c.Designation = designation;
                c.Connectionid = Context.ConnectionId;
                c.Query = query;

                ClientWrapper user = UserHandler.client.Find(x => x.Client.Connectionid == c.Query);
                clientWrapper.Client = c;
                clientWrapper.TicketId = user.TicketId;
                UserHandler.client.Add(clientWrapper);
                UserHandler.dict.Add(user, clientWrapper);
                }

                else
                {
                    await Clients.Caller.SendAsync("ErrorMessage");
                }
            }

        }

    }
}


