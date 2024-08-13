namespace Cinema.Helpers
{
    public class NavigationManager
    {
        public PagesTagsEnum PreviousPage { get; set; } = PagesTagsEnum.LocalsFilmsPage;
        public PagesTagsEnum CurrentPage { get; set; } = PagesTagsEnum.LocalsFilmsPage;
    }
}