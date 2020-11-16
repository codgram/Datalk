using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Datalk.Client.Models
{
    // Add profile data for application users by adding properties to the DatalkUser class
    public class DatalkUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get {
                return FirstName + " " + LastName;
            }
        }
    }
}
