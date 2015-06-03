using System;
using System.Diagnostics;
using System.Net;
using Carcassonne_Desktop.Models.NetModels.GameModels;
using Carcassonne_Desktop.resources;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;

namespace Carcassonne_Desktop.Models.NetModels
{
    internal class WebGameConnection
    {
        public Action<string> Closed;
        public Action<string> Failed;
        public Action<string> Opened;

        public Action<Turn> ReceivedTurn;
        private Guid GameId { get; set; }
        public Game Game { get; set; }
        private IHubProxy HubProxy { get; set; }
        private HubConnection Connection { get; set; }

        public void SendTurn(Turn t)
        {
            HubProxy.Invoke("PlayTurn", t);
        }

        public void SendScores(Game g)
        {
            HubProxy.Invoke("UpdateScores", g);
        }

        /// <summary>
        ///     Creates and connects the hub connection and hub proxy. This method
        ///     is called asynchronously from Connect().
        /// </summary>
        private async void ConnectAsync()
        {
            Connection = new HubConnection(URLResource.GameConnectionURL)
            {
                JsonSerializer = { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }
            };

            Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy("GameHub");

            HubProxy.On<Turn>("PlayedTurn", turn =>
                ReceivedTurn.Invoke(turn)
                );

            Connection.CookieContainer = new CookieContainer();
            Connection.CookieContainer.Add(new Cookie
            {
                Name = "BearerToken",
                Value = UserState._getInstance().GetUser().Token.AccessToken,
                Domain = new Uri(URLResource.GameConnectionURL).Host
            });

            try
            {
                await Connection.Start();
                Game = await HubProxy.Invoke<Game>("ConnectGame", GameId);

                if (Opened != null)
                    Opened.Invoke("Connected to server at " + URLResource.GameConnectionURL);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                if (Failed != null)
                    Failed.Invoke("Unable to connect to server.");
            }
        }

        /// <summary>
        ///     If the server is stopped, the connection will time out after 30 seconds (default), and the
        ///     Closed event will fire.
        /// </summary>
        private void Connection_Closed()
        {
            if (Closed != null)
                Closed.Invoke("Connection is closed");
        }

        public void Connect(Guid gameId)
        {
            GameId = gameId;
            //Connect to server (use async method to avoid blocking UI thread) 
            ConnectAsync();
        }

        public void Close()
        {
            if (Connection != null)
            {
                Connection.Stop();
                Connection.Dispose();
            }
        }
    }
}