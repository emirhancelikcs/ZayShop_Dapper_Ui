using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZayShop_Dapper_Ui.Dtos.InfosDtos;
using ZayShop_Dapper_Ui.Dtos.NavbarLinkDtos;

namespace ZayShop_Dapper_Ui.ViewComponents.Layout
{
	public class _NavbarViewComponentPartial : ViewComponent
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public _NavbarViewComponentPartial(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var client1 = _httpClientFactory.CreateClient();
			var client2 = _httpClientFactory.CreateClient();
			var responseMessageForInfos = await client1.GetAsync("https://localhost:7167/api/Infos");
			var responseMessageForLinks = await client2.GetAsync("https://localhost:7167/api/NavbarLink");

			if (responseMessageForInfos.IsSuccessStatusCode && responseMessageForLinks.IsSuccessStatusCode)
			{
				var jsonDataForInfos = await responseMessageForInfos.Content.ReadAsStringAsync();
				var valuesForInfos = JsonConvert.DeserializeObject<ResultInfosDto>(jsonDataForInfos);

				var jsonDataForLinks = await responseMessageForLinks.Content.ReadAsStringAsync();
				var valuesForLinks = JsonConvert.DeserializeObject<List<ResultNavbarLinkDto>>(jsonDataForLinks);

				ViewBag.InfosEmail = valuesForInfos.InfosEmail;
				ViewBag.InfosPhone = valuesForInfos.InfosPhone;
				ViewBag.InfosFB = valuesForInfos.InfosFacebookAddress;
				ViewBag.InfosIG = valuesForInfos.InfosInstagramAddress;
				ViewBag.InfosTW = valuesForInfos.InfosTwitterAddress;
				ViewBag.InfosLI = valuesForInfos.InfosLinkedinAddress;

				return View(valuesForLinks); 
			}

			return View();
		}
	}
}
