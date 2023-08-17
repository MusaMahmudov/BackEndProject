using Microsoft.AspNetCore.Mvc;

namespace EduProject.ViewComponents
{
    public class SubscribeViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
