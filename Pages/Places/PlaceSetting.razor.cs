namespace NextDesk.App.Pages.Places
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using NextDesk.Classes.Client;
    using NextDesk.Classes.OptionsFor;
    using NextDesk.DataTransferObject.Places;
    using NextDesk.MongoDataLibrary.Models.Org;
    using NextDesk.MongoDataLibrary.Models.Sys;

    public partial class PlaceSetting : BaseComponentPage
    {
        private Place? place;
        private StyleSetting? style;

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
                style = await Crud.ReadAsync<StyleSetting>(place.TheStyle);
            }
            await base.OnInitializedAsync();
        }

        private async Task OnClickSaveDescription()
        {
            if (place is null) return;

            var result = descriptionForm.ValidateFields();
            if (!result.IsValid) return;

            var response = await Crud.UpdateAsync(descriptionForm, place);
            if (response.Success && response.Content is not null) place = response.Content;
        }

        private string GetImageUrl()
        {
            if (style is null || style.Images.Count == 0) return " /images/coffe.jpg";
            else return style.Images[0].Url;
        }

        private async Task OnClickSaveServices()
        {
            if (place is null) return;

            var result = servicesForm.ValidateFields();
            if (!result.IsValid) return;

            var response = await Crud.UpdateAsync(servicesForm, place);
            if (response.Success && response.Content is not null) place = response.Content;
        }

        private async Task SalvaDaySetting()
        {
            if (place is null) return;

            var result = editDayDialog.form.ValidateFields();
            if (!result.IsValid) return;

            var response = await Crud.UpdateAsync(editDayDialog.form, place);
            if (response.Success && response.Content is not null)
            {
                place = response.Content;
                editDayDialog.Hide();
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
            if (!result.IsValid) return;

            var response = await Crud.UpdateAsync(editTableDialog.form, place);
            if (response.Success && response.Content is not null)
            {
                place = response.Content;
                editTableDialog.Hide();
            }
        }
    }
}