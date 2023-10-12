namespace NextDesk.App.Pages.Bookings
{
    using NextDesk.Classes.Client;

    public partial class Bookings : BaseComponentPage
    {
        protected override async Task OnInitializedAsync()
        {
            Navigator.NavigateTo("/bookings/" + (State.CurrentUser.IsPartner ? "partnerlist" : "userlist"));
            await base.OnInitializedAsync();
        }
    }
}
