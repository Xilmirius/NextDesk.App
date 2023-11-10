namespace NextDesk.App.Pages
{
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Components;
    using NextDesk.Classes.Client;
    using NextDesk.Classes.Form.Users;
    using NextDesk.DataTransferObject.Login;
    using System.Threading.Tasks;
    using NextDesk.Classes.Form;
    using NextDesk.Utilitity;
    using NextDesk.Classes.Translation;

    public partial class Index : BaseComponentPage
    {
        private bool Loading = false;
        private string Error = string.Empty;

        private PasswordDialog passwordDialog = new();

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
                    if (State.CurrentUser.IsAuthenticated && State.CurrentUser.Expiration > DateTime.UtcNow)
                        Navigator.NavigateTo(State.CurrentUser.IsPartner ? "/home/partner" : "/home/user");
                }

                Loading = false;
            }

            await base.OnInitializedAsync();
        }

        public async Task Login()
        {
            var result = LoginForm.ValidateFields();
            handler.SetValidationResult(result);

            if (!result.IsValid) return;

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
                        Navigator.NavigateTo(State.CurrentUser.IsPartner ? "/home/partner" : "/home/user");
                    }
                }
            }
            else
            {
                Error = "Error: " + response.Error.ErrorDescription;
            }

            Loading = false;
            StateHasChanged();
        }

        public async Task ResetPassword()
        {
            passwordDialog.Error = string.Empty;
            if (string.IsNullOrWhiteSpace(passwordDialog.Email))
            {
                passwordDialog.Error = AppTexts.Empty_Field;
                return;
            }

            if (!Validators.IsEmailValid(passwordDialog.Email))
            {
                passwordDialog.Error = AppTexts.Email_Not_Valid;
                return;
            }

            var data = new LoginModel()
            {
                Email = passwordDialog.Email,
                Password = "reset"
            };

            var response = await Client.PostAsync<LoginResult>("/api/resetpassword", data);
            if (response.Success && response.Content is not null)
            {
                if (response.Content.Successful)
                {
                    passwordDialog.Sent = true;
                }
            }
            else
            {
                passwordDialog.Error = response.Error.ErrorDescription;
            }

            StateHasChanged();
        }
    }
}
