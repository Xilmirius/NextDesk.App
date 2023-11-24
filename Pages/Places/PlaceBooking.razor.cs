namespace NextDesk.App.Pages.Places
{
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using NextDesk.Classes.Client;
    using NextDesk.DataTransferObject.Bookings;
    using NextDesk.MongoDataLibrary.Helpers.Org;
    using NextDesk.MongoDataLibrary.Models.Booking;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class PlaceBooking : BaseComponentPage
    {
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

            var today = DateTime.UtcNow.Date;
            for (int dayIndex = 0; dayIndex <= 7; dayIndex++)
            {
                var day = today.AddDays(dayIndex);
                var config = place.Calendar.Days.Find(f => f.DayOfWeek == day.DayOfWeek);

                if (config is null) return;
                nextBookingDays.Add(new DayItemConfiguration(config, day));
            }
        }

        private string StyleDateButton(DateTime date)
        {
            return SelectedDay?.Date == date ? "background-color: #00cb74" : string.Empty;
        }

        public List<string> OpenTimeRanges()
        {
            var list = new List<string>();
            if (place is null) return list;

            foreach (var day in place.Calendar.Days.Where(w => w.DayOfWeek == DateTime.UtcNow.DayOfWeek))
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

        private void ShowBookingDialog()
        {
            if (SelectedDay is null) return;

            bookingDialog.Show(Id, SelectedDay);
        }

        private async Task OnClickCreateBooking()
        {
            if (place is null || SelectedDay is null) return;

            var response = await Crud.CreateAsync<Booking, BookingCreate>(bookingDialog.form);
            if (response.Success && response.Content is not null)
            {
                Navigator.NavigateTo("/bookings/details/" + response.Content.Id);
            }
            else
            {
                bookingDialog.Error = response.Error.ErrorDescription;
            }
        }

        private string ShortDescription()
        {
            if (place is null) return string.Empty;
            return place.Info.Description.Length > 45 ? place.Info.Description[..44] : place.Info.Description;
        }
    }

    public class DayItemConfiguration(DayConfiguration config, DateTime date)
    {
        public DayConfiguration Config { get; } = config;

        public DateTime Date { get; } = date;

        public TimeRange? Range { get; set; }
    }
}