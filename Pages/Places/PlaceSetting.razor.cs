namespace NextDesk.App.Pages.Places
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using NextDesk.Classes.Client;
    using NextDesk.Classes.OptionsFor;
    using NextDesk.DataTransferObject.Places;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class PlaceSetting : BaseComponentPage
    {
        private Place? place;

        private PlaceServicesUpdate servicesForm = new();
        private PlaceDescriptionUpdate descriptionForm = new();
        private DaysDialog editDayDialog = new();
        private TablesDialog editTableDialog = new();

        [Parameter]
        public string Id { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            place = await Crud.ReadAsync<Place>(Id);
            if (place is not null)
            {
                servicesForm.SetData(place.Info.Services, OptionsForPlaces.Services);
                descriptionForm.SetData(place);
            }
            await base.OnInitializedAsync();
        }

        private async Task OnClickSaveDescription()
        {
            if (place is null) return;

            var result = descriptionForm.ValidateFields();
            if (result.IsValid)
            {
                var response = await Crud.UpdateAsync(descriptionForm, place);
                if (response.Success && response.Content is not null) place = response.Content;
            }
        }

        private async Task OnClickSaveServices()
        {
            if (place is null) return;

            var result = servicesForm.ValidateFields();
            if (result.IsValid)
            {
                var response = await Crud.UpdateAsync(servicesForm, place);
                if (response.Success && response.Content is not null) place = response.Content;
            }
        }

        private async Task SalvaDaySetting()
        {
            if (place is null) return;

            var result = editDayDialog.form.ValidateFields();
            if (result.IsValid)
            {
                var response = await Crud.UpdateAsync(editDayDialog.form, place);
                if (response.Success && response.Content is not null)
                {
                    place = response.Content;
                    editDayDialog.Hide();
                }
            }
        }

        private void OnClickTablesDialog()
        {
            if (place is null) return;

            editTableDialog.Show(place.Booking);
        }

        private async Task OnClickSaveTables()
        {
            if (place is null) return;

            var result = editTableDialog.form.ValidateFields();
            if (result.IsValid)
            {
                var response = await Crud.UpdateAsync(editTableDialog.form, place);
                if (response.Success && response.Content is not null)
                {
                    place = response.Content;
                    editTableDialog.Hide();
                }
            }
        }
    }
}
