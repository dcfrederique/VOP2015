using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carcassonne_Web.Models.GameObj;
using Carcassonne_Web.Models;
using System.Data.Entity;

namespace Carcassonne_Web.DAL
{
    public class LogRepository : ILogRepository, IDisposable
    {
        private readonly CarcassonneContext _context;

        public LogRepository(CarcassonneContext context)
        {
            this._context = context;
        }

        public IEnumerable<Log> GetLogs()
        {
            return _context.Logs.ToList();
        }

        public Log GetLogById(int logId)
        {
            return _context.Logs.FirstOrDefault(x => x.ID == logId);
        }

        public void InsertLog(Log log)
        {
            _context.Logs.Add(log);
        }

        public void DeleteLog(int logId)
        {
            Log Log = _context.Logs.Find(logId);
            _context.Logs.Remove(Log);
        }

        public void UpdateLog(Log log)
        {
            _context.Entry(log).State = EntityState.Modified;
        }

        public IEnumerable<Log> GetRelatedLogs(string id, LogType logType)
        {
            return _context.Logs.Where(x => x.Category == logType && x.CategoryAttribute == id).ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
