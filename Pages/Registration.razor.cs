namespace NextDesk.App.Pages
{
    using NextDesk.Classes.Client;
    using NextDesk.Classes.Form;
    using NextDesk.Classes.Form.Users;
    using NextDesk.DataTransferObject.Login;

    public partial class Registration : BaseComponentPage
    {
        private bool Loading = false;
        private FormHandler handler = new();

        public UserRegistrationForm UserFormData { get; set; } = new();
        public PartnerRegistrationForm PartnerFormData { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            handler.BindFormData(UserFormData);

            await base.OnInitializedAsync();
        }

        public async Task Register()
        {
            var result = handler.Validate();

            if (result.IsValid)
            {
                Loading = true;

                var data = new RegisterModel()
                {
                    FirstName = UserFormData.FirstName,
                    LastName = UserFormData.LastName,
                    Email = UserFormData.Email,
                    Password = UserFormData.Password,
                    Age = UserFormData.Eta,
                    Gender = UserFormData.Sesso,
                };

                var response = await Client.PostAsync<RegisterResult>("/api/register", data);
                if (response.Success && response.Content is not null)
                {
                    if (response.Content.Successful)
                    {
                        Navigator.NavigateTo("/");
                    }
                }
            }

            Loading = false;
        }
    }
}
