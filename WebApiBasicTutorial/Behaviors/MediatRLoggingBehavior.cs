using MediatR;
using System.Diagnostics;
using System.Text.Json;
using WebApiBasicTutorial.Logger;

namespace WebApiBasicTutorial.Behaviors
{
    public class MediatRLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IApiLogger _logger;

        public MediatRLoggingBehavior(IApiLogger logger)
        {
            _logger = logger;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            string requestName = typeof(TRequest).Name;
            string uniqueId = Guid.NewGuid().ToString();
            _logger.LogInformation($"Begin Request Id:{uniqueId}, request name:{requestName},\nRequest={JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true })}");
            var timer = new Stopwatch();
            timer.Start();
            var response = next();
            timer.Stop();

            _logger.LogInformation($"End Request Id:{uniqueId}, request name:{requestName},\nResponse={JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true })}" +
                $"\ntotal elapsed time: {timer.ElapsedMilliseconds}");

            return response;
        }
    }
}
