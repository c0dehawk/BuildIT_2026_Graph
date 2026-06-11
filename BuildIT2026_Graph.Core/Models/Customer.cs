using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BuildIT2026_Graph.Core.Models
{

    public class Customer
    {      
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string DisplayName => $"{FirstName} {LastName}";

        public CreateNodeRequest CreateNodeRequest() { 
            return new CreateNodeRequest
            {
                Label = "Customer",
                Properties = new Dictionary<string, object?>
                {
                    { "customerId", CustomerId },
                    { "firstName", FirstName },
                    { "lastName", LastName },
                    { "displayName", DisplayName }
                }
            };
        }   
    }

}
