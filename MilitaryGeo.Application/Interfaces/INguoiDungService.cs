using MilitaryGeo.Application.DTOs.NguoiDung;

namespace MilitaryGeo.Application.Interfaces;

public interface INguoiDungService
{
    Task<IEnumerable<NguoiDungDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<NguoiDungDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<NguoiDungDto> CreateAsync(CreateNguoiDungDto dto, CancellationToken cancellationToken = default);
    Task<NguoiDungDto> UpdateAsync(int id, CreateNguoiDungDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
}
