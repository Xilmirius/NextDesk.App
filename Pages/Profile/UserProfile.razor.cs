namespace NextDesk.App.Pages.Profile
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
    using NextDesk.Classes.Client;
    using NextDesk.Classes.Form;
    using NextDesk.DataTransferObject.Login;
    using NextDesk.DataTransferObject.Places;
    using NextDesk.MongoDataLibrary.Models.Users;
    using System.Threading.Tasks;

    public partial class UserProfile : BaseComponentPage
    {
        private AccountUpdate form = new();
        private FormHandler handler = new();

        private Account? account;

        [Parameter]
        public string Id { get; set; } = string.Empty;

        [Inject]
        public AuthenticationStateProvider AuthProv { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            account = await Crud.ReadAsync<Account>(Id);
            if (account is not null)
            {
                form.SetData(account);
                handler.SetFields(form.GetAllFields());
            }
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

        private async Task OnClickSaveAccount()
        {
            if (account is null) return;

            var result = form.ValidateFields();
            handler.SetValidationResult(result);
            if (!result.IsValid) return;

            var response = await Crud.UpdateAsync(form, account);
            if (response.Success && response.Content is not null) account = response.Content;
        }
    }
}
