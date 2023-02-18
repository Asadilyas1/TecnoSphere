using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TecnoSphere.Core.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static string IsActive(this IHtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            if(currentController != null)
            {
                return controller.ToLower().Split(',').Contains(currentController.ToLower()) && action.ToLower().Split(',').Contains(currentAction.ToLower()) ?
                cssClass : String.Empty;
            }
            return string.Empty;
        }
    }
}
