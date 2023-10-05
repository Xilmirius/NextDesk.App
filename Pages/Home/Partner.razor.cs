namespace NextDesk.App.Pages.Home
{
    using System.Threading.Tasks;
    using NextDesk.Classes.Client;
    using NextDesk.DataTransferObject.Places;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class Partner : BaseComponentPage
    {
        private List<Place> places = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadPlacesData();

            await base.OnInitializedAsync();
        }

        private async Task LoadPlacesData()
        {
            var response = await Crud.ReadAsync<Place, PlacesResult>("PartnerPermissionPlaces");
            if (response is not null) places = response.Places;
        }
    }
}
