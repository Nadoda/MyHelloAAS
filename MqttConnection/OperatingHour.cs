using BaSyx.AAS.Client.Http;
using BaSyx.Models.Core.AssetAdministrationShell.Implementations;
using BaSyx.Models.Core.Common;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace HelloAssetAdministrationShell.MqttConnection
{
    public class OperatingHour
    {

       
        public bool IsTaskRunning { get; private set;} = false;

        private static CancellationTokenSource cancellationTokenSource;

        private static AssetAdministrationShellHttpClient Aasclient;

        

        public OperatingHour(AssetAdministrationShellHttpClient aasclient) 
        { 
           
           Aasclient = aasclient;

           
        }

        public void EventHandlerMethod(object sender,string MachineStatus)
        {
           
            if (MachineStatus == "on" && IsTaskRunning == false)
            {
                StartTask();
                Console.WriteLine("Current Status.............." + IsTaskRunning);
            }
            else if (MachineStatus == "on" && IsTaskRunning == true)
            {
                Console.WriteLine("Task is Running & status is " + IsTaskRunning);
            }
            else if (MachineStatus == "off" && IsTaskRunning == true)
            {
                StopTask();
                Console.WriteLine("Current Status.............." + IsTaskRunning);
            }
        }
        public async void StartTask()
        {
            cancellationTokenSource = new CancellationTokenSource();
            IsTaskRunning = true;
           await IncrementAsync(cancellationTokenSource.Token);
        }
        public void StopTask()
        {
            cancellationTokenSource.Cancel();
            IsTaskRunning = false;
        }
        private static async Task IncrementAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try {
                    var OperatingHourRetrieve = Aasclient.RetrieveSubmodelElementValue("MaintenanceSubmodel", "Maintenance_1/MaintenanceDetails/OperatingHours");
                    var OperatingHour = Convert.ToInt32(OperatingHourRetrieve.Entity.Value);
                    var IncrementOperatingHour = OperatingHour + 1;
                    IValue OperatingHourValue = new ElementValue((object)IncrementOperatingHour);
                    var OperatingHourUpdate = Aasclient.UpdateSubmodelElementValue("MaintenanceSubmodel", "Maintenance_1/MaintenanceDetails/OperatingHours", OperatingHourValue);
                    await Task.Delay(1000);
                }
                catch (Exception e) 
                {
                    Console.WriteLine("The task is Running but there is an Exception occured, please handle it");
                }
                
            }
            
            // cancellation handling statement
            Console.WriteLine("Task is canceled.");
        }

    }
}















//private readonly MqttClient client;

////////////////In Constructor ////////////
//client = new uPLibrary.Networking.M2Mqtt.MqttClient("test.mosquitto.org");

//client.MqttMsgPublishReceived += MqttMsgPublishReceived;

//string clientId = Guid.NewGuid().ToString();
//client.Connect(clientId);

//client.Subscribe(new string[] { "MacnineData/ID-0000" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

///////////////////////////////////////////////////////////

//private void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
//{
//    var message = System.Text.Encoding.UTF8.GetString(e.Message);

//    var jsonObject = JsonConvert.DeserializeObject<dynamic>(message);
//    var MachineStatus = (string)jsonObject["MachineStatus"];

//    if (MachineStatus == "on" && IsTaskRunning == false)
//    {
//        StartTask();
//        Console.WriteLine("Current Status.............." + IsTaskRunning);
//    }
//    else if (MachineStatus == "on" && IsTaskRunning == true)
//    {
//        Console.WriteLine("Task is Running & status is " + IsTaskRunning);
//    }
//    else if (MachineStatus == "off" && IsTaskRunning == true)
//    {
//        StopTask();
//        Console.WriteLine("Current Status.............." + IsTaskRunning);
//    }
//}