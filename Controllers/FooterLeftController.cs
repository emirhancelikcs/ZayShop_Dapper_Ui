using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ZayShop_Dapper_Ui.Dtos.FooterLeftDtos;

namespace ZayShop_Dapper_Ui.Controllers
{
	[Authorize(Roles = "Admin")]
	public class FooterLeftController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public FooterLeftController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7167/api/FooterLeft");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultFooterLeftDto>>(jsonData);
				//Listeleme için DeserializeObject

				return View(values);
			}

			return View();
		}

		public IActionResult CreateFooterLeft() => View();

		[HttpPost]
		public async Task<IActionResult> CreateFooterLeft(CreateFooterLeftDto createFooterLeftDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createFooterLeftDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7167/api/FooterLeft", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "FooterLeft created!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "FooterLeft coould not be created!";
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateFooterLeft(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync($"https://localhost:7167/api/FooterLeft/{id}");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var value = JsonConvert.DeserializeObject<UpdateFooterLeftDto>(jsonData);
				return View(value);
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateFooterLeft(UpdateFooterLeftDto updateFooterLeftDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateFooterLeftDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7167/api/FooterLeft", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "FooterLeft updated!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "FooterLeft could not be updated!";
			return View();
		}

		public async Task<IActionResult> DeleteFooterLeft(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.DeleteAsync($"https://localhost:7167/api/FooterLeft?id={id}");

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "FooterLeft deleted!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "FooterLeft could not be deleted!";
			return View();
		}
	}
}
