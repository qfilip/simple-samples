using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Hosting;

namespace MediatorPipeline.MediatorBase
{
    public class BaseExceptionAction<TRequest, TException> : IRequestExceptionAction<TRequest, TException>
        where TException : Exception
    {
        private readonly string _logPath;
        public BaseExceptionAction(IWebHostEnvironment environment)
        {
            _logPath = Path.Combine(environment.WebRootPath, "exception_log.txt");
        }

        public async Task Execute(TRequest request, TException exception, CancellationToken cancellationToken)
        {
            var log = $"Shit happend at {DateTime.UtcNow} \r\n";
            await File.AppendAllTextAsync(_logPath, log);
        }
    }
}
