using FluentValidation;

namespace Hotel_Management_API.DTOs.Validators
{
    public class CreateRoomRequestValidator : AbstractValidator<CreateRoomRequest>
    {
        public CreateRoomRequestValidator()
        {
            RuleFor(x => x.HotelId)
                .GreaterThan(0).WithMessage("HotelId must be greater than 0.");

            RuleFor(x => x.RoomTypeId)
                .GreaterThan(0).WithMessage("RoomTypeId must be greater than 0.");

            RuleFor(x => x.RoomNumber)
                .GreaterThan(0).WithMessage("Room number must be greater than 0.");

            RuleFor(x => x.FloorNumber)
                .GreaterThanOrEqualTo(0).WithMessage("Floor number must be greater than or equal to 0.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than 0.");

            RuleFor(x => x.PricePerNight)
                .GreaterThanOrEqualTo(0).WithMessage("Price per night must be greater than or equal to 0.");
        }
    }

    public class UpdateRoomRequestValidator : AbstractValidator<UpdateRoomRequest>
    {
        public UpdateRoomRequestValidator()
        {
            RuleFor(x => x.HotelId)
                .GreaterThan(0).WithMessage("HotelId must be greater than 0.");

            RuleFor(x => x.RoomTypeId)
                .GreaterThan(0).WithMessage("RoomTypeId must be greater than 0.");

            RuleFor(x => x.RoomNumber)
                .GreaterThan(0).WithMessage("Room number must be greater than 0.");

            RuleFor(x => x.FloorNumber)
                .GreaterThanOrEqualTo(0).WithMessage("Floor number must be greater than or equal to 0.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than 0.");

            RuleFor(x => x.PricePerNight)
                .GreaterThanOrEqualTo(0).WithMessage("Price per night must be greater than or equal to 0.");
        }
    }
}
