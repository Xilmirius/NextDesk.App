namespace NextDesk.App.Pages
{
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Components;
    using NextDesk.Classes.Client;
    using NextDesk.Classes.Form.Users;
    using NextDesk.DataTransferObject.Login;
    using System.Threading.Tasks;
    using NextDesk.Classes.Form;

    public partial class Index : BaseComponentPage
    {
        private bool Loading = false;
        private string Error = string.Empty;

        private FormHandler handler = new();
        public UserLoginForm LoginForm { get; set; } = new();

        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            handler.SetFields(LoginForm.GetAllFields());

            if (State.CurrentUser.IsAuthenticated)
            {
                Loading = true;
                await Client.AuthPingServer();

                if (AuthStateProvider is ApiAuthenticationStateProvider service)
                {
                    await service.GetAuthenticationStateAsync();
                    if (State.CurrentUser.IsAuthenticated && State.CurrentUser.Expiration > DateTime.UtcNow) Navigator.NavigateTo("/home");
                }

                Loading = false;
            }

            await base.OnInitializedAsync();
        }

        public async void Login()
        {
            var result = LoginForm.ValidateFields();
            handler.SetValidationResult(result);

            if (result.IsValid)
            {
                Loading = true;
                var data = new LoginModel()
                {
                    Email = LoginForm.Email,
                    Password = LoginForm.Password,
                    RememberMe = true,
                };

                var response = await Client.PostAsync<LoginResult>("/api/login", data);
                if (response.Success && response.Content is not null)
                {
                    if (response.Content.Successful)
                    {
                        if (AuthStateProvider is ApiAuthenticationStateProvider service && await service.MarkUserAsAuthenticatedAsync(response.Content.Token))
                        {
                            Navigator.NavigateTo("/home");
                        }
                    }
                }
                else
                {
                    Error = "Error: " + response.Error.ErrorDescription;
                }
            }

            Loading = false;
            StateHasChanged();
        }
    }
}
