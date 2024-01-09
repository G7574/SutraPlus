using Microsoft.AspNetCore.Mvc.Filters;

namespace SutraPlus.Filters
{
    public class RuntimeManagerFilter : IActionFilter
    {

        internal string sessionID = Guid.NewGuid().ToString();

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //if (CoreRuntime.Context != null)
            //{
            //    CoreRuntime.RemoveContext();
            //}
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //CoreRuntime.StartWebContext(sessionID);

        }
    }
}
