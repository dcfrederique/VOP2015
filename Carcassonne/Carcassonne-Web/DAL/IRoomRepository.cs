using Carcassonne_Web.Models.GameObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carcassonne_Web.DAL
{
    public interface IRoomRepository : IDisposable
    {
        IEnumerable<Room> GetRooms();
        Room GetRoomByID(int roomId);
        void InsertRoom(Room room);
        void DeleteRoom(int roomID);
        void UpdateRoom(Room room);
        void Save();

    }
}