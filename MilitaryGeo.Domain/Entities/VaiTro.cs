using MilitaryGeo.Domain.Base;

namespace MilitaryGeo.Domain.Entities;

public class VaiTro : EntityAuditBase<int>
{
    public string MaVaiTro { get; set; } = string.Empty;
    public string TenVaiTro { get; set; } = string.Empty;
    public string MoTa { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string Quyen { get; set; } = string.Empty; // JSON string ch?a các quy?n
    public int ThuTu { get; set; }
    public string? GhiChu { get; set; }
}
