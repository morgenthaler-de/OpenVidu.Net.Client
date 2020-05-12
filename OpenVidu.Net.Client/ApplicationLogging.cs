using Microsoft.Extensions.Logging;

namespace OpenVidu.Net.Client
{
	public class ApplicationLogging
    {
        private static ILoggerFactory _factory;

        public static ILoggerFactory LoggerFactory
        {
            get => _factory ?? (_factory = new LoggerFactory());
            set => _factory = value;
        }

        public static ILogger createLogger(string category) => LoggerFactory.CreateLogger(category);
    }
}
