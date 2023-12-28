namespace NextDesk.App
{
    using NextDesk.Classes.Client.App;

    public partial class App : Application
    {
        private readonly ApplicationState state;

        public App(ApplicationState state)
        {
            this.state = state;
            InitializeComponent();
            MainPage = new MainPage();
        }
    }
}