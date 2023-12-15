using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ZayShop_Dapper_Ui.Dtos.FooterCenterDtos;

namespace ZayShop_Dapper_Ui.Controllers
{
	[Authorize(Roles = "Admin")]
	public class FooterCenterController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public FooterCenterController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7167/api/FooterCenter");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultFooterCenterDto>>(jsonData);
				//Listeleme için DeserializeObject

				return View(values);
			}

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateFooterCenter(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync($"https://localhost:7167/api/FooterCenter/{id}");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var value = JsonConvert.DeserializeObject<UpdateFooterCenterDto>(jsonData);
				return View(value);
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateFooterCenter(UpdateFooterCenterDto updateFooterCenterDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateFooterCenterDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7167/api/FooterCenter", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "FooterCenter updated!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "FooterCenter could not be updated!";
			return View();
		}

		public async Task<IActionResult> CreateFooterCenter() => View();

		[HttpPost]
		public async Task<IActionResult> CreateFooterCenter(CreateFooterCenterDto createFooterCenterDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createFooterCenterDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7167/api/FooterCenter", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "FooterCenter created!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "FooterCenter coould not be created!";
			return View();
		}

		public async Task<IActionResult> DeleteFooterCenter(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.DeleteAsync($"https://localhost:7167/api/FooterCenter?id={id}");

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "FooterCenter deleted!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "FooterCenter could not be deleted!";
			return View();
		}
	}
}
