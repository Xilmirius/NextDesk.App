namespace NextDesk.App.Pages.Bookings
{
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using NextDesk.Classes.Client;
    using NextDesk.MongoDataLibrary.Helpers.Org;
    using NextDesk.MongoDataLibrary.Models.Booking;
    using NextDesk.MongoDataLibrary.Models.Org;
    using NextDesk.MongoDataLibrary.Models.Sys;

    public partial class BookingDetail : BaseComponentPage
    {
        private bool visible = false;

        private StyleSetting? style;
        private Place? place;
        private Booking? booking;

        [Parameter]
        public string Id { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            booking = await Crud.ReadAsync<Booking>(Id);
            if (booking is not null) place = await Crud.ReadAsync<Place>(booking.ThePlace);
            if (place is not null) style = await Crud.ReadAsync<StyleSetting>(place.TheStyle);

            await base.OnInitializedAsync();
        }

        private async Task OnClickDelete()
        {
            if (booking is null) return;

            var response = await Crud.DeleteAsync(booking);
            if (response.Success && response.Content is not null && response.Content.Success)
                booking = await Crud.ReadAsync<Booking>(Id);

            visible = false;
        }

        private string GetStatusCssContainer()
        {
            var style = "rounded-4 p-2 text-center ";
            if (booking is null) return style + "bg-info-subtle text-white";

            return style + booking.Status switch
            {
                "confirmed" => "bg-success",
                "pending" => "bg-info",
                "canceled" => "bg-danger text-white",
                _ => throw new InvalidOperationException(nameof(booking.Status)),
            };
        }

        public string GetBookingStatusTitle()
        {
            if (booking is null) return "Prenotazione non trovata";
            return booking.Status switch
            {
                "confirmed" => "Prenotazione confermata!",
                "pending" => "Prenotazione in attesa",
                "canceled" => "Prenotazione cancellata",
                _ => throw new InvalidOperationException(nameof(booking.Status)),
            };
        }

        public string GetStatusIcon()
        {
            if (booking is null) return "mb-3 fa-solid fa-question fa-2xl";
            var style = "mb-3 fa-solid ";
            return style + booking.Status switch
            {
                "confirmed" => "fa-circle-check fa-2xl",
                "pending" => "spinner-border fa-2xl",
                "canceled" => "fa-xmark fa-2xl",
                _ => throw new InvalidOperationException(nameof(booking.Status)),
            };
        }

        private string ConvertTimeRange()
        {
            if (booking is null) return string.Empty;
            return booking.BookingDate.Date.Add(booking.Range.OpenTime).ToString("HH:mm", CultureInfo.CurrentUICulture);
        }

        private string GetTableNumber()
        {
            if (booking is null || place is null) return "-";
            var index = place.Booking.Tables.FindIndex(f => f.TableId == booking.TableId);
            return index == -1 ? "-" : $"{index + 1}";
        }

        private string GetImageUrl()
        {
            if (style is null || style.Images.Count == 0) return " /images/coffe.jpg";
            else return style.Images[0].Url;
        }

        private string ShortDescription()
        {
            if (place is null) return string.Empty;
            return place.Info.Description.Length > 45 ? place.Info.Description[..44] : place.Info.Description;
        }
    }
}
