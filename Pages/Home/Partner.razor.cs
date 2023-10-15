namespace NextDesk.App.Pages.Home
{
    using System.Threading.Tasks;
    using NextDesk.Classes.Client;
    using NextDesk.DataTransferObject.API;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class Partner : BaseComponentPage
    {
        private List<Place> places = new();

        protected override async Task OnInitializedAsync()
        {
            var response = await Crud.ReadAsync<Place, SearchResult<Place>>("PartnerPermissionPlaces");
            if (response is not null) places = response.ListResult;

            await base.OnInitializedAsync();
        }
    }
}
