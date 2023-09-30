namespace NextDesk.App.Pages.Home
{
    using System.Threading.Tasks;
    using NextDesk.Classes.Client;

    public partial class Home : BaseComponentPage
    {
        protected override async Task OnInitializedAsync()
        {
            Navigator.NavigateTo("/home/" + (State.CurrentUser.IsPartner ? "partner" : "user"));
            await base.OnInitializedAsync();
        }
    }
}

