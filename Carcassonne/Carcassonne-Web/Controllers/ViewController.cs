using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Carcassonne_Web.DAL;

namespace Carcassonne_Web.Controllers
{
    [Authorize]
    public class ViewController : Controller
    {
        // GET: View
        public ActionResult Rooms()
        {
            IRoomRepository roomRepo = new RoomRepository(new CarcassonneContext());

            ViewBag.UserID = User.Identity.GetUserId();
            ViewBag.UserName = User.Identity.GetUserName();
            bool joined = false;
            int roomid = 0;

            foreach (var item in roomRepo.GetRooms())
            {
                foreach (var player in item.Players)
                {
                    joined = player.Id == ViewBag.UserID;
                    roomid = item.RoomId;

                    if (joined) { break; }
                }
                if (joined) { break; }
            }
            ViewBag.RoomId = roomid;
            ViewBag.Joined = joined;

            return View();
        }

        public ActionResult Scores()
        {
            return View();
        }
    }
}