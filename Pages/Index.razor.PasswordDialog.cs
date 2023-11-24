namespace NextDesk.App.Pages
{
    public partial class PasswordDialog
    {
        public bool Visible { get; set; }

        public bool Sent { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Error { get; set; } = string.Empty;

        public void Show()
        {
            Email = string.Empty;
            Visible = true;
        }
        public void Hide() => Visible = false;
    }
}