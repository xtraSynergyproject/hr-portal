using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
public    class RemoteSignInSignOutViewModel
    {

       public string Id { get; set; }
        public string UserId { get; set; }
        public string PersonId { get; set; }
        public string signinTime { get; set; }

        public  string signOutTime { get; set; }

        public  string LocationName { get; set; }
        public  string Date { get; set; }
        public string ServiceNo { get; set; }
        public string ServiceOwner { get; set; }
        public string Status { get; set; }

        public string ServiceId { get; set; }


    }
}
