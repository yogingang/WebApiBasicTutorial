using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApiBasicTutorial.Command;
using WebApiBasicTutorial.Interface.Contract;
using WebApiBasicTutorial.Logger;

namespace WebApiBasicTutorial.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class UserController:ControllerBase
    {
        private readonly IApiLogger _logger;
        private readonly IMediator _mediator;

        public UserController(IApiLogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateUser(string id, string name, string email)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new AddUser() { Id = id, Name = name, Email = email };
            return Ok(await _mediator.Send(user));
        }

        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, string name, string email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new UpdateUser { Id = id, Name = name, Email = email };
            return Ok(await _mediator.Send(user));
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUser(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new RemoveUser { Id = id };
            await _mediator.Send(user);
            return Ok("Success");
        }

        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _mediator.Send(new GetUsers()));
        }

        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(new GetUser { Id = id }));
        }

    }
}
