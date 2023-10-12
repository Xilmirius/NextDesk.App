namespace NextDesk.App.Pages.Places
{
    using NextDesk.DataTransferObject.Bookings;

    public partial class BookingDialog
    {
        public BookingCreate form = new();

        public bool Visible { get; set; }

        public void Show(string Id, DayItemConfiguration config)
        {
            if (config is null) throw new ArgumentNullException(nameof(config));
            if (config.Range is null) throw new ArgumentNullException(nameof(config.Range));

            form.SetData(Id, config.Config, config.Date, config.Range);
            Visible = true;
        }

        public void Hide() => Visible = false;
    }
}
