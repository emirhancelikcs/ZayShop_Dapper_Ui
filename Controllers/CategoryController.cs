using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ZayShop_Dapper_Ui.Dtos.CategoryDtos;

namespace ZayShop_Dapper_Ui.Controllers
{
	[Authorize(Roles = "Admin")]
	public class CategoryController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public CategoryController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7167/api/Categories");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);
				//Listeleme için DeserializeObject

				return View(values);
			}

			return View();
		}

		[HttpGet]
		public IActionResult CreateCategory() => View();

		[HttpPost]
		public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createCategoryDto);//Güncelleme/Ekleme için SerializeObject
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7167/api/Categories", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "Category created!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "Category could not be created!";
			return View();
		}

		public async Task<IActionResult> DeleteCategory(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.DeleteAsync($"https://localhost:7167/api/Categories?id={id}");

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "Category deleted!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "Category could not be deleted!";
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateCategory(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync($"https://localhost:7167/api/Categories/{id}");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<UpdateCategoryDto>(jsonData);
				return View(values);
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateCategoryDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7167/api/Categories", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "Category updated!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "Category could not be updated!";
			return View();
		}
	}
}
