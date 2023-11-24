namespace NextDesk.App.Pages.Home
{
    using System.Threading.Tasks;
    using NextDesk.Classes.Client;
    using NextDesk.DataTransferObject.API;
    using NextDesk.DataTransferObject.Places;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class User : BaseComponentPage
    {
        private List<PlaceDetailsDTO> places = new();
        private string stringSearch = string.Empty;

        public string Search
        {
            get => stringSearch;
            set
            {
                stringSearch = value;
                SetPollingTimer(350);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadPlacesData();
            await base.OnInitializedAsync();
        }

        private async Task LoadPlacesData()
        {
            await BackgroundLoader.StartTaskAsync(async _ =>
            {
                var search = new SearchObj()
                {
                    StringSearch = Search,
                };

                var response = await Crud.GenericDTOSearchFor<Place, PlaceDetailsDTO>(search);
                if (response is not null) places = response.ListResult;
            }, nameof(User));
            StateHasChanged();
        }

        public override async Task OnPollingAsync()
        {
            await LoadPlacesData();
        }
    }
}