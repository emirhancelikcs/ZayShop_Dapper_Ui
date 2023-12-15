using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ZayShop_Dapper_Ui.Dtos.CategoryOfTheMonthDto;

namespace ZayShop_Dapper_Ui.Controllers
{
	[Authorize(Roles = "Admin")]
	public class CategoryOfTheMonthController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public CategoryOfTheMonthController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7167/api/CategoryOfTheMonth");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultCategoryOfTheMonthDto>>(jsonData);
				//Listeleme için DeserializeObject
				return View(values);
			}

			return View();
		}

		[HttpGet]
		public IActionResult CreateCategoryOfTheMonth() => View();

		[HttpPost]
		public async Task<IActionResult> CreateCategoryOfTheMonth(CreateCategoryOfTheMonthDto createCategoryOfTheMonthDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createCategoryOfTheMonthDto);//Güncelleme/Ekleme için SerializeObject
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7167/api/CategoryOfTheMonth", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "Category Of The Month created!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "Category Of The Month could not be created!";
			return View();
		}

		public async Task<IActionResult> DeleteCategoryOfTheMonth(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.DeleteAsync($"https://localhost:7167/api/CategoryOfTheMonth?id={id}");

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "Category Of The Month deleted!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "Category Of The Month could not be deleted!";
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateCategoryOfTheMonth(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync($"https://localhost:7167/api/CategoryOfTheMonth/{id}");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<UpdateCategoryOfTheMonthDto>(jsonData);
				return View(values);
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateCategoryOfTheMonth(UpdateCategoryOfTheMonthDto updateCategoryOfTheMonthDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateCategoryOfTheMonthDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7167/api/CategoryOfTheMonth", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "Category Of The Month updated!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "Category Of The Month could not be updated!";
			return View();
		}
	}
}
