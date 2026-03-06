namespace MilitaryGeo.Application.DTOs.NguoiDung;

public record CreateNguoiDungDto
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Department { get; init; } = string.Empty;
    public string Position { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? Note { get; init; }
}

public record NguoiDungDto
{
    public int Id { get; init; }
    public string Username { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Department { get; init; } = string.Empty;
    public string Position { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? Note { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
    public Guid? CreatedBy { get; init; }
    public DateTimeOffset? ModifiedDate { get; init; }
    public Guid? ModifiedBy { get; init; }
}
