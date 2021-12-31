using MediatR;
using WebApiBasicTutorial.Infrastructure;

namespace WebApiBasicTutorial.Command
{
    public class RemoveUser : IRequest
    {
        public string Id { get; set; }
    }

    public class RemoveUserHandler : AsyncRequestHandler<RemoveUser>
    {
        private readonly MyDbContext _dbContext;

        public RemoveUserHandler(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        protected override Task Handle(RemoveUser request, CancellationToken cancellationToken)
        {
            var user = _dbContext.Users.Where(u => u.Id == request.Id).FirstOrDefault();

            if (user == null) throw new Exception($"Can not found user, id = {request.Id}");

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
