using MilitaryGeo.Application.Interfaces;
using System.Windows;

namespace MilitaryGeo.Desktop.Services;

/// <summary>
/// WPF implementation of IMessageService using MessageBox
/// </summary>
public class MessageService : IMessageService
{
    public void ShowInfo(string message, string title = "Thông báo")
    {
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public void ShowError(string message, string title = "Lỗi hệ thống")
    {
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public bool ShowConfirmation(string message, string title = "Xác nhận")
    {
        var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        return result == MessageBoxResult.Yes;
    }
}
