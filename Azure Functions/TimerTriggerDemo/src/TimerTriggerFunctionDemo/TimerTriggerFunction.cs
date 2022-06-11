using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace TimerTriggerFunctionDemo
{
    public class TimerTriggerFunction
    {
        private readonly ILogger<TimerTriggerFunction> _logger;
        public TimerTriggerFunction(ILogger<TimerTriggerFunction> logger)
        {
            _logger = logger;
        }

        [FunctionName("TimerTriggerFunction")]
        public void Run([TimerTrigger("0/5 * * * * *")]TimerInfo timer)
        {
            if (timer.Schedule != default)
            {
                _logger.LogInformation("Last ran on: {0}", timer?.ScheduleStatus?.Last);
                _logger.LogInformation("Last run updated on: {0}", timer?.ScheduleStatus?.LastUpdated);
                _logger.LogInformation("Next run on: {0}", timer?.ScheduleStatus?.Next);
            }

            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
