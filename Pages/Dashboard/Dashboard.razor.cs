namespace NextDesk.App.Pages.Dashboard
{
    using System.Threading.Tasks;
    using NextDesk.Classes.Client;
    using NextDesk.DataTransferObject.Places;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class Dashboard : BaseComponentPage
    {
        private List<Place> places = new();

        private List<Place> appuntamenti = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadData(State.CurrentUser.IsPartner);

            await base.OnInitializedAsync();
        }

        private async Task LoadData(bool isParner)
        {
            ContentLoading = true;
            if (isParner)
            {
                await BackgroundLoader.StartTaskAsync(async (_) =>
                {
                    var response = await Client.GetAsync<OwnerPlacesResult>("/api/MongoPlace/ReadPartnerPlaces");
                    if (response.Success && response.Content is not null) places = response.Content.Places;
                }, "placesList");
            }
            else
            {
                await BackgroundLoader.StartTaskAsync(async (_) =>
                {
                    var response = await Client.GetAsync<PlacesResult>("/api/MongoPlace/SearchForAllPlaces");
                    if (response.Success && response.Content is not null) appuntamenti = response.Content.Places;
                }, "bookingList");
            }

            ContentLoading = false;
        }
    }
}

