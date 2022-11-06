using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MovieCatalogBackend.Helpers;

public class RequireValidModelAttribute : ActionFilterAttribute
{
    public int MaxErrors { get; set; } = 0;
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.ErrorCount <= MaxErrors) return;
        if (context.Controller is not ControllerBase controller)
        {
            context.Result = new BadRequestObjectResult(context.ModelState);
            return;
        }

        context.Result = controller.ValidationProblem(context.ModelState);
    }
}