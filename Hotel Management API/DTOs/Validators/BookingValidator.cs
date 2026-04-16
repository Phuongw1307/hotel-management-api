using FluentValidation;

namespace Hotel_Management_API.DTOs.Validators
{
    public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
    {
        public CreateBookingRequestValidator()
        {
            RuleFor(x => x.HotelId)
                .GreaterThan(0);
            RuleFor(x => x.RoomId)
                .GreaterThan(0);

            RuleFor(x => x.CustomerName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.CustomerPhone)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.CustomerEmail)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(x => x.CheckInDate)
                .NotEmpty();

            RuleFor(x => x.CheckOutDate)
                .NotEmpty()
                .GreaterThan(x => x.CheckInDate)
                .WithMessage("Check-out date must be greater than check-in date.");
        }
    }
}
