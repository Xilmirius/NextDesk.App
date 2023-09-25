namespace NextDesk.App.Pages.Bookings
{
    using System.Threading.Tasks;
    using NextDesk.Classes.Client;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class Bookings : BaseComponentPage
    {
        private List<Place> places = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
