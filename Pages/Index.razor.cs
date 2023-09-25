namespace NextDesk.App.Pages
{
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Components;
    using NextDesk.Classes.Client;
    using NextDesk.Classes.Form.Users;
    using NextDesk.DataTransferObject.Login;
    using System.Threading.Tasks;
    using NextDesk.Classes.Form;
    using NextDesk.Classes.Toast;

    public partial class Index : BaseComponentPage
    {
        private bool Loading = false;
        private FormHandler handler = new();
        public UserLoginForm UserForm { get; set; } = new();

        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            handler.BindFormData(UserForm);

            if (State.CurrentUser.IsAuthenticated) await Client.AuthPingServer();

            if (AuthStateProvider is ApiAuthenticationStateProvider service)
            {
                await service.GetAuthenticationStateAsync();
                if (State.CurrentUser.IsAuthenticated) Navigator.NavigateTo("/dashboard");
            }

            await base.OnInitializedAsync();
        }

        public async void Login()
        {
            var result = handler.Validate();

            if (result.IsValid)
            {
                Loading = true;
                var data = new LoginModel()
                {
                    Email = UserForm.Email,
                    Password = UserForm.Password,
                    RememberMe = true,
                };

                var response = await Client.PostAsync<LoginResult>("/api/login", data);
                if (response.Success && response.Content is not null)
                {
                    if (response.Content.Successful)
                    {
                        if (AuthStateProvider is ApiAuthenticationStateProvider service && await service.MarkUserAsAuthenticatedAsync(response.Content.Token))
                        {
                            Navigator.NavigateTo("/dashboard");
                        }
                    }
                }
                else
                {
                    Toast.ShowNotification("Error", response.Error.ErrorDescription, ToastType.Error);
                }
            }

            Loading = false;
            StateHasChanged();
        }
    }
}
