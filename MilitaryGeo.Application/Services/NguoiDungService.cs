using FluentValidation;
using MilitaryGeo.Application.DTOs.NguoiDung;
using MilitaryGeo.Application.Interfaces;
using MilitaryGeo.Application.Validators;
using MilitaryGeo.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace MilitaryGeo.Application.Services;

public class NguoiDungService : INguoiDungService
{
    private readonly CreateNguoiDungValidator _validator;
    private static readonly List<NguoiDung> _mockData = new();
    private static int _nextId = 1;
    private static readonly Guid SystemUserId = Guid.NewGuid();

    public NguoiDungService()
    {
        _validator = new CreateNguoiDungValidator();
        InitializeMockData();
    }

    private void InitializeMockData()
    {
        if (_mockData.Count == 0)
        {
            _mockData.AddRange(new[]
            {
                new NguoiDung
                {
                    Id = _nextId++,
                    Username = "admin",
                    PasswordHash = HashPassword("123456"),
                    FullName = "Nguyễn Văn Quân",
                    Email = "admin@militarygeo.vn",
                    Phone = "0123456789",
                    Department = "Ban Chỉ huy",
                    Position = "Quản trị viên hệ thống",
                    Role = "Admin",
                    Status = "Hoạt động",
                    CreatedDate = DateTimeOffset.Now.AddMonths(-12),
                    CreatedBy = SystemUserId
                },
                new NguoiDung
                {
                    Id = _nextId++,
                    Username = "user01",
                    PasswordHash = HashPassword("123456"),
                    FullName = "Trần Văn An",
                    Email = "tvana@militarygeo.vn",
                    Phone = "0987654321",
                    Department = "Phòng Tác chiến",
                    Position = "Sĩ quan tác chiến",
                    Role = "User",
                    Status = "Hoạt động",
                    CreatedDate = DateTimeOffset.Now.AddMonths(-10),
                    CreatedBy = SystemUserId
                }
            });
        }
    }

    public async Task<IEnumerable<NguoiDungDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);
        return _mockData.Select(MapToDto);
    }

    public async Task<NguoiDungDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await Task.Delay(50, cancellationToken);
        var entity = _mockData.FirstOrDefault(x => x.Id == id);
        return entity != null ? MapToDto(entity) : null;
    }

    public async Task<NguoiDungDto> CreateAsync(CreateNguoiDungDto dto, CancellationToken cancellationToken = default)
    {
        // Validate
        var validationResult = await _validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Check duplicates
        if (await UsernameExistsAsync(dto.Username, cancellationToken))
        {
            throw new InvalidOperationException("Tên đăng nhập đã tồn tại!");
        }

        if (await EmailExistsAsync(dto.Email, cancellationToken))
        {
            throw new InvalidOperationException("Email đã được sử dụng!");
        }

        // Create entity
        var currentUserId = Guid.NewGuid(); // TODO: Get from current user context
        var entity = new NguoiDung
        {
            Id = _nextId++,
            Username = dto.Username,
            PasswordHash = HashPassword(dto.Password),
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            Department = dto.Department,
            Position = dto.Position,
            Role = dto.Role,
            Status = dto.Status,
            Note = dto.Note,
            CreatedDate = DateTimeOffset.Now,
            CreatedBy = currentUserId
        };

        _mockData.Add(entity);
        await Task.Delay(200, cancellationToken);

        return MapToDto(entity);
    }

    public async Task<NguoiDungDto> UpdateAsync(int id, CreateNguoiDungDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mockData.FirstOrDefault(x => x.Id == id);
        if (entity == null)
            throw new KeyNotFoundException($"Không tìm thấy người dùng với ID: {id}");

        // Validate
        var validationResult = await _validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Update
        var currentUserId = Guid.NewGuid(); // TODO: Get from current user context
        entity.FullName = dto.FullName;
        entity.Email = dto.Email;
        entity.Phone = dto.Phone;
        entity.Department = dto.Department;
        entity.Position = dto.Position;
        entity.Role = dto.Role;
        entity.Status = dto.Status;
        entity.Note = dto.Note;
        entity.ModifiedDate = DateTimeOffset.Now;
        entity.ModifiedBy = currentUserId;

        await Task.Delay(200, cancellationToken);
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = _mockData.FirstOrDefault(x => x.Id == id);
        if (entity == null)
            return false;

        _mockData.Remove(entity);
        await Task.Delay(100, cancellationToken);
        return true;
    }

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        await Task.Delay(50, cancellationToken);
        return _mockData.Any(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        await Task.Delay(50, cancellationToken);
        return _mockData.Any(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    private static NguoiDungDto MapToDto(NguoiDung entity) => new()
    {
        Id = entity.Id,
        Username = entity.Username,
        FullName = entity.FullName,
        Email = entity.Email,
        Phone = entity.Phone,
        Department = entity.Department,
        Position = entity.Position,
        Role = entity.Role,
        Status = entity.Status,
        Note = entity.Note,
        CreatedDate = entity.CreatedDate,
        CreatedBy = entity.CreatedBy,
        ModifiedDate = entity.ModifiedDate,
        ModifiedBy = entity.ModifiedBy
    };

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}