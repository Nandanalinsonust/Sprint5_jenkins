using HealthAxis.Admin.Services.Toast;

namespace HealthAxis.Admin.Services.Interfaces
{
    public interface IToastService
    {
        event Action<ToastMessage>? OnShow;

        void ShowToast(ToastMessage msg);

        void ShowSuccess(string title, string message);
        void ShowWarning(string title, string message);
        void ShowError(string title, string message);
        void ShowInfo(string title, string message);
    }
}