namespace NextDesk.App.Pages.Places
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using NextDesk.Classes.Client;
    using NextDesk.MongoDataLibrary.Helpers.Org;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class PlaceBooking : BaseComponentPage
    {
        private bool ShowDescription => place?.Description.Length > 45 && showFullDescription;
        private bool showFullDescription = false;

        private Place? place;
        private List<DayItemConfiguration> nextBookingDays = new();

        private DateTime? SelectedDate;

        [Parameter]
        public string Id { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            place = await Crud.ReadAsync<Place>(Id);

            CreateDaysList();
            await base.OnInitializedAsync();
        }

        private void CreateDaysList()
        {
            if (place is null) return;

            var today = DateTime.Now.Date;
            for (int dayIndex = 0; dayIndex <= 7; dayIndex++)
            {
                var day = today.AddDays(dayIndex);
                var config = place.Calendar.Days.Find(f => f.DayOfWeek == day.DayOfWeek);

                if (config is null) return;
                nextBookingDays.Add(new DayItemConfiguration(config, day));
            }
        }

        private string CssDateButton(DateTime date)
        {
            var selected = SelectedDate == date ? "btn-success" : string.Empty;
            return $"btn m-2 p-1 border rounded-2 {selected}";
        }
    }

    public class DayItemConfiguration
    {
        public DayConfiguration Config { get; }

        public DateTime Date { get; }

        public DayItemConfiguration(DayConfiguration config, DateTime date)
        {
            Config = config;
            Date = date;
        }
    }
}
