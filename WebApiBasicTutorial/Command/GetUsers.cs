using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApiBasicTutorial.Infrastructure;
using WebApiBasicTutorial.Interface.Contract;

namespace WebApiBasicTutorial.Command
{
    public class GetUsers : IRequest<List<User>>
    {
    }

    public class GetUsersHandler : IRequestHandler<GetUsers, List<User>>
    {
        private readonly MyDbContext _dbContext;

        public GetUsersHandler(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> Handle(GetUsers request, CancellationToken cancellationToken)
        {
            var users = await _dbContext.Users.Select(u => new User
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            }).ToListAsync();

            return users;
        }


    }
}
