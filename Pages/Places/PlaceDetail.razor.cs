namespace NextDesk.App.Pages.Places
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using NextDesk.Classes.Client;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class PlaceDetail : BaseComponentPage
    {
        private Place? place;

        [Parameter]
        public string Id { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            place = await Crud.ReadAsync<Place>(Id);

            await base.OnInitializedAsync();
        }
    }
}
