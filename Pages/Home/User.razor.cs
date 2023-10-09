namespace NextDesk.App.Pages.Home
{
    using System.Threading.Tasks;
    using NextDesk.Classes.Client;
    using NextDesk.DataTransferObject.API;
    using NextDesk.MongoDataLibrary.Models.Booking;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class User : BaseComponentPage
    {
        public string Search { get; set; } = string.Empty;
        private List<Booking> places = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadPlacesData();

            await base.OnInitializedAsync();
        }

        private async Task LoadPlacesData()
        {
            var search = new SearchObj()
            {
                StringSearch = Search,
            };
            var response = await Crud.SearchFor<Booking, SearchObj, SearchResult<Booking>>(search);
            if (response is not null) places = response.ListResult;
        }
    }
}
