using Serilog.Events;

namespace NewsWebSite.Models
{
    public class LogModel
    {
        public string Message { get; set; }
        public string Level { get; set; }
        public Exception Exception { get; set; }
        public string StatusCode { get; set; }
        public IReadOnlyDictionary<string, LogEventPropertyValue> Properties { get; set; }
    }
}
