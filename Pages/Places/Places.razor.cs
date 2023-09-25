namespace NextDesk.App.Pages.Places
{
    using System.Threading.Tasks;
    using NextDesk.Classes.Client;
    using NextDesk.DataTransferObject.Places;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class Places : BaseComponentPage
    {
        private List<Place> places = new();

        protected override async Task OnInitializedAsync()
        {
            ContentLoading = true;

            await LoadPlacesData();

            await base.OnInitializedAsync();
        }

        private async Task LoadPlacesData()
        {
            var response = await Client.GetAsync<PlacesResult>("/api/MongoPlace/SearchForAllPlaces");
            if (response.Success && response.Content is not null) places = response.Content.Places;
        }
    }
}
