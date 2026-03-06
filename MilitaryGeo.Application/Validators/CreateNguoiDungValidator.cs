using FluentValidation;
using MilitaryGeo.Application.DTOs.NguoiDung;

namespace MilitaryGeo.Application.Validators;

public class CreateNguoiDungValidator : AbstractValidator<CreateNguoiDungDto>
{
    public CreateNguoiDungValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Vui lòng nh?p tên ??ng nh?p!")
            .MinimumLength(3).WithMessage("Tên ??ng nh?p ph?i có ít nh?t 3 ký t?!")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Tên ??ng nh?p ch? ch?a ch? cái, s? và d?u g?ch d??i!");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Vui lòng nh?p m?t kh?u!")
            .MinimumLength(6).WithMessage("M?t kh?u ph?i có ít nh?t 6 ký t?!");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Vui lòng nh?p h? và tên!");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Vui lòng nh?p email!")
            .EmailAddress().WithMessage("Email không ?úng ??nh d?ng!");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Vui lòng nh?p s? ?i?n tho?i!")
            .Matches(@"^0\d{9}$").WithMessage("S? ?i?n tho?i ph?i có 10 s? và b?t ??u b?ng 0!");

        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("Vui lòng ch?n ??n v?!");

        RuleFor(x => x.Position)
            .NotEmpty().WithMessage("Vui lòng nh?p ch?c v?!");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Vui lòng ch?n vai trò!")
            .Must(role => new[] { "Admin", "Manager", "User" }.Contains(role))
            .WithMessage("Vai trò không h?p l?!");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Vui lòng ch?n tr?ng thái!")
            .Must(status => new[] { "Ho?t ??ng", "Khóa" }.Contains(status))
            .WithMessage("Tr?ng thái không h?p l?!");
    }
}
