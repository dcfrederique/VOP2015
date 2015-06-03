using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carcassonne_Web.DAL;
using Carcassonne_Web.Models;
using Carcassonne_Web.Models.GameObj;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Json;
using Newtonsoft.Json;

namespace Carcassonne_Web.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly IGameRepository _gameRepository;
        private ILogRepository logRepo;

        public GameHub()
        {
            var context = new CarcassonneContext();
            _gameRepository = new GameRepository(context);
            logRepo = new LogRepository(context);
        }

        public Game ConnectGame(Guid g)
        {
            Groups.Add(Context.ConnectionId, g.ToString());
            var game = _gameRepository.GetGameByID(g);


            logRepo.InsertLog(new Log()
            {
                Category = LogType.Game,
                CategoryAttribute = game.GameID.ToString(),
                Date = DateTime.Now,
                Message = String.Format("Player {0} has joined game {1}", Context.User.Identity.GetUserName(), g)
            });
            logRepo.Save();

            return game;
        }

        public void PlayTurn(Turn t)
        {
            Game g = _gameRepository.GetGameByID(t.GameId);

            PlayerGameData p = g.PlayerData.FirstOrDefault(x => x.PlayerID == t.Current.ID);
            var p_help = g.PlayerData.FirstOrDefault(x => x.TurnOrder == (p.TurnOrder + 1));

            t.NextPlayer = p_help == null ? g.PlayerData.FirstOrDefault(x => x.TurnOrder == 0).Player : p_help.Player;

            Clients.OthersInGroup(t.GameId.ToString()).PlayedTurn(t);

            _gameRepository.AddTurn(t,g);
            _gameRepository.Save();

            logRepo.InsertLog(new Log()
            {
                Category = LogType.Game,
                CategoryAttribute = t.GameId.ToString(),
                Date = DateTime.Now,
                Message = String.Format("Player {0} has played a turn in Game {1}", Context.User.Identity.GetUserName(), t.GameId)
            });
            logRepo.Save();
        }

        public void UpdateScores(Game g)
        {
            var game = _gameRepository.GetGameByID(g.GameID);

            foreach (var data in game.PlayerData)
            {
                data.Score = g.PlayerData.FirstOrDefault(x => x.PlayerID == data.PlayerID).Score;
            }
            _gameRepository.UpdateGame(game);
            _gameRepository.Save();
        }
    }
}