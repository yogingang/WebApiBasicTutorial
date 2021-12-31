using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApiBasicTutorial.Infrastructure;
using WebApiBasicTutorial.Interface.Contract;

namespace WebApiBasicTutorial.Command
{
    public class GetUser : IRequest<User>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class GetUserHandler : IRequestHandler<GetUser, User>
    {
        private readonly MyDbContext _dbContext;

        public GetUserHandler(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> Handle(GetUser request, CancellationToken cancellationToken)
        {

            var findUsers = await _dbContext.Users.Where(u => u.Id == request.Id)
                .Select(u => new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email
                })
                .ToListAsync();

            var findUser = findUsers.SingleOrDefault();

            if (findUser == null) throw new Exception($"Can not found user, user id = {request.Id}");

            return findUser;
        }


    }
}
