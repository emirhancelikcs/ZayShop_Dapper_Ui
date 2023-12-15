using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ZayShop_Dapper_Ui.Dtos.InfosDtos;

namespace ZayShop_Dapper_Ui.Controllers
{
	[Authorize(Roles = "Admin")]
	public class InfosController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public InfosController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7167/api/Infos");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var value = JsonConvert.DeserializeObject<ResultInfosDto>(jsonData);
				//Listeleme için DeserializeObject

				return View(value);
			}

			return View();
		}

		public async Task<IActionResult> UpdateInfos()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync($"https://localhost:7167/api/Infos");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var value = JsonConvert.DeserializeObject<UpdateInfosDto>(jsonData);
				return View(value);
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateInfos(UpdateInfosDto updateInfosDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateInfosDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7167/api/Infos", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "Infos updated!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "Infos could not be updated!";
			return View();
		}
	}
}
