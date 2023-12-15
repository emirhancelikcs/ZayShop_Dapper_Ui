using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZayShop_Dapper_Ui.Dtos.CategoryDtos;

namespace ZayShop_Dapper_Ui.Areas.Identity.Data
{
	public class ZayShopDbContext : IdentityDbContext<IdentityUser>
	{
		public ZayShopDbContext(DbContextOptions<ZayShopDbContext> options)
				: base(options)
		{
		}
	}
}
