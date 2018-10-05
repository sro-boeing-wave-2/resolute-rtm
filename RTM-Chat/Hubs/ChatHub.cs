﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Net.Http;
using RTM_Chat.IO;
using Newtonsoft.Json;
using RTM_Chat.Models;
using System.Text;
using RTM_Chat.Services;
using RTM_Chat.Hubs;

namespace RTM_Chat.Hubs
{
    public class ChatHub : Hub
    {
        public string enduserurl = Constants.BASE_URL + ":" + Constants.ENDUSER_URL;
        public string ticketurl = Constants.BASE_URL + ":" + Constants.TICKET_URL;

        public IMessageThreadService _service;

        public ChatHub(IMessageThreadService messageThreadService)
        {
            _service = messageThreadService;
        }

        // Bot Allocator Says
        public void RegisterBotFactory()
        {
            Console.WriteLine("Inside BotFactory");
            BotFactoryManager.Factories.Add(Context.ConnectionId);
            Clients.Client(Context.ConnectionId).SendAsync("Acknowledgement", "Done");
        }

        // Bot Says ..
        public async void AssignMeToUser(string groupId)
        {
            Console.WriteLine("Allocating To User");
            Console.WriteLine(Context.ConnectionId);
            Console.WriteLine(groupId);
            GroupHandler.UserGroupMapper.Add(Context.ConnectionId, groupId);
            Console.WriteLine($"Bot Trying to connect to {groupId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (BotFactoryManager.Factories.Contains(Context.ConnectionId))
            {
                Console.WriteLine("Bot Factory Closing");
                BotFactoryManager.Factories.Remove(Context.ConnectionId);
            }
            else
            {
                try
                {
                    if(ClientHandler.clienthandler[GroupHandler.UserGroupMapper[Context.ConnectionId]].MessageDetails.Last().Name != "Bot")
                        _service.CreateMessageThread(ClientHandler.clienthandler[GroupHandler.UserGroupMapper[Context.ConnectionId]]);
                }catch(Exception ex) { }
            }
            return base.OnDisconnectedAsync(exception);
        }


        // User Says...
        public async Task AllocateMeAnAgent(string userInput, string query)

        {
            if (!query.StartsWith('#'))
            {
                Console.WriteLine(userInput, query);
                //Create a Ticket
                HttpClient httpClient = new HttpClient();
                ChatDto ticketDto = new ChatDto() { Query = query, Useremail = userInput };
                HttpRequestMessage postMessage = new HttpRequestMessage(HttpMethod.Post, ticketurl + "/create")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(ticketDto), UnicodeEncoding.UTF8, "application/json")
                };
                postMessage.Headers.Add("Access", "Allow_Service");

                Console.WriteLine(postMessage);

                var response = await httpClient.SendAsync(postMessage);
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);

                string[] responsesplit = responseString.Split(',');
                string ticketid = responsesplit[0].Split(':')[1].Replace("\"", "");


                //Add to ClientMapper
                ClientHandler.clienthandler[ticketid] = new MessageThread() { TicketId = ticketid, MessageDetails = new List<Message>() };


                var groupId = ticketid;
                Console.WriteLine(groupId);


                GroupHandler.UserGroupMapper.Add(Context.ConnectionId, groupId);
                await Groups.AddToGroupAsync(Context.ConnectionId, groupId);


                var BotFactory = BotFactoryManager.Factory;
                if (!string.IsNullOrEmpty(BotFactory))
                {
                    await Clients.Client(BotFactory).SendAsync("AllocateMeABot", groupId, query);
                }
            }
        }

        public async void SendMessage(Message messageobj)
        {
            if (":resolve".Equals(messageobj.MessageText))
            {
                var groupId = GroupHandler.UserGroupMapper[Context.ConnectionId];
                await Clients.GroupExcept(groupId, Context.ConnectionId).SendAsync("GetFeedback");
            }
            else
            {
                var groupId = GroupHandler.UserGroupMapper[Context.ConnectionId];
                messageobj.Timestamp = DateTime.Now;

                await Clients.GroupExcept(groupId, Context.ConnectionId).SendAsync("message", messageobj);
                ClientHandler.clienthandler[groupId].MessageDetails.Add(messageobj);
                Console.WriteLine(messageobj.MessageText);
            }
        }

        public void Handover(string threadId)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, ticketurl + "/assignagent/"+threadId);
            requestMessage.Headers.Add("Access", "Allow_Service");
            var response =  httpClient.SendAsync(requestMessage);

            var groupId = GroupHandler.UserGroupMapper[Context.ConnectionId];

            Message messageobj = new Message();
            
            messageobj.Name = "bot";
            messageobj.Timestamp = DateTime.Now;
            messageobj.EmailId = "bot@gmail.com";
            messageobj.MessageText = "I'm sorry, I'm not able to find a solution to your query. Let me transfer you to an agent.";
            
            Clients.GroupExcept(groupId , Context.ConnectionId).SendAsync("message", messageobj);
            ClientHandler.clienthandler[groupId].MessageDetails.Add(messageobj);
            Console.WriteLine(messageobj.MessageText);
            Console.WriteLine(response.Result);
        }

        public void Handover2(string threadId){
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, ticketurl + "/assignagent/"+threadId);
            requestMessage.Headers.Add("Access", "Allow_Service");
            var response =  httpClient.SendAsync(requestMessage);

            Message messageobj = new Message();
            
            messageobj.Name = "bot";
            messageobj.Timestamp = DateTime.Now;
            messageobj.EmailId = "bot@gmail.com";
            messageobj.MessageText = "I'm sorry, I'm not able to find a solution to your query. Let me transfer you to an agent.";
            
            Clients.All.SendAsync("message",messageobj);
            ClientHandler.clienthandler[threadId].MessageDetails.Add(messageobj);

        }
    
        public void SolutionEnd(){
            var groupId = GroupHandler.UserGroupMapper[Context.ConnectionId];
            Clients.GroupExcept(groupId, Context.ConnectionId).SendAsync("GetFeedback");
        }

        public void SetFeedback(int feedback){
            var groupId = GroupHandler.UserGroupMapper[Context.ConnectionId];
            Console.WriteLine(""+feedback+groupId);
            if(feedback == 5){
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, ticketurl +"/"+groupId + "?status=close&feedbackscore="+feedback);
            requestMessage.Headers.Add("Access", "Allow_Service");
            var response =  httpClient.SendAsync(requestMessage);
            }

            else if(feedback == 1){
                Handover2(groupId);
            }
        } 

        public List<Message> GetConversation(string threadId){
            List<Message> result = _service.GetListMessageThread(threadId);
            return result;
        }
    }
}
