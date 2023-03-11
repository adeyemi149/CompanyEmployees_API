using System;

public class ValidationFilterAttribute : IActionFilter
{
	public ValidationFilterAttribute()
	{
	}
	public void OnActionExecuting(ActionExecutingContext context) 
	{
		var action = context.RouteData.Values["action"];
		var controller = context.RouteData.Values["controller"];

		var param = context.ActionArgumejts.SingleOrDefault(x => x.Value.ToString().Contains("Dto")).Value;

		if(param== null)
		{
			context.Result = new BadRequestObject($"Object is null. Controller: {controller}, action: {action}");
			return;
		}

		if (!context.ModelState.IsValid)
			context.Result = new UnprocessableEntityObjectResult(context.ModelState);
	}

	public void OnActionExecuted(ActionExecutingContext context) { }

}
