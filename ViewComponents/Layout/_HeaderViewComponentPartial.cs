using Microsoft.AspNetCore.Mvc;

namespace ZayShop_Dapper_Ui.ViewComponents.Layout
{
	public class _HeaderViewComponentPartial : ViewComponent
	{
		public IViewComponentResult Invoke() => View();
	}
}
