namespace NextDesk.App.Pages.Places
{
    using NextDesk.DataTransferObject.Places;
    using NextDesk.MongoDataLibrary.Helpers.Org;

    public partial class DaysDialog
    {
        public PlaceDaySettingUpdate form = new();

        public bool Visible { get; set; }

        public void Show(DayConfiguration day)
        {
            if (day is null) throw new ArgumentNullException(nameof(day));

            form.SetData(day);
            Visible = true;
        }

        public void Hide() => Visible = false;
    }
}
