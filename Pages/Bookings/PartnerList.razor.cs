namespace NextDesk.App.Pages.Bookings
{
    using System.Threading.Tasks;
    using NextDesk.Classes.Client;
    using NextDesk.DataTransferObject.API;
    using NextDesk.MongoDataLibrary;
    using NextDesk.MongoDataLibrary.Models.Booking;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class PartnerList : BaseComponentPage
    {
        private List<Booking> bookings = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadPlacesData();
            await base.OnInitializedAsync();
        }

        private async Task LoadPlacesData()
        {
            await BackgroundLoader.StartTaskAsync(async _ =>
            {
                var search = new SearchObj() { UseUserPermission = false, TheResource = MiniMe.GetMiniMe<Place>("65198036b34c5e4378e8920e") };

                var response = await Crud.GenericSearchFor<Booking>(search);
                if (response is not null) bookings = response.ListResult;
            }, nameof(PartnerList));
            StateHasChanged();
        }
    }
}
