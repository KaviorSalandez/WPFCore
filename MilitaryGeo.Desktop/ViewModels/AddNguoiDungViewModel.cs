using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using MilitaryGeo.Application.DTOs.NguoiDung;
using MilitaryGeo.Application.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace MilitaryGeo.Desktop.ViewModels;

public partial class AddNguoiDungViewModel : ObservableObject
{
    private readonly INguoiDungService _nguoiDungService;
    private readonly IMessageService _messageService; 

    // Observables
    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _fullName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _phone = string.Empty;

    [ObservableProperty]
    private string? _selectedDepartment;

    [ObservableProperty]
    private string _position = string.Empty;

    [ObservableProperty]
    private string? _selectedRole = "User";

    [ObservableProperty]
    private string? _selectedStatus = "Ho?t ??ng";

    [ObservableProperty]
    private string? _note;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string? _errorMessage;

    // Collections for ComboBoxes
    public ObservableCollection<string> Departments { get; } = new()
    {
        "Ban Chỉ huy",
        "Phòng Tác chiến",
        "Phòng Kế hoạch",
        "Phòng Hậu cần",
        "Phòng Kỹ thuật",
        "Phòng Hành chính",
        "Phòng Tài chính"
    };

    public ObservableCollection<string> Roles { get; } = new()
    {
        "Admin",
        "Manager",
        "User"
    };

    public ObservableCollection<string> Statuses { get; } = new()
    {
        "Hoạt động",
        "Khóa"
    };

    // Result
    public bool IsSuccess { get; private set; }
    public NguoiDungDto? CreatedUser { get; private set; }

    public AddNguoiDungViewModel(INguoiDungService nguoiDungService, IMessageService messageService)
    {
        _nguoiDungService = nguoiDungService ?? throw new ArgumentNullException(nameof(nguoiDungService));
        _messageService = messageService;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            // Create DTO
            var dto = new CreateNguoiDungDto
            {
                Username = Username.Trim(),
                Password = Password,
                FullName = FullName.Trim(),
                Email = Email.Trim(),
                Phone = Phone.Trim(),
                Department = SelectedDepartment ?? string.Empty,
                Position = Position.Trim(),
                Role = SelectedRole ?? "User",
                Status = SelectedStatus ?? "Hoạt động",
                Note = Note
            };

            // Call service (validation will be done in service)
            CreatedUser = await _nguoiDungService.CreateAsync(dto);
            
            IsSuccess = true;

            _messageService.ShowInfo($"Thêm người dùng '{CreatedUser.FullName}' thành công!", "Thành công");

            // Close window will be handled by view
            OnCloseRequested();
        }
        catch (ValidationException vex)
        {
            var errors = string.Join("\n", vex.Errors.Select(e => $"• {e.ErrorMessage}"));
            ErrorMessage = $"Lỗi nhập liệu:\n{errors}";
            _messageService.ShowError(ErrorMessage, "Lỗi nhập liệu");
        }
        catch (InvalidOperationException iex)
        {
            ErrorMessage = iex.Message;
            _messageService.ShowError(ErrorMessage, "Lỗi");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
            _messageService.ShowError(ErrorMessage, "Lỗi hệ thống");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        var confirmed = _messageService.ShowConfirmation(
            "Bạn có chắc chắn muốn hủy? Dữ liệu đã nhập sẽ không được lưu.",
            "Xác nhận hủy");

        if (confirmed)
        {
            OnCloseRequested();
        }
    }

    // Event for closing window
    public event EventHandler? CloseRequested;

    private void OnCloseRequested()
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
