using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Carcassonne_Web.Models.GameObj
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        [NotMapped]
        public ICollection<Player> Participants { get; set; }
        [JsonIgnore]
        public ICollection<ApplicationUser> Players { get; set; }
        [JsonIgnore]
        public ApplicationUser RoomLeader { get; set; }
        [NotMapped]
        public Player RoomPrincipal { get; set; }
        public String RoomName { get; set; }
        public bool isPrivate { get; set; }
        public bool hasStarted { get; set; }
        public Guid StartedGame { get; set; }
        public DateTime CreationDate {get; set;}

        public void Map()
        {
            if (Players != null)
            {
                this.Participants = this.Players.Select(x => new Player()
                {
                    ID = x.Id,
                    UserName = x.UserName,
                    Avatar = x.Avatar
                }).ToList();
            }
            if (RoomLeader != null)
            {
                RoomPrincipal = new Player()
                {
                    ID = RoomLeader.Id,
                    UserName = RoomLeader.UserName,
                    Avatar = RoomLeader.Avatar
                };
            }
        }
    }
}