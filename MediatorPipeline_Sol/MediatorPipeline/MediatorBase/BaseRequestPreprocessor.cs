using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Hosting;

namespace MediatorPipeline.MediatorBase
{
    public class BaseRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly string _logPath;
        public BaseRequestPreProcessor(IWebHostEnvironment environment)
        {
            _logPath = Path.Combine(environment.WebRootPath, "preprocessor_log.txt");
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var log = $"I've preprocessed stuff at {DateTime.UtcNow} \r\n";
            await File.AppendAllTextAsync(_logPath, log);
        }
    }
}
