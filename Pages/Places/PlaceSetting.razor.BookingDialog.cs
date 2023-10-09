namespace NextDesk.App.Pages.Places
{
    using NextDesk.DataTransferObject.Bookings;

    public partial class BookingDialog
    {
        public BookingCreate form = new();

        public bool Visible { get; set; }

        public void Show(DayItemConfiguration config)
        {
            form.SetData(config);
        }
    }
}
