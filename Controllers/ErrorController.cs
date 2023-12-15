using Microsoft.AspNetCore.Mvc;

namespace ZayShop_Dapper_Ui.Controllers
{
	public class ErrorController : Controller
	{
		public IActionResult Index() => View();

		[ActionName("404")]
		public IActionResult PageNotFound() => View();
	}
}
