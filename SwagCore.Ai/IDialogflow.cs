namespace SwagCore.Ai
{
    public interface IDialogflow
    {
        void Connect(string clientAccessToken);
        string SendMessage(string message);
    }
}