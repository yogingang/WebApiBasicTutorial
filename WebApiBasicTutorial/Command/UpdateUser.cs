using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApiBasicTutorial.Infrastructure;
using WebApiBasicTutorial.Interface.Contract;

namespace WebApiBasicTutorial.Command
{
    public class UpdateUser:IRequest<User>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class UpdateUserHandler : IRequestHandler<UpdateUser, User>
    {
        private readonly MyDbContext _dbContext;

        public UpdateUserHandler(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<User> Handle(UpdateUser request, CancellationToken cancellationToken)
        {
            var user = _dbContext.Users.Where(u => u.Id == request.Id).FirstOrDefault();
            if (user == null) throw new Exception($"Can not found user, id = {request.Id}");

            user.Name = request.Name;
            user.Email = request.Email;

            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();

            var findUser = _dbContext.Users.Where(u => u.Id == request.Id).FirstOrDefault();
            if (findUser == null) throw new Exception($"Can not found user after update user, id = {request.Id}");

            var response = new User
            {
                Id = findUser.Id,
                Email = findUser.Email,
                Name = findUser.Name,
            };

            return Task.FromResult(response);
        }
    }
}
