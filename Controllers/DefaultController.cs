using Microsoft.AspNetCore.Mvc;

namespace ZayShop_Dapper_Ui.Controllers
{
	public class DefaultController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
