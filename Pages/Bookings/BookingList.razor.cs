namespace NextDesk.App.Pages.Bookings
{
    using System.Threading.Tasks;
    using NextDesk.Classes.Client;
    using NextDesk.DataTransferObject;
    using NextDesk.DataTransferObject.API;
    using NextDesk.DataTransferObject.Places;
    using NextDesk.MongoDataLibrary;
    using NextDesk.MongoDataLibrary.Models.Booking;
    using NextDesk.MongoDataLibrary.Models.Org;
    using NextDesk.MongoDataLibrary.Models.Users;

    public partial class BookingList : BaseComponentPage
    {
        private List<Booking> bookings = [];
        private List<PairValueDescription> places = [];
        private string SelectedPlace = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            if (State.CurrentUser.IsPartner) await LoadPlacesAsync();
            await LoadBookingsAsync();
            await base.OnInitializedAsync();
        }

        private async Task LoadBookingsAsync()
        {
            await BackgroundLoader.StartTaskAsync(async _ =>
            {
                var resource = State.CurrentUser.IsPartner ? MiniMe.GetMiniMe<Place>(SelectedPlace) : MiniMe.GetMiniMe<Account>(State.CurrentUser.Id);
                var search = new SearchObj()
                {
                    UseUserPermission = State.CurrentUser.IsPartner,
                    TheResource = resource
                };

                var response = await Crud.GenericSearchFor<Booking>(search);
                if (response is not null) bookings = response.ListResult;
            }, nameof(BookingList));
            StateHasChanged();
        }

        private async Task LoadPlacesAsync()
        {
            var response = await Crud.ReadAsync<Place, SearchResult<PlaceDetailsDTO>>("PartnerPermissionPlaces");
            if (response is not null) places = response.ListResult.ConvertAll(c => new PairValueDescription(c.Id, c.Name));
            if (places.Count > 0) SelectedPlace = places[0].Value;
            StateHasChanged();
        }

        public override async Task OnPollingAsync()
        {
            await LoadBookingsAsync();
        }
    }
}