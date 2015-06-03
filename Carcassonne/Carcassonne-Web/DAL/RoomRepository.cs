using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carcassonne_Web.Models.GameObj;
using Carcassonne_Web.Models;
using System.Data.Entity;

namespace Carcassonne_Web.DAL
{
    public class RoomRepository : IRoomRepository, IDisposable
    {
        private CarcassonneContext context;

        public RoomRepository(CarcassonneContext context)
        {
            this.context = context;
        }

        public IEnumerable<Room> GetRooms()
        {
            var rooms = context.Rooms.Include(x => x.Players);

            foreach (var item in rooms)
            {
                item.Map();
            }

            return rooms.ToList();
        }

        public Room GetRoomByID(int id)
        {
            var room = context.Rooms.Where(x => x.RoomId == id).Include(x => x.Players).FirstOrDefault();
            room.Map();
            return room;
        }

        public void InsertRoom(Room room)
        {
            context.Rooms.Add(room);
        }

        public void DeleteRoom(int roomID)
        {
            Room room = context.Rooms.Find(roomID);
            context.Rooms.Remove(room);
        }

        public void UpdateRoom(Room room)
        {
            context.Entry(room).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
