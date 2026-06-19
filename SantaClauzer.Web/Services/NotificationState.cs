using System;

namespace SantaClauzer.Web.Services
{
    public sealed class NotificationState
    {
        private readonly object _sync = new();
        private PendingToast? _pending;

        // Event raised when a toast is added so layout can react immediately.
        public event Action<PendingToast>? ToastAdded;

        public void SetSuccess(string message, string title = "Success")
        {
            lock (_sync)
            {
                _pending = new PendingToast(NotificationType.Success, title, message);
            }
            Console.WriteLine($"NotificationState.SetSuccess called: {title} - {message}");
            ToastAdded?.Invoke(_pending!);
        }

        public void SetInfo(string message, string title = "Info")
        {
            lock (_sync)
            {
                _pending = new PendingToast(NotificationType.Info, title, message);
            }
            ToastAdded?.Invoke(_pending!);
        }

        public PendingToast? Consume()
        {
            lock (_sync)
            {
                var tmp = _pending;
                _pending = null;
                return tmp;
            }
        }
    }

    public sealed record PendingToast(NotificationType Type, string Title, string Message);

    public enum NotificationType
    {
        Success,
        Error,
        Info,
        Warning
    }
}