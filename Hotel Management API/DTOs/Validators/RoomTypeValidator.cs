using FluentValidation;

namespace Hotel_Management_API.DTOs.Validators
{
    public class CreateRoomTypeRequestValidator : AbstractValidator<CreateRoomTypeRequest>
    {
        public CreateRoomTypeRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Room type name is required.")
                .MaximumLength(50).WithMessage("Room type name must not exceed 50 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(300).WithMessage("Description must not exceed 300 characters.");

            RuleFor(x => x.BasePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Base price must be greater than or equal to 0.");
        }
    }

    public class UpdateRoomTypeRequestValidator : AbstractValidator<UpdateRoomTypeRequest>
    {
        public UpdateRoomTypeRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Room type name is required.")
                .MaximumLength(50).WithMessage("Room type name must not exceed 50 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(300).WithMessage("Description must not exceed 300 characters.");

            RuleFor(x => x.BasePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Base price must be greater than or equal to 0.");
        }
    }
}
