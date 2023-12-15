using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZayShop_Dapper_Ui.Dtos.CategoryOfTheMonthDto;

namespace ZayShop_Dapper_Ui.ViewComponents.HomePage
{
	public class _HomePageCategoriesOfTheMonthComponentPartial : ViewComponent
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public _HomePageCategoriesOfTheMonthComponentPartial(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7167/api/CategoryOfTheMonth");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultCategoryOfTheMonthDto>>(jsonData);

				ViewBag.Header = values.Select(x => x.CategoryOfTheMonthHeader).FirstOrDefault();
				ViewBag.Title = values.Select(x => x.CategoryOfTheMonthTitle).FirstOrDefault();
			}

			return View();
		}
	}
}
