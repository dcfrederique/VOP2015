using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Carcassonne_Desktop.Views;
using Carcassonne_Desktop.ViewModels;
using Carcassonne_Desktop.Models.Games;
using System.Diagnostics;
using System.Web;
using Carcassonne_Desktop.Models.NetModels;

namespace Carcassonne_Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            MainMenuView v = new MainMenuView();
            v.Show();

            if (e.Args.Count() > 0)
            {
                MessageBox.Show(e.Args[0]);
            }
            

            if(e.Args.Length==1&&e.Args[0].ToLower().StartsWith("carcassonne:?"))
            {
                string qs = e.Args[0].Replace("carcassonne:","");
                var parsed = HttpUtility.ParseQueryString(qs);
                v.Hide();
                Debug.WriteLine("Started in Online game mode with id: " + parsed["gameid"] + " with player: " + parsed["playerid"]);
                UserState._getInstance().GetUser(new TokenResponseModel(){ AccessToken = parsed["token"]}).Id = parsed["playerid"];
                OnlineBoardViewModel bm = new OnlineBoardViewModel(new WebGame(parsed["gameid"]));
                OnlineBoardView b = new OnlineBoardView { DataContext = bm };
                b.Closing += bm.OnWindowsClosing;
                bm.window = b;
                b.Owner = v;
                b.Show();
            }
        }
    }
}
