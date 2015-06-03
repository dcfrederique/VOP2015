using Carcassonne_Web.DAL;
using Carcassonne_Web.Models.GameObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web.Http;
using Carcassonne_Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR.Hubs;

namespace Carcassonne_Web.Controllers
{
    [RoutePrefix("api/Game")]
    [Authorize]
    public class GameController : ApiController
    {
        private IGameRepository gameRepo;
        private IRoomRepository roomRepo;
        private ILogRepository logRepo;

        public GameController()
        {
            CarcassonneContext context = new CarcassonneContext();
            gameRepo = new GameRepository(context);
            roomRepo = new RoomRepository(context);
            logRepo = new LogRepository(context);
        }

        // POST: api/Game
        public Guid Post([FromBody]string value)
        {
            
            var room = roomRepo.GetRoomByID(Convert.ToInt32(value));

            if (room.RoomLeader.Id == User.Identity.GetUserId())
            {

                Game game = new Game()
                {
                    GameID = Guid.NewGuid(),
                    Started = DateTime.Now,
                    PlayerData = new HashSet<PlayerGameData>()
                };


                bool[] turnOrder = new bool[room.Players.Count];

                foreach (var item in room.Players)
                {
                    bool found = false;
                    int i = 0;

                    while (!found)
                    {
                        i = new Random().Next(0, room.Players.Count);
                        if (!turnOrder[i])
                        {
                            turnOrder[i] = true;
                            found = true;
                        }
                    }

                    game.PlayerData.Add(new PlayerGameData()
                    {
                        Player = item,
                        Pawns = 8,
                        Win = false,
                        TurnOrder = i
                    });
                }

                gameRepo.InsertGame(game);
                gameRepo.Save();

                room.StartedGame = game.GameID;
                room.hasStarted = true;

                roomRepo.UpdateRoom(room);
                roomRepo.Save();

                logRepo.InsertLog(new Log()
                {
                    Category = LogType.Room,
                    CategoryAttribute = room.RoomId.ToString(),
                    Date = DateTime.Now,
                    Message = String.Format("Player {0} has started a game for Room {1}", room.RoomLeader.UserName, room.RoomId)
                });
                logRepo.Save();

                return game.GameID;
            }
            else
            {
                throw new NotAuthorizedException();
            }
        }
    }
}
