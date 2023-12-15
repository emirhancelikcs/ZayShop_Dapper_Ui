namespace ZayShop_Dapper_Ui.Dtos.NavbarLinkDtos
{
	public class UpdateNavbarLinkDto
	{
		public int NavbarLinkId { get; set; }
		public string NavbarLinkText { get; set; }
		public string NavbarLinkController { get; set; }
		public string NavbarLinkAction { get; set; }
	}
}
