namespace NextDesk.App.Pages.Places
{
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using NextDesk.Classes.Client;
    using NextDesk.MongoDataLibrary.Helpers.Org;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class PlaceBooking : BaseComponentPage
    {
        private bool ShowDescription => place?.Info.Description.Length > 45 && showFullDescription;
        private bool showFullDescription = false;

        private Place? place;
        private List<DayItemConfiguration> nextBookingDays = new();

        private DayItemConfiguration? SelectedDay;

        private BookingDialog bookingDialog = new();

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
            var selected = SelectedDay?.Date == date ? "btn-success" : string.Empty;
            return $"btn m-2 p-1 border rounded-2 {selected}";
        }

        public List<string> OpenTimeRanges()
        {
            var list = new List<string>();
            if (place is null) return list;

            foreach (var day in place.Calendar.Days.Where(w => w.DayOfWeek == DateTime.Now.DayOfWeek))
            {
                foreach (var time in day.TimeRanges)
                {
                    var timeZone = TimeZoneInfo.FindSystemTimeZoneById(place.Calendar.TimeZone);
                    var placeLocalTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZone);
                    var currentLocalTime = placeLocalTime.TimeOfDay;

                    list.Add($"{time.OpenTime.Hours}:{time.OpenTime.Minutes} - {time.CloseTime.Hours}:{time.CloseTime.Minutes}");
                }
            }

            return list;
        }

        private void SelectDay(DayItemConfiguration day)
        {
            SelectedDay = day;
            SelectedDay.Range = null;
        }

        private string ConvertTimeRange(TimeRange range)
        {
            if (SelectedDay is null) return string.Empty;
            var open = SelectedDay.Date.Date.Add(range.OpenTime).ToString("HH:mm", CultureInfo.CurrentUICulture);
            var close = SelectedDay.Date.Date.Add(range.CloseTime).ToString("HH:mm", CultureInfo.CurrentUICulture);
            return $"{open} - {close}";
        }

        private async Task OnClickCreateBooking()
        {

        }
    }

    public class DayItemConfiguration
    {
        public DayConfiguration Config { get; }

        public DateTime Date { get; }

        public TimeRange? Range { get; set; }

        public DayItemConfiguration(DayConfiguration config, DateTime date)
        {
            Config = config;
            Date = date;
        }
    }
}
