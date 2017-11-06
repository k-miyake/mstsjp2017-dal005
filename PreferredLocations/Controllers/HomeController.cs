using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreferredLocations.Models;
using Microsoft.Extensions.Options;

namespace PreferredLocations.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppOption _options;

        public HomeController(IOptions<AppOption> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public async Task<ActionResult> Index()
        {
            var repository = new DocumentDBRepository<Item>(_options);
            var items = await repository.GetItemsAsync(d => !d.Completed);

            var vm = new HomeViewModel
            {
                Items = items,
                WriteRegion = repository.GetWriteEndpoint(),
                ReadRegion = repository.GetReadEndpoint()
            };
            return View(vm);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
