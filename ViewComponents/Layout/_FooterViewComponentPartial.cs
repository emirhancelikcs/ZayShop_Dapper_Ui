using Microsoft.AspNetCore.Mvc;

namespace ZayShop_Dapper_Ui.ViewComponents.Layout
{
	public class _FooterViewComponentPartial : ViewComponent
	{
		public IViewComponentResult Invoke() => View();
	}
}
