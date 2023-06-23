using Microsoft.AspNetCore.Mvc;

namespace Template.MvcWebApp.Services.Rendering
{
    public interface IRazorViewRenderer
    {
        Task<string> RenderViewAsync<TModel>(string viewName, TModel model, ControllerContext controllerContext);
        Task<string> RenderPartialViewAsync<TModel>(string viewName, TModel model, ControllerContext controllerContext);
    }
}
