using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [Route("")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [Route(""), HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public RedirectResult RedirectToSwaggerUi()
        {
            return Redirect("/swagger/");
        }
    }
}
