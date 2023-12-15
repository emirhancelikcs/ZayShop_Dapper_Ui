using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZayShop_Dapper_Ui.Controllers
{
	public class ProfileController : Controller
	{
		[Authorize(Roles = "Admin")]
		public IActionResult Index()
		{
			return View();
		}
	}
}
