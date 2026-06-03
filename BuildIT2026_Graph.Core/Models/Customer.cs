using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildIT2026_Graph.Core.Models
{

    public class Customer
    {      
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }      
        public string Email { get; set; }
        public string PhoneNumber { get; set; }        

        // Metadata / audit fields
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Status
        public bool IsActive { get; set; }
 
    }

}
