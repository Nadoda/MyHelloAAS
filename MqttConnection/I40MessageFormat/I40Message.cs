using System.Collections.Generic;

namespace HelloAssetAdministrationShell.MqttConnection.I40MessageFormat
{
    public class I40Message
    {
        public Frame frame { get; set; }
        public List<object> interactionElements { get; set; }
    }
}

  
