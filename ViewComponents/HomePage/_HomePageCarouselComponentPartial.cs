using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZayShop_Dapper_Ui.Dtos.IndexCarouselDtos;

namespace ZayShop_Dapper_Ui.ViewComponents.HomePage
{
	public class _HomePageCarouselComponentPartial : ViewComponent
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public _HomePageCarouselComponentPartial(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7167/api/IndexCarousel");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultIndexCarouselDto>>(jsonData);

				return View(values);
			}

			return View();
		}
	}
}
