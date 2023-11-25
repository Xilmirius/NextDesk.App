namespace NextDesk.App.Pages.Home
{
    using System.Threading.Tasks;
    using NextDesk.Classes.Client;
    using NextDesk.DataTransferObject.API;
    using NextDesk.DataTransferObject.Places;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class Partner : BaseComponentPage
    {
        private List<PlaceDetailsDTO> places = [];

        protected override async Task OnInitializedAsync()
        {
            var response = await Crud.ReadAsync<Place, SearchResult<PlaceDetailsDTO>>("PartnerPermissionPlaces");
            if (response is not null) places = response.ListResult;

            await base.OnInitializedAsync();
        }
    }
}