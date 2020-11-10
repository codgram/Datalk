using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Datalk.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
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

        private string messageInput { get; set; }

        
        private string typingMessage;
        private string notifyTyping;


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

            hubConnection.On<string, string, string>("ReceiveTyping", (chatroomId, username, typingMessage) => {
                if(chatroomId == Id) {
                    var encodedMsg = typingMessage;
                    notifyTyping = encodedMsg;
                    StateHasChanged();
                }
            });

            await hubConnection.StartAsync();

            if(Id != "fantastic")
                await GetChatroom();
                
            await GetMessages();

            
        }

        async Task SendTyping(ChangeEventArgs e) {
            /****************************
            This method is used to notify the users that someone is typing
            The "ChangeEventArgs" argument is used to track and get 
                the changes that are happening in the input field
            **************************/
            
            // 1 - Get the username of the typing user
            userInput = await GetUserName();
            
            // 2 - Assign the typing message
            if(messageInput != null || messageInput != "")
                typingMessage = username + " is typing";
            else
                typingMessage = "";

            // 3 - Notify other users that someone is typing
            // - If the value's lenght in the input field is greater than 0
            //      update the notification message for the "Other" users with a message saying "{username} is typing"
            // - else update the notification message for the "Other" users with an empty message to remove the previous notification
            if(e.Value.ToString().Length > 0) {
                await hubConnection.SendAsync("SendTypingMessage", Id, userInput, typingMessage);
            }
            else {
                await hubConnection.SendAsync("SendTypingMessage", Id, userInput, "");
            }


        }



        // This method calls the Send() function when the user press enter after typing
        async Task Enter(KeyboardEventArgs e) {
            if(e.Code == "Enter" || e.Code == "NumpadEnter")
                await Send();
        }

        async Task Send()
        {
            if(messageInput != null && messageInput != "" && messageInput.Replace(" ", String.Empty) != "") {

                //userInput = await GetUserName(); // This section is commented because it already gets the userInput in SendTyping() method

                // 1 - Create message in database
                await CreateMessage();

                // 2 - Send message to group using SignalR
                await hubConnection.SendAsync("SendMessage", Id, userInput, messageInput);

                // 3 - Empty the Input field
                messageInput = "";

                // 4 - Remove the typing message notification
                await hubConnection.SendAsync("SendTypingMessage", Id, userInput, "");
            }
            
            
            
            
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