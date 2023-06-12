/*******************************************************************************
* Copyright (c) 2020, 2021 Robert Bosch GmbH
* Author: Constantin Ziesche (constantin.ziesche@bosch.com)
*
* This program and the accompanying materials are made available under the
* terms of the Eclipse Distribution License 1.0 which is available at
* https://www.eclipse.org/org/documents/edl-v10.html
*
* 
*******************************************************************************/
using BaSyx.AAS.Client.Http;
using BaSyx.AAS.Server.Http;
using BaSyx.API.Components;
using BaSyx.Common.UI;
using BaSyx.Common.UI.Swagger;
using BaSyx.Discovery.mDNS;
using BaSyx.Models.Extensions;
using BaSyx.Utils.Settings.Types;
using HelloAssetAdministrationShell.AASHTTPClient;
using HelloAssetAdministrationShell.MqttConnection;
using HelloAssetAdministrationShell.MqttConnection.I40MessageFormat;
using HelloAssetAdministrationShell.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Web;
using System;
using System.Linq;

namespace HelloAssetAdministrationShell
{
    class Program
    {
        //Create logger for the application
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public static readonly string ServerEndpoint = "http://192.168.2.186:5180 ";
        //public static readonly string ServerEndpoint = "http://141.44.237.199:5180";
        static void Main(string[] args)
        {
            logger.Info("Starting HelloAssetAdministrationShell's HTTP server...");

            

            //Loading server configurations settings from ServerSettings.xml;
            ServerSettings serverSettings = ServerSettings.LoadSettingsFromFile("ServerSettings.xml");

            //Initialize generic HTTP-REST interface passing previously loaded server configuration
            AssetAdministrationShellHttpServer server = new AssetAdministrationShellHttpServer(serverSettings);

            //Configure the entire application to use your own logger library (here: Nlog)
            server.WebHostBuilder.UseNLog();
            
            // server.WebHostBuilder.ConfigureServices(Services => { Services.AddHostedService<CounterInBackground>(); });
            // additional service Registration
           


            //Instantiate Asset Administration Shell Service
            HelloAssetAdministrationShellService shellService = new HelloAssetAdministrationShellService();

       //     server.WebHostBuilder.ConfigureServices(services => { services.AddHostedService<GetDataService>(); });
            //Dictate Asset Administration Shell service to use provided endpoints from the server configuration
            shellService.UseAutoEndpointRegistration(serverSettings.ServerConfig);

            //  WebHostBuilder webHostBuilder = new WebHostBuilder();
            Console.WriteLine(shellService.ServiceDescriptor.Endpoints);
          
            //Assign Asset Administration Shell Service to the generic HTTP-REST interface
            server.SetServiceProvider(shellService);
            //string clientUri = shellService.;
            //Console.WriteLine(clientUri);
            //AssetAdministrationShellHttpClient AASHttpClient = new AssetAdministrationShellHttpClient(new Uri(serverSettings));

            //Add Swagger documentation and UI
            server.AddSwagger(Interface.AssetAdministrationShell);

            //Add BaSyx Web UI
            server.AddBaSyxUI(PageNames.AssetAdministrationShellServer);
            server.AddBaSyxUI("DashBoard");

            
            AssetAdministrationShellHttpClient AASHttpClient =new AssetAdministrationShellHttpClient(new Uri(ServerEndpoint));
            
            MqttClientFunction cl = new MqttClientFunction(AASHttpClient);

           OperatingHour operatingHour = new OperatingHour(AASHttpClient);
            cl.MyEvent += operatingHour.EventHandlerMethod;

            //Action that gets executued when server is fully started 
            server.ApplicationStarted = () =>
            {
                //Use mDNS discovery mechanism in the network. It is used to register at the Registry automatically.
                shellService.StartDiscovery();
            };

            //Action that gets executed when server is shutting down
            server.ApplicationStopping = () =>
            {
                //Stop mDNS discovery thread
                shellService.StopDiscovery();
            };

            //Run HTTP server
            server.Run();
        }
    }
}