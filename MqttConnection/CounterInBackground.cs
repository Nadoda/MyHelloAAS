using BaSyx.AAS.Client.Http;
using BaSyx.Models.Core.AssetAdministrationShell.Generics;
using BaSyx.Models.Core.AssetAdministrationShell.Implementations;
using BaSyx.Models.Core.Common;
using BaSyx.Utils.ResultHandling;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HelloAssetAdministrationShell.MqttConnection
{
    public class CounterInBackground : BackgroundService
    {
       
       
        public bool Status { get; set; }
        
        private static AssetAdministrationShellHttpClient client;
        public CounterInBackground()
        {
           //HelloAssetAdministrationShellService helloAssetAdministrationShellService = new HelloAssetAdministrationShellService();
           // client= new AssetAdministrationShellHttpClient(new Uri(helloAssetAdministrationShellService.ServerEndpoint));
        }
       

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    Console.WriteLine(Status);
                    for(int i=0; i < 100000000; i++)
                    {
                        Console.WriteLine(i);
                        await Task.Delay(5000);
                    };
                    
                    //IResult<IAssetAdministrationShell> result = client.RetrieveAssetAdministrationShell();
                    //var OperatingHourRetrieve = client.RetrieveSubmodelElementValue("MaintenanceSubmodel", "Maintenance_1/MaintenanceDetails/OperatingHours");
                    //var OperatingHour = Convert.ToInt32(OperatingHourRetrieve.Entity.Value);
                    //var IncrementOperatingHour = OperatingHour + 1;
                    //IValue OperatingHourValue = new ElementValue((object)IncrementOperatingHour);
                    //var OperatingHourUpdate = client.UpdateSubmodelElementValue("MaintenanceSubmodel", "Maintenance_1/MaintenanceDetails/OperatingHours", OperatingHourValue);
                    //Console.WriteLine("Task is running..." + Task.CompletedTask.Status);

                    //await Task.Delay(5000);

                    if (Status == true)
                    {
                        Console.WriteLine("MachineStaus is on");
                        //var OperatingHourRetrieve = client.RetrieveSubmodelElementValue("MaintenanceSubmodel", "Maintenance_1/MaintenanceDetails/OperatingHours");
                        //var OperatingHour = Convert.ToInt32(OperatingHourRetrieve.Entity.Value);
                        //var IncrementOperatingHour = OperatingHour + 1;
                        //IValue OperatingHourValue = new ElementValue((object)IncrementOperatingHour);
                        //var OperatingHourUpdate = client.UpdateSubmodelElementValue("MaintenanceSubmodel", "Maintenance_1/MaintenanceDetails/OperatingHours", OperatingHourValue);
                        //Console.WriteLine("Task is running..." + Task.CompletedTask.Status);

                        //await Task.Delay(5000);
                    }
                    else if(Status==false)
                    {
                        //Console.WriteLine("Can not Increment the Operating Hour because MachineStatus is off");
                        await Task.Delay(2000);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not Increment the Operating Hour because MachineStatus is off");
            }
            
        }

       
    }
}
    

