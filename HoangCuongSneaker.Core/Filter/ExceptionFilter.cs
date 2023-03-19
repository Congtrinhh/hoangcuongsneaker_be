using HoangCuongSneaker.Core.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Filter
{
    public class ExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                var objectRespose = CreateObjectResponse(filterContext.Exception);
                filterContext.Result = new ObjectResult(objectRespose) { StatusCode=200 };
                filterContext.ExceptionHandled = true;
            }
            throw new NotImplementedException();
        }

        private ApiResponse CreateObjectResponse(Exception exception)
        {
            var response = new ApiResponse();
            response.OnFailure(exception);
            return response;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
}
