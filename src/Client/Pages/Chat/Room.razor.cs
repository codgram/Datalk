using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Datalk.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;

namespace Datalk.Client.Pages.Chat
{
    public partial class Room
    {
        Message[] dbMessages;
        private Chatroom chatroom;
        private DatalkUser user;
        public class DatalkUser
        {
            public string Id { get; set; }
            public string UserName { get; set; }
        }
        


        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string chatroomName { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private string username;

        private string userId;

        

        private HubConnection hubConnection;

        private List<string> messages = new List<string>();

        private string userInput;

        private string messageInput;

        public async Task GetMessages()
        {
            dbMessages = await Http.GetFromJsonAsync<Message[]>("api/message");
        }

        public async Task GetChatroom()
        {
            chatroom = await Http.GetFromJsonAsync<Chatroom>($"api/chatroom/{Id}");
        }

        protected override async Task OnInitializedAsync()
        {
            Id = Id ?? "fantastic";
            

            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/chathub"))
                .Build();

            hubConnection.On<string, string, string>("ReceiveMessage", (chatroomId, username, message) =>
            {
                if(chatroomId == Id) {
                    var encodedMsg = $"{username}: {message}";
                    messages.Add(encodedMsg);
                    StateHasChanged();
                }
                
                
            });

            await hubConnection.StartAsync();

            if(Id != "fantastic")
                await GetChatroom();
                
            await GetMessages();

            
        }

        async Task Send()
        {
            userInput = await GetUserName();
            await CreateMessage();
            await hubConnection.SendAsync("SendMessage", Id, userInput, messageInput);
            messageInput = "";
        }

        public bool IsConnected =>
            hubConnection.State == HubConnectionState.Connected;

        public void Dispose()
        {
            _ = hubConnection.DisposeAsync();
        }



        private async Task<string> GetUserName()
        {
            var authState = await authenticationStateTask;
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                username = user.Identity.Name;
            }
            return username;

        }

        private async Task<string> GetUserId()
        {
            string api = "api/user/" + await GetUserName();
            user = await Http.GetFromJsonAsync<DatalkUser>(api);
            userId = user.Id;
            return userId;
        }

        public async Task CreateMessage()
        {
            var userId = await GetUserId();
            var username = await GetUserName();
            Message message = new Message()
            {
                DatalkUserId = userId,
                ChatroomId = Id,
                Content = messageInput,
                UserName = username
            };

            await Http.PostAsJsonAsync("api/message", message);
        }

        
    }
}