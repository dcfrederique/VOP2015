using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carcassonne_Web.DAL;
using Carcassonne_Web.Models;
using PagedList;

namespace Carcassonne_Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class LogController : Controller
    {
        private readonly ILogRepository _logRepo;

        public LogController()
        {
            _logRepo = new LogRepository(new CarcassonneContext());
        }

        // GET: Log
        public ActionResult Index(int? page)
        {
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            ViewBag.viewRelated = false;
            return View(_logRepo.GetLogs().ToPagedList(pageNumber, pageSize));
        }

        // GET: Log/Details/5
        public ActionResult RelatedLogs(int? page,  string id, LogType logType)
        {
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            ViewBag.viewRelated = true;
            return View("Index",_logRepo.GetRelatedLogs(id, logType).ToPagedList(pageNumber, pageSize));
        }
    }
}
