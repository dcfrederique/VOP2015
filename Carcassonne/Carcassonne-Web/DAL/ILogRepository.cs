using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carcassonne_Web.Models;

namespace Carcassonne_Web.DAL
{
    public interface ILogRepository : IDisposable
    {
        IEnumerable<Log> GetLogs();
        Log GetLogById(int logId);
        void InsertLog(Log log);
        void DeleteLog(int logId);
        void UpdateLog(Log log);
        IEnumerable<Log> GetRelatedLogs(string id, LogType logType);
        void Save();

    }
}