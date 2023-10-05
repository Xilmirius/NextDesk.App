namespace NextDesk.App.Pages.Places
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using NextDesk.Classes.Client;
    using NextDesk.DataTransferObject.Places;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class PlaceSetting : BaseComponentPage
    {
        private Place? place;
        private PlaceServicesUpdate servicesForm = new();
        private PlaceDescriptionUpdate descriptionForm = new();
        private PlaceDaySettingUpdate editDay = new();

        [Parameter]
        public string Id { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            place = await Crud.ReadAsync<Place>(Id);
            if (place is not null)
            {
                servicesForm.SetData(place.Info.Services);
                descriptionForm.SetData(place.Info.Description);
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

            var result = editDay.ValidateFields();
            if (result.IsValid)
            {
                var response = await Crud.UpdateAsync(editDay, place);
                if (response.Success && response.Content is not null)
                {
                    place = response.Content;
                    editDay.visibleDialog = false;
                }
            }
        }
    }
}
