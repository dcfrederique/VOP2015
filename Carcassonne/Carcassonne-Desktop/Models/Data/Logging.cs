using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcassonne_Desktop.Models.Data
{
    public class Logging
    {
        private StreamWriter w;
        private string LogFolder;

        public Logging(string file)
        {
            Init(file);
        }

        public Logging()
        {
            Init();
        }

        private void FolderSetup()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string specificFolder = Path.Combine(folder, "Carcassonne");
            if (!Directory.Exists(specificFolder))
                Directory.CreateDirectory(specificFolder);
            specificFolder = Path.Combine(specificFolder, "Log");
            if (!Directory.Exists(specificFolder))
                Directory.CreateDirectory(specificFolder);
            LogFolder = specificFolder;
        }


        public void Init()
        {
            FolderSetup();
            string filename = "carcassonne_log_" + DateTime.Now.ToLongDateString().Replace(" ", "_");
            string path = Path.Combine(LogFolder, filename);
            int i = 1;
            string log = path;
            while (File.Exists(log + ".txt"))
            {
                log = path + "_"+ i++;
            }
            log += ".txt";
            w = File.AppendText(log); 
        }

        public void Init(string file)
        {
            w = File.AppendText(file);
        }

        public void Log(string message)
        {
            if (w != null)
            {
                w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                w.WriteLine("  :");
                w.WriteLine("  :{0}", message);
                w.WriteLine("-------------------------------");
            }
        }
        public void LogForPlayer(string message, Player player)
        {
            if (w != null)
            {
                w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                w.WriteLine("  :{0}", player.Username);
                w.WriteLine("  :{0}", message);
                w.WriteLine("-------------------------------");
            }
        }
    }
}
