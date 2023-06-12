using BaSyx.AAS.Client.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Net.Http;

namespace HelloAssetAdministrationShell.AASHTTPClient
{
    public class AASClient
    {

        public AssetAdministrationShellHttpClient client;
        public AASClient(AssetAdministrationShellHttpClient client)
        {
            this.client = client;
        }

        public dynamic GetOrdrerData()
        {
           
            var data =client.RetrieveSubmodelElementValue("MaintenanceSubmodel", "Maintenance_1/MaintenanceDetails/OperatingHours");
            return data ;
        }
    }
}
