namespace NextDesk.App.Pages
{
    using NextDesk.Classes.Client;
    using NextDesk.Classes.Form;
    using NextDesk.Classes.Form.Users;
    using NextDesk.DataTransferObject.Login;

    public partial class Registration : BaseComponentPage
    {
        private FormHandler userHandler = new();
        private FormHandler partnerHandler = new();

        private string tab = "User";
        private string error = string.Empty;

        private bool AbleToCreate => Terms && Privacy && !Loading;
        private bool Loading = false;
        private bool Privacy = false;
        private bool Terms = false;

        public UserRegistrationForm UserFormData { get; set; } = new();
        public PartnerRegistrationForm PartnerFormData { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            userHandler.BindFormData(UserFormData);
            partnerHandler.BindFormData(PartnerFormData);

            await base.OnInitializedAsync();
        }

        public async Task Register()
        {
            error = string.Empty;
            if (tab == "User") await RegisterUser();
            if (tab == "Business") await RegisterPartner();
        }

        public async Task RegisterUser()
        {
            var result = userHandler.Validate();

            if (result.IsValid)
            {
                Loading = true;

                var data = new RegisterModel()
                {
                    FirstName = UserFormData.FirstName,
                    LastName = UserFormData.LastName,
                    Email = UserFormData.Email,
                    Password = UserFormData.Password,
                    Birth = UserFormData.Birth ?? new(),
                    Gender = UserFormData.Gender,
                };

                var response = await Client.PostAsync<RegisterResult>("/api/registeruser", data);
                if (response.Success && response.Content is not null)
                {
                    if (response.Content.Successful)
                    {
                        Navigator.NavigateTo("/");
                    }
                }
                else
                {
                    error = response.Error.ErrorDescription;
                }
            }

            Loading = false;
        }

        public async Task RegisterPartner()
        {
            var result = partnerHandler.Validate();

            if (result.IsValid)
            {
                Loading = true;

                var data = new RegisterModel()
                {
                    RefFirstName = PartnerFormData.RefFirstName,
                    RefLastName = PartnerFormData.RefLastName,
                    Email = PartnerFormData.Email,
                    Password = PartnerFormData.Password,
                    IsPartner = true,
                    ActivityName = PartnerFormData.ActivityName,
                    Telephone = PartnerFormData.Telephone,
                    WebSite = PartnerFormData.WebSite,
                    PartitaIva = PartnerFormData.PartitaIva,
                    Address = PartnerFormData.Address,
                };

                var response = await Client.PostAsync<RegisterResult>("/api/registerpartner", data);
                if (response.Success && response.Content is not null)
                {
                    if (response.Content.Successful)
                    {
                        Navigator.NavigateTo("/");
                    }
                }
                else
                {
                    error = response.Error.ErrorDescription;
                }
            }

            Loading = false;
        }

        private void SelectedTab(string selected) => tab = selected;
    }
}
