namespace ZayShop_Dapper_Ui.Dtos.ProductDtos
{
	public class ResultProductDto
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public double ProductPrice { get; set; }
		public string ProductDescription { get; set; }
		public double ProductRating { get; set; }
		public string ProductBrand { get; set; }
		public string ProductImageUrl { get; set; }
		public int ProductCategory {  get; set; }
	}
}
