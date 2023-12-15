using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using ZayShop_Dapper_Ui.Dtos.ProductDtos;

namespace ZayShop_Dapper_Ui.Controllers
{
	public class ShopController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public ShopController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7167/api/Products/ProductListWithCategory");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonData);

				return View(values);
			}

			return View();
		}

		public async Task<IActionResult> Details(int id)
		{
			if (id == null)
			{
				RedirectToAction("Index");
			}

			string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync($"https://localhost:7167/api/Products/{id}");

			

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var value = JsonConvert.DeserializeObject<ResultProductDto>(jsonData);

				return View(value);
			}

			return RedirectToAction("404", "Index");
		}
	}
}
