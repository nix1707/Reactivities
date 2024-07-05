using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Bio { get; set; }
            public string DisplayName { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
            }
        }

        public class Handler(DataContext context, IUserAccessor userAccessor)
            : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context = context;
            private readonly IUserAccessor _userAccessor = userAccessor;

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                user.Bio = request.Bio ?? user.Bio;
                user.DisplayName = request.DisplayName ?? user.DisplayName;

                _context.Entry(user).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync() > 0;  

                if (result == true) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Failed to update profile");
            }
        }
    }
}