using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Application.Common
{
    public class APIResponse
    {
        public HttpStatusCode statusCode { get; set; }

        public bool IsSuccess { get; set; } = false;

        public object Result { get; set; }

        public string DisplayMessage { get; set; } = "";

        public List<APIErrors> Errors { get; set; } = new();

        public List<APIWarnings> Warnings { get; set; } = new();

        public void AddError(string errorMessage)
        {
            APIErrors error = new APIErrors(description: errorMessage);
            Errors.Add(error);
        }
        public void AddWarning(string warningMessage)
        {
            APIWarnings warning = new APIWarnings(description: warningMessage);
            Warnings.Add(warning);
        }
    }
}
