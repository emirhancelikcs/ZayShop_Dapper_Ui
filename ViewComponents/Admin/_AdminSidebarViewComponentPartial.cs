﻿using Microsoft.AspNetCore.Mvc;

namespace ZayShop_Dapper_Ui.ViewComponents.Admin
{
	public class _AdminSidebarViewComponentPartial : ViewComponent
	{
		public IViewComponentResult Invoke() => View();
	}
}
