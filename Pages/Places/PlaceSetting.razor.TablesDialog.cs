namespace NextDesk.App.Pages.Places
{
    using NextDesk.DataTransferObject.Places;
    using NextDesk.MongoDataLibrary.Models.Org;

    public partial class TablesDialog
    {
        public PlaceTablesUpdate form = new();

        public bool Visible { get; set; }

        public void Show(BookingConfig config)
        {
            if (config is null) throw new ArgumentNullException(nameof(config));

            form.SetData(config);
            Visible = true;
        }

        public void Hide() => Visible = false;
    }
}
