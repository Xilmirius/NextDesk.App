namespace NextDesk.App.Pages.Bookings
{
    using System.Threading.Tasks;
    using NextDesk.Classes.Client;
    using NextDesk.DataTransferObject.API;
    using NextDesk.MongoDataLibrary.Models.Booking;

    public partial class UserList : BaseComponentPage
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
                var search = new SearchObj() { UseUserPermission = true };

                var response = await Crud.GenericSearchFor<Booking>(search);
                if (response is not null) bookings = response.ListResult;
            }, nameof(UserList));
            StateHasChanged();
        }

        public override async Task OnPollingAsync()
        {
            await LoadPlacesData();
        }
    }
}
