using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using ZayShop_Dapper_Ui.Dtos.CategoryDtos;
using ZayShop_Dapper_Ui.Dtos.ProductDtos;

namespace ZayShop_Dapper_Ui.Controllers
{
	[Authorize(Roles = "Admin")]
	public class ProductController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public ProductController(IHttpClientFactory httpClientFactory)
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

		[HttpGet]
		public async Task<IActionResult> CreateProduct()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7167/api/Categories");
			var jsonData = await responseMessage.Content.ReadAsStringAsync();
			var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);

			List<SelectListItem> categoryValues = (from x in values
																						 select new SelectListItem
																						 {
																							 Text = x.CategoryName,
																							 Value = x.CategoryId.ToString()
																						 }).ToList();

			ViewBag.v = categoryValues;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto, IFormFile? formFile)
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

				createProductDto.ProductImageUrl = fileName;
			}

			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createProductDto);//Güncelleme/Ekleme için SerializeObject
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7167/api/Products", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "Product created!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "Product could not be created!";
			return View();
		}

		public async Task<IActionResult> DeleteProduct(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.DeleteAsync($"https://localhost:7167/api/Products?id={id}");

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "Product deleted!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "Product could not be deleted!";
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateProduct(int id)
		{
			var clientCategory = _httpClientFactory.CreateClient();
			var responseMessageCategory = await clientCategory.GetAsync("https://localhost:7167/api/Categories");
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync($"https://localhost:7167/api/Products/{id}");
			var jsonDataCategory = await responseMessageCategory.Content.ReadAsStringAsync();
			var valuesCategory = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonDataCategory);

			List<SelectListItem> categoryValues = (from x in valuesCategory
																						 select new SelectListItem
																						 {
																							 Text = x.CategoryName,
																							 Value = x.CategoryId.ToString()
																						 }).ToList();			

			if (responseMessage.IsSuccessStatusCode && responseMessageCategory.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<UpdateProductDto>(jsonData);
				ViewBag.v = categoryValues;
				return View(values);
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateProductDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7167/api/Products", stringContent);

			if (responseMessage.IsSuccessStatusCode)
			{
				TempData["Success"] = "Product updated!";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "Product could not be updated!";
			return View();
		}
	}
}
