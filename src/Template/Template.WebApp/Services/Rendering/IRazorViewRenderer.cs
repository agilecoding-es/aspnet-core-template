using Microsoft.AspNetCore.Mvc;

namespace Template.WebApp.Services.Rendering
{
    public interface IRazorViewRenderer
    {
        Task<string> RenderViewAsync<TModel>(string viewName, TModel model, ControllerContext controllerContext);
        Task<string> RenderPartialViewAsync<TModel>(string viewName, TModel model, ControllerContext controllerContext);
    }
}
