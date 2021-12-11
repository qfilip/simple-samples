using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Hosting;

namespace MediatorPipeline.MediatorBase
{
    public class BaseRequestPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly string _logPath;
        public BaseRequestPostProcessor(IWebHostEnvironment environment)
        {
            _logPath = Path.Combine(environment.WebRootPath, "postprocessor_log.txt");
        }


        public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            var log = $"I've postprocessed stuff at {DateTime.UtcNow} \r\n";
            await File.AppendAllTextAsync(_logPath, log);
        }
    }
}
