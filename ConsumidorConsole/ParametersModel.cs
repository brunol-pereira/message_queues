using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumidorConsole
{
    public class ParametersModel
    {
        public ParametersModel()
        {
            BootstrapServer = "localhost:9092";
            TopicName = "ordens-topic";
            GroupId = "consumidor-group";
        }

        public string BootstrapServer { get; set; }
        public string TopicName { get; set; }
        public string GroupId { get; set; }
    }
}
