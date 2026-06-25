public class ToastService
{
    public event Action<string>? OnSuccess;
    public event Action<string>? OnError;

    public void ShowSuccess(string msg) => OnSuccess?.Invoke(msg);
    public void ShowError(string msg) => OnError?.Invoke(msg);
}