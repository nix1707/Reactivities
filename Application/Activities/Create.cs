using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public Activity Activity { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
        }
    }

    public class Handler(DataContext context, IUserAccessor userAccessor)
        : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;
        private readonly IUserAccessor _userAccessor = userAccessor;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x =>
                x.UserName == _userAccessor.GetUsername());

            var attendee = new ActivityAttendee
            {
                AppUser = user,
                Activity = request.Activity,
                IsHost = true
            };

            request.Activity.Attendees.Add(attendee);

            _context.Activities.Add(request.Activity);

            //The integer value indicates the number of entities successfully saved to the database.
            var result = await _context.SaveChangesAsync() > 0;

            if (result == false) return Result<Unit>.Failure("Failed to create activity");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
