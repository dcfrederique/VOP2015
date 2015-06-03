using Carcassonne_Web.DAL;
using Carcassonne_Web.Models.GameObj;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Carcassonne_Web.Models;

namespace Carcassonne_Web.Controllers
{
    [RoutePrefix("api/Rooms")]
    [Authorize]
    public class RoomsController : ApiController
    {
        private IRoomRepository roomRepo;
        private IPlayerRepository playerRepo;
        private ILogRepository logRepo;

        public RoomsController()
        {
            var context = new CarcassonneContext();
            roomRepo = new RoomRepository(context);
            playerRepo = new PlayerRepository(context);
            logRepo = new LogRepository(context);
        }

        // GET: api/Rooms
        public IEnumerable<Room> GetRooms()
        {
            return roomRepo.GetRooms();
        }

        // GET: api/Rooms/5
        public Room Get(int id)
        {
            return roomRepo.GetRoomByID(id);
        }

        // POST: api/Rooms/5/users
        [Route("{id:int}/users")]
        public void Post(int id, [FromBody]string value)
        {
            var room = roomRepo.GetRoomByID(id);
            var player = playerRepo.GetPlayerByID(value);
            room.Players.Add(player);
            roomRepo.UpdateRoom(room);
            roomRepo.Save();
            logRepo.InsertLog(new Log()
            {
                Category = LogType.Room,
                CategoryAttribute = id.ToString(),
                Date = DateTime.Now,
                Message = String.Format("Player {0} has joined Room {1}", player.UserName, room.RoomName)
            });
            logRepo.Save();
        }

        // PUT: api/Rooms
        public void Put(Room value)
        {
            value.CreationDate = DateTime.Now;
            value.RoomLeader = playerRepo.GetPlayerByID(User.Identity.GetUserId());
            roomRepo.InsertRoom(value);
            roomRepo.Save();
            logRepo.InsertLog(new Log()
            {
                Category = LogType.Room,
                CategoryAttribute = value.RoomId.ToString(),
                Date = DateTime.Now,
                Message = String.Format("Player {0} has started Room {1}", value.RoomLeader.UserName, value.RoomName)
            });
            logRepo.Save();
        }

        // DELETE: api/Rooms/5/users
        [Route("{id:int}/users")]
        public void Delete(int id, [FromBody]string value)
        {
            var room = roomRepo.GetRoomByID(id);
            var player = playerRepo.GetPlayerByID(value);
            room.Players.Remove(player);
            roomRepo.UpdateRoom(room);
            roomRepo.Save();
            logRepo.InsertLog(new Log()
            {
                Category = LogType.Room,
                CategoryAttribute = room.RoomId.ToString(),
                Date = DateTime.Now,
                Message = String.Format("Player {0} has left Room {1}", player.UserName, room.RoomName)
            });
            logRepo.Save();
        }
    }
}
