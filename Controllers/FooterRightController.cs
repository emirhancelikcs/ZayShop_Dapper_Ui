using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ZayShop_Dapper_Ui.Dtos.FooterRightDtos;

namespace ZayShop_Dapper_Ui.Controllers
{
	[Authorize(Roles = "Admin")]
	public class FooterRightController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public FooterRightController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7167/api/FooterRight");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultFooterRightDto>>(jsonData);
				//Listeleme için DeserializeObject

				return View(values);
			}

			return View();
		}

		public async Task<IActionResult> CreateFooterRight() => View();

		[HttpPost]
		public async Task<IActionResult> CreateFooterRight(CreateFooterRightDto createFooterRight)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createFooterRight);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7167/api/FooterRight", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "FooterRight created!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "FooterRight could not be created!";
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateFooterRight(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync($"https://localhost:7167/api/FooterRight/{id}");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var value = JsonConvert.DeserializeObject<UpdateFooterRightDto>(jsonData);
				return View(value);
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateFooterRight(UpdateFooterRightDto updateFooterRightDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateFooterRightDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7167/api/FooterRight", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "FooterRight updated!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "FooterRight could not be updated!";
			return View();
		}

		public async Task<IActionResult> DeleteFooterRight(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.DeleteAsync($"https://localhost:7167/api/FooterRight?id={id}");

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "FooterRight deleted!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "FooterRight could not be deleted!";
			return View();
		}
	}
}
