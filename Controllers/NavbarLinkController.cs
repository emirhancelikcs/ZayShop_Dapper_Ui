using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ZayShop_Dapper_Ui.Dtos.NavbarLinkDtos;

namespace ZayShop_Dapper_Ui.Controllers
{
	[Authorize(Roles = "Admin")]
	public class NavbarLinkController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public NavbarLinkController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7167/api/NavbarLink");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultNavbarLinkDto>>(jsonData);
				//Listeleme için DeserializeObject

				return View(values);
			}

			return View();
		}

		public IActionResult CreateNavbarLink() => View();

		[HttpPost]
		public async Task<IActionResult> CreateNavbarLink(CreateNavbarLinkDto createNavbarLinkDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createNavbarLinkDto);//Güncelleme/Ekleme için SerializeObject
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7167/api/NavbarLink", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "NavbarLink created!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "NavbarLink could not be created!";
			return View();
		}

		public async Task<IActionResult> UpdateNavbarLink(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync($"https://localhost:7167/api/NavbarLink/{id}");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<UpdateNavbarLinkDto>(jsonData);
				return View(values);
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateNavbarLink(UpdateNavbarLinkDto updateNavbarLinkDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateNavbarLinkDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7167/api/NavbarLink", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "NavbarLink updated!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "NavbarLink could not be updated!";
			return View();
		}

		public async Task<IActionResult> DeleteNavbarLink(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.DeleteAsync($"https://localhost:7167/api/NavbarLink?navbarLinkId={id}");

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "NavbarLink deleted!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "NavbarLink could not be deleted!";
			return View();
		}
	}
}
