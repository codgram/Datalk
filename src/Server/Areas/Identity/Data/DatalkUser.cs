using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Datalk.Server.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the DatalkUser class
    public class DatalkUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get {
                return FirstName + " " + LastName;
            }
        }
    }
}
