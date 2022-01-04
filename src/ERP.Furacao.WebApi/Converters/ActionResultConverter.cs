using ERP.Furacao.WebApi.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Furacao.WebApi.Converters
{
    public interface IActionResultConverter
    {
        /// <summary>
        /// HTTP 201
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IActionResult CreatedResult(object data);

        /// <summary>
        /// HTTP 200
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IActionResult OkObjectResult(object data);

        /// <summary>
        /// HTTP 202
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IActionResult AcceptedResult(object data);

        /// <summary>
        /// HTTP 422
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        IActionResult BusinessErrorResult(string[] errors);

        /// <summary>
        /// HTTP 422
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        IActionResult BusinessErrorResult(object data, string[] errors);
        
        /// <summary>
        /// HTTP 200
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalItens"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IActionResult PaginatedObjectResult(object data, int totalItems, int page, int pageSize);

    }

    public class ActionResultConverter : IActionResultConverter
    {
        private readonly string path;
        private readonly IHttpContextAccessor accessor;
        public ActionResultConverter(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
            this.path = accessor.HttpContext.Request.Path.Value ?? string.Empty;
        }

        public IActionResult AcceptedResult(object data)
        {
            var result = new JsonData() { Data = data, Errors = new string[] { } };
            return new AcceptedResult(path, result);
        }

        public IActionResult BusinessErrorResult(string[] errors)
        {
            var result = new JsonData() { Data = null, Errors = errors };
            return new UnprocessableEntityObjectResult(result);
        }

        public IActionResult BusinessErrorResult(object data, string[] errors)
        {
            var result = new JsonData() { Data = data, Errors = errors };
            return new UnprocessableEntityObjectResult(result);
        }

        public IActionResult CreatedResult(object data)
        {
            var result = new JsonData() { Data = data, Errors = new string[] { } };
            return new CreatedResult(path, result);
        }

        public IActionResult OkObjectResult(object data)
        {
            var result = new JsonData() { Data = data, Errors = new string[] { } };
            return new OkObjectResult(result);
        }

        public IActionResult PaginatedObjectResult(object data, int totalItems, int page, int pageSize)
        {
            var result = new JsonData() { Data = data, Errors = new string[] { } };
            accessor.HttpContext.Response.Headers.Add("TotalItems", totalItems.ToString());
            accessor.HttpContext.Response.Headers.Add("Page", page.ToString());
            accessor.HttpContext.Response.Headers.Add("PageSize", pageSize.ToString());

            return new OkObjectResult(result);
        }
    }
}
