using Microsoft.AspNetCore.Mvc;

namespace IS6IntegrationAspNetCoreMVC.Areas.Customer.Controllers
{
    public class PersonViewModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this._logger = logger;
        }

        public IActionResult Index()
        {
            var person = new PersonViewModel()
            {
                FirstName = "Ana",
                LastName = "Dimitrova",
                Email = "ana.dimitrova@gmail.com",
            };

            return View(person);
        }
    }
}
