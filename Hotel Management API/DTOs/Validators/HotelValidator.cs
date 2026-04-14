using FluentValidation;
using Hotel_Management_API.Dtos;

namespace Hotel_Management_API.DTOs.Validators
{
    public class CreateHotelRequestValidator : AbstractValidator<CreateHotelRequest>
    {
        public CreateHotelRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Hotel name is required.")
                .MaximumLength(100).WithMessage("Hotel name must not exceed 100 characters.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200).WithMessage("Address must not exceed 200 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .MaximumLength(20).WithMessage("Phone must not exceed 20 characters.");
        }
    }

    public class UpdateHotelRequestValidator : AbstractValidator<UpdateHotelRequest>
    {
        public UpdateHotelRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Hotel name is required.")
                .MaximumLength(100).WithMessage("Hotel name must not exceed 100 characters.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200).WithMessage("Address must not exceed 200 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .MaximumLength(20).WithMessage("Phone must not exceed 20 characters.");
        }
    }
}
