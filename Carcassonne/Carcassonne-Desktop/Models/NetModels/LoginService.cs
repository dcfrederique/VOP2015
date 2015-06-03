using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Carcassonne_Desktop.Models.NetModels
{
    class LoginService : NetService
    {
        public TokenResponseModel Token { get; private set; }

        public bool Login(string Username, string password)
        {
            try
            {
                SendLogin(Username, password).Wait();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task SendLogin(string un, string pw)
        {
            try
            {
                string json = await PostFormContent("Token", new FormUrlEncodedContent(new List<KeyValuePair<String, String>>() 
                    {
                        new KeyValuePair<String,String>("username",un),
                        new KeyValuePair<String,String>("password",pw),
                        new KeyValuePair<String,String>("grant_type","password")
                    })).ConfigureAwait(false);

                Token = JsonConvert.DeserializeObject<TokenResponseModel>(json);
            }
            catch (Exception ex)
            {
                throw new SecurityException("Bad credentials", ex);
            }
        }


    }
}
