using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcassonne_Desktop.Models.NetModels
{
    class UserState
    {

        private static UserState _instance;

        private User user;

        private UserState() { }

        public static UserState _getInstance()
        {
            if (_instance == null)
            {
                _instance = new UserState();
            }
            return _instance;
        }

        public User GetUser(TokenResponseModel token = null)
        {

            if (user == null || user.Token == null)
            {
                LoadUser(token);
            }
            else
            {
                return user;
            }

            return user;
        }

        private void LoadUser(TokenResponseModel token = null, string id = null)
        {
            user = new User() { Token = token, Id = id};

        }

    }
}
