using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Application.Common
{
    public class APIErrors
    {
        public string Description { get; set; }

        public APIErrors(string description)
        {
            Description = description;
        }
    }
}
