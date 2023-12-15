using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ZayShop_Dapper_Ui.Dtos.IndexCarouselDtos;

namespace ZayShop_Dapper_Ui.Controllers
{
	[Authorize(Roles = "Admin")]
	public class IndexCarouselController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public IndexCarouselController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7167/api/IndexCarousel");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultIndexCarouselDto>>(jsonData);
				//Listeleme için DeserializeObject

				return View(values);
			}

			return View();
		}

		public IActionResult CreateIndexCarousel() => View();

		[HttpPost]
		public async Task<IActionResult> CreateIndexCarousel(CreateIndexCarouselDto createIndexCarouselDto, IFormFile? formFile)
		{
			if (formFile != null)
			{
				if (!Directory.Exists("wwwroot/img"))
					Directory.CreateDirectory("wwwroot/img");

				string fileExtension = Path.GetExtension(formFile.FileName);
				string fileName = Guid.NewGuid() + fileExtension;
				string fileSavePath = Path.Combine("wwwroot/img", fileName);

				using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
					formFile.CopyTo(stream);

				createIndexCarouselDto.ImageName = fileName;
			}

			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createIndexCarouselDto);//Güncelleme/Ekleme için SerializeObject
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7167/api/IndexCarousel", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "IndexCarousel created!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "IndexCarousel could not be created!";
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateIndexCarousel(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync($"https://localhost:7167/api/IndexCarousel/{id}");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var value = JsonConvert.DeserializeObject<UpdateIndexCarouselDto>(jsonData);
				//Listeleme için DeserializeObject

				return View(value);
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateIndexCarousel(UpdateIndexCarouselDto updateIndexCarouselDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateIndexCarouselDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7167/api/IndexCarousel", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "IndexCarousel updated!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "IndexCarousel could not be updated!";
			return View();
		}

		public async Task<IActionResult> DeleteIndexCarousel(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.DeleteAsync($"https://localhost:7167/api/IndexCarousel?id={id}");

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "IndexCarousel deleted!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "IndexCarousel could not be deleted!";
			return View();
		}
	}
}
