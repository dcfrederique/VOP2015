using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carcassonne_Web.Models.GameObj
{
    public class Player
    {
        public String ID { get; set; }
        public String UserName { get; set; }

        //Voor Facebook : http://graph.facebook.com/FACEBOOKID/picture?type=large
        public String Avatar { get; set; }

    }
}
