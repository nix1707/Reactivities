using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Id);

            if (activity is null) return null;

            _context.Remove(activity);
            var result = await _context.SaveChangesAsync() > 0;
            
            if (result == false)
                return Result<Unit>.Failure("Failed to delete the activity");
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
