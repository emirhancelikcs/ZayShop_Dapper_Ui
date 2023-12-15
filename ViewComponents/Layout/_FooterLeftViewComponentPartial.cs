using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZayShop_Dapper_Ui.Dtos.FooterLeftDtos;

namespace ZayShop_Dapper_Ui.ViewComponents.Layout
{
	public class _FooterLeftViewComponentPartial : ViewComponent
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public _FooterLeftViewComponentPartial(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7167/api/FooterLeft");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultFooterLeftDto>>(jsonData);

				return View(values);
			}

			return View();
		}
	}
}
