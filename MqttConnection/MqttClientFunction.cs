using BaSyx.Models.Core.AssetAdministrationShell.Generics;
using BaSyx.Models.Core.AssetAdministrationShell.Identification.BaSyx;
using BaSyx.Models.Core.AssetAdministrationShell.Identification;
using BaSyx.Models.Core.AssetAdministrationShell;
using BaSyx.Models.Core.AssetAdministrationShell.Implementations;
using BaSyx.Models.Core.Common;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

using CsvHelper;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

using System.Collections.Generic;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace HelloAssetAdministrationShell.MqttConnection
{
    public class MqttClientFunction
    {
        private MqttClient client;


        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string Speed { get; set; }
        public string TimeStamp { get; set; }

        private StringBuilder csv;

        public MqttClientFunction()
        {
            // create client instance 
            client = new uPLibrary.Networking.M2Mqtt.MqttClient("test.mosquitto.org");

            // register to message received 
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic "/home/temperature" with QoS 2 
            client.Subscribe(new string[] { "MacnineData/ID-0000" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });


        }



        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received 
            // Console.WriteLine("Received: " + System.Text.Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic);

            var message = System.Text.Encoding.UTF8.GetString(e.Message);
            Console.WriteLine(message.GetType());

            var jsonObject = JsonConvert.DeserializeObject<dynamic>(message);
            Console.WriteLine(jsonObject);
            Console.WriteLine("Temperature: " + jsonObject["Temperature"]);
            Console.WriteLine("Speed: " + jsonObject["Speed"]);
            Console.WriteLine(jsonObject["Timestamp"]);


            var Temperature = jsonObject["Temperature"].ToString();
            var Humidity = jsonObject["Humidity"].ToString();
            var Speed = jsonObject["Speed"];
            var TimeStamp = jsonObject["Timestamp"];
            Console.WriteLine("current Temperture is " + Temperature);
            
            var csv = new StringBuilder();
            var newLine = string.Format("Temperature : {0}, Humidity : {1}, Speed : {2}, TimeStamp : {3}", Temperature, Humidity,Speed,TimeStamp);
            csv.AppendLine(newLine);
            File.WriteAllText("data.csv", csv.ToString());

        }


        

        //after your loop




        public string getMachineDataTimeStamp()
        {

            return TimeStamp;

        }
        public string getMachineDataSpeed() { return Speed; }

        public string getMachineDataHumidity() { return Humidity; }
        public string getMachineDataTemperature() { return Speed; }

    }
}

