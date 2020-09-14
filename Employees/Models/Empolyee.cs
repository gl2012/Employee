using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Models
{
    public class Empolyee
    {
        public fullname name { get; set; }
        public string jobTitle { get; set; }
        public string photoURL { get; set; }
    }

    public class fullname { 
        public string first { get; set; }
        public string last { get; set; }
    
    }

   
}