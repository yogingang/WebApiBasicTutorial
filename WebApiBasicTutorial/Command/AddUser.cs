using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApiBasicTutorial.Infrastructure;
using WebApiBasicTutorial.Interface.Contract;

namespace WebApiBasicTutorial.Command
{
    public class AddUser:IRequest<List<User>>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class AddUserHandler : IRequestHandler<AddUser, List<User>>
    {
        private readonly MyDbContext _dbContext;

        public AddUserHandler(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<User>> Handle(AddUser request, CancellationToken cancellationToken)
        {
            _dbContext.Users.Add(new Infrastructure.Models.User
            {
                Id = request.Id,
                Name = request.Name,
                Email = request.Email,
            });

            _dbContext.SaveChanges();

            var users = await _dbContext.Users.Select(u => new User()
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
            }).ToListAsync();

            return users;
        }
    }
}
