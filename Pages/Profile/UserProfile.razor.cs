namespace NextDesk.App.Pages.Profile
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
    using NextDesk.Classes.Client;
    using NextDesk.DataTransferObject.Login;
    using System.Threading.Tasks;

    public partial class UserProfile : BaseComponentPage
    {
        [Inject]
        public AuthenticationStateProvider AuthProv { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private async void Logout()
        {
            await Client.PostAsync<LoginResult>("/api/logout");
            if (AuthProv is ApiAuthenticationStateProvider service)
            {
                await service.MarkUserAsLoggedOutAsync();
                Navigator.NavigateTo("/");
            }
        }
    }
}
