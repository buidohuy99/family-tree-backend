using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers
{
    public class HttpResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public IEnumerable<object> Errors { get; set; }

        public HttpResponse(T data, string message){
            Data = data;
            Message = message;
        }

        public HttpResponse(T data, string message, IEnumerable<object> errors)
        {
            Data = data;
            Message = message;
            Errors = errors;
        }
    }
}
