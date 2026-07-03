using HealthAxis.Admin.Services.Interfaces;
using HealthAxis.Admin.Services.Toast;

namespace HealthAxis.Admin.Services.Impl
{
    public class ToastService : IToastService
    {
        public event Action<ToastMessage>? OnShow;

        public void ShowToast(ToastMessage msg)
        {
            OnShow?.Invoke(msg);
        }

        public void ShowSuccess(string title, string message)
        {
            ShowToast(new ToastMessage
            {
                Title = title,
                Message = message,
                Type = ToastType.Success
            });
        }

        public void ShowWarning(string title, string message)
        {
            ShowToast(new ToastMessage
            {
                Title = title,
                Message = message,
                Type = ToastType.Warning
            });
        }

        public void ShowError(string title, string message)
        {
            ShowToast(new ToastMessage
            {
                Title = title,
                Message = message,
                Type = ToastType.Error
            });
        }

        public void ShowInfo(string title, string message)
        {
            ShowToast(new ToastMessage
            {
                Title = title,
                Message = message,
                Type = ToastType.Info
            });
        }
    }
}