using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using ZayShop_Dapper_Ui.Dtos.CartDtos;
using ZayShop_Dapper_Ui.Dtos.ProductDtos;

namespace ZayShop_Dapper_Ui.Controllers
{
	[Authorize]
	public class CartController : Controller
	{		
		private readonly IHttpClientFactory _httpClientFactory;

		public CartController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IActionResult> Index()
		{
			var client = _httpClientFactory.CreateClient();
			string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var responseMessage = await client.GetAsync($"https://localhost:7167/api/Cart/{UserId}");

			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultCartDto>>(jsonData);
				//Listeleme için DeserializeObject
				
				return View(values);
			}

			return View();
		}

		public async Task<IActionResult> AddToCart(int id)
		{
			AddCartDto addCartDto = new();
			string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			addCartDto.UserId = UserId;
			addCartDto.ProductId = id;

			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(addCartDto);//Güncelleme/Ekleme için SerializeObject
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7167/api/Cart", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				//TempData["Success"] = "Category created!";
				return RedirectToAction("Index", "Cart");
			}

			//TempData["Error"] = "Category could not be created!";
			return RedirectToAction("Index", "Shop");
		}

		public async Task<IActionResult> Delete(int id)
		{
			string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.DeleteAsync($"https://localhost:7167/api/Cart?ProductId={id}&UserId={UserId}");

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "Category deleted!";
				return RedirectToAction("Index", "Cart");
			}

			TempData["Error"] = "Category could not be deleted!";
			return RedirectToAction("Index", "Shop");
		}
	}
}
