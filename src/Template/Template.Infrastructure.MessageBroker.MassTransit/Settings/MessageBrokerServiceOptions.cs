using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.MessageBroker.MassTransit.Settings
{
    public class MessageBrokerServiceOptions
    {
        public const string Key = "MessageBrokerService";

        public bool Enabled { get; set; }
        public string Host => $"amqp://{Hostname}:{Port}";
        public string Hostname { get; set; }
        public int? Port { get; set; } = 5672;
        public string Username { get; set; }
        public string Password { get; set; }
        public string Exchange { get; set; }
        public string Queue { get; set; }
    }
}
