using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Template.WebApp.Services.Rendering
{
    public class RazorViewRenderer : IRazorViewRenderer
    {
        private IRazorViewEngine _viewEngine;
        private ITempDataProvider _tempDataProvider;
        private IServiceProvider _serviceProvider;
        private HttpContext _context;

        public RazorViewRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IHttpContextAccessor accessor)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _context = accessor.HttpContext;
        }

        public async Task<string> RenderPartialViewAsync<TModel>(string viewName, TModel model, ControllerContext controllerContext)
        {
            return await RenderToStringAsync(viewName, model, controllerContext, false);
        }

        public async Task<string> RenderViewAsync<TModel>(string viewName, TModel model, ControllerContext controllerContext)
        {
            return await RenderToStringAsync(viewName, model, controllerContext, true);
        }

        private async Task<string> RenderToStringAsync<TModel>(string viewName, TModel model, ControllerContext controllerContext, bool isView)
        {
            var actionContext = new ActionContext(_context, new RouteData(), new ActionDescriptor());
            var view = FindView(actionContext, viewName, controllerContext, isView);

            var viewData = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary());
            viewData.Model = model;

            var tempData = new TempDataDictionary(_context, _tempDataProvider);

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(actionContext, view, viewData, tempData, output, new HtmlHelperOptions());
                viewContext.RouteData = _context.GetRouteData();   //set route data here

                await view.RenderAsync(viewContext);

                return output.ToString();
            }
        }

        private IView FindView(ActionContext actionContext, string viewName, ControllerContext controllerContext, bool isView)
        {
            var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: isView);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: isView);
            if (findViewResult.Success)
            {
                return findViewResult.View;
            }

            var findViewResultController = _viewEngine.FindView(controllerContext, viewName, isMainPage: isView);
            if (findViewResultController.Success)
            {
                return findViewResultController.View;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations));

            throw new InvalidOperationException(errorMessage);
        }
    }
}
