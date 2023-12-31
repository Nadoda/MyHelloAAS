﻿using BaSyx.Models.Core.AssetAdministrationShell.Generics;
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
using System.Net.Http;
using BaSyx.AAS.Client;
using System.Text.Json;
using BaSyx.AAS.Client.Http;
using BaSyx.Utils.Settings;
using BaSyx.Utils.Settings.Sections;
using System.Threading.Tasks;
using System.Threading;
using HelloAssetAdministrationShell.AASHTTPClient;
using System.ComponentModel.Design;

namespace HelloAssetAdministrationShell.MqttConnection
{
    
    public class MqttClientFunction
    {
        private readonly MqttClient client;
        
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string Speed { get; set; }
        public string TimeStamp { get; set; }
        public string MachineStatus { get; set; }   
       

        public static AssetAdministrationShellHttpClient AASHttpClient;
       
        private readonly StringBuilder csv;

        public event EventHandler<string> MyEvent;

        public MqttClientFunction(AssetAdministrationShellHttpClient aasclient)
        {
            
            // create client instance 
            client = new uPLibrary.Networking.M2Mqtt.MqttClient("test.mosquitto.org");

            // register to message received 
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic "MacnineData/ID-0000" with QoS 2 
            client.Subscribe(new string[] { "MacnineData/ID-0000" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            AASHttpClient = aasclient;

           
        }

        public void OnStatusReceived(String status)
        {
            MyEvent?.Invoke(this, status);
        }
      

        private async void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received 
            // Console.WriteLine("Received: " + System.Text.Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic);
            
            var message = System.Text.Encoding.UTF8.GetString(e.Message);
            Console.WriteLine(message.GetType());

            var jsonObject = JsonConvert.DeserializeObject<dynamic>(message);
            Console.WriteLine(jsonObject);

            Console.WriteLine("Temperature: " + jsonObject["Temperature"]);
            //Console.WriteLine("Speed: " + jsonObject["Speed"]);
            //Console.WriteLine(jsonObject["Timestamp"]);
            //Console.WriteLine(jsonObject["MachineStatus"]);

            var Temperature = jsonObject["Temperature"].ToString();
         // var temp = jsonObject["Temperature"].ToSting();
            var Humidity = jsonObject["Humidity"].ToString();
            var Speed = jsonObject["Speed"];
            var TimeStamp = jsonObject["Timestamp"];
            var MachineStatus = (string)jsonObject["MachineStatus"];
           
            Console.WriteLine("current Temperture is " + Temperature);
            //Console.WriteLine("current Machine Status is " + MachineStatus);



            var csv = new StringBuilder();
                var newLine = string.Format("Temperature : {0}, Humidity : {1}, Speed : {2}, TimeStamp : {3}, MachineStatus : {4}", Temperature, Humidity, Speed, TimeStamp, MachineStatus);
                csv.AppendLine(newLine);
                File.WriteAllText("data.csv", csv.ToString());
                string mes = JsonConvert.SerializeObject(Temperature);
                string Hum = JsonConvert.SerializeObject(Humidity);
                HttpClient client = new HttpClient();
            
            IValue TemperatureValue = new ElementValue((object)Temperature);
            IValue HumidityValue = new ElementValue((object)Humidity);
           
            try
            {
                // Submodel Element Value Updating Logic
                var TemperatureUpdate = AASHttpClient.UpdateSubmodelElementValue("OperationalDataSubmodel", "OperationalData/Temperature", TemperatureValue);
                var HumidityUpdate = AASHttpClient.UpdateSubmodelElementValue("OperationalDataSubmodel", "OperationalData/Humidity", HumidityValue);

                OnStatusReceived(MachineStatus);

                

            }
            catch
            {
                Console.WriteLine("Unable to connect");

            }
            await Task.Delay(1000);
         
        }
        



       




    }
}

