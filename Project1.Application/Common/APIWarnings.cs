using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Application.Common
{
    public class APIWarnings
    {
        public string Description { get; set; }

        public APIWarnings(string description)
        {
            Description = description;
        }
    }
}
