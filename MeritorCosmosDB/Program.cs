using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;


namespace MeritorCosmosDB
{
    class Program
    {
        static string CosmosDBUrl = "https://meritorplants.documents.azure.com:443/";
        static string CosmosDBKey = "";
        static string CosmosDBName = "MOSPlantsSuppliers";
        static string CosmosDBColl = "MOSSupplierRejection";
        static void Main(string[] args)
        {
            GetCommunityKeyAsync().Wait();
            //CreateDBColl().Wait();
            //CreateMOSPlantDoc().Wait();
            CreateLocSupDoc().Wait();
            //CreateLocSupRejDoc().Wait();
            //ExecuteSimpleQuery();
            //GetPlantNames("NA");
            //GetAllPlantNames();
            //GetPartitionKeyRange().Wait();
        }


        protected static async Task GetCommunityKeyAsync()
        {
            try
            {
                var client = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(GetAccessTokenAsync),
                new HttpClient());

                var secret = await client.GetSecretAsync("https://meritorkeys.vault.azure.net/", "MeritorCosmosDBKey");

                CosmosDBKey = secret.Value;
            }
            catch(Exception ex)
            {

            }
        }


        private static async Task<string> GetAccessTokenAsync(string authority, string resource, string scope)
        {
            //DEMO ONLY
            //Storing ApplicationId and Key in code is bad idea :)
            var appCredentials = new ClientCredential("523ed7d4-1e4b-4ba2-b897-1256e80ad641", "ISL7H4/PkleAbWpamqYrKBkIJEx/kDQNS7wi+IG9ors=");
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);

            var result = await context.AcquireTokenAsync(resource, appCredentials);

            return result.AccessToken;
        }

        public async static Task CreateDBColl()
        {
            try
            {
                DocumentClient client = new DocumentClient(new Uri("https://meritorplants.documents.azure.com:443/"), "9siIU4dXJ8YIBeOWtqzYLwEeJjEs1m1KWFhKLrGfltnSOuxEhQzJfm78OhBX83ezxcyp0PIhob3qjqqZePu6GQ==");
                await client.CreateDatabaseIfNotExistsAsync(new Database { Id = "MOS" });

                DocumentCollection myCollection = new DocumentCollection();
                myCollection.Id = "MOS_Plants";
                myCollection.PartitionKey.Paths.Add("/DataType");
                //myCollection.UniqueKeyPolicy = new UniqueKeyPolicy
                //{
                //    UniqueKeys =
                //    new Collection<UniqueKey>
                //    {
                //    new UniqueKey { Paths = new Collection<string> { "/PlantName" } }
                //    }
                //};
                await client.CreateDocumentCollectionIfNotExistsAsync(
                    UriFactory.CreateDatabaseUri("MOS"),
                    myCollection,
                    new RequestOptions { OfferThroughput = 500 });
            }
            catch(Exception ex)
            {

            }
        }

        public async static Task CreateMOSPlantDoc()
        {
            try
            {
                DocumentClient client = new DocumentClient(new Uri(CosmosDBUrl), CosmosDBKey);

                MOS_Supplier_Plant MosPlantDoc = new MOS_Supplier_Plant()
                {
                    PlantName = "Huayang China (Brakes)",
                    DataType = "Plant",
                    region = "AP",
                    BusinessGroup = "Divested Sites"
                };
                Uri collUri = UriFactory.CreateDocumentCollectionUri(CosmosDBName, CosmosDBColl);
                //var options = new RequestOptions { PreTriggerInclude = new[] { "ValidatePlantName" } };
                ResourceResponse<Document> result = await client.CreateDocumentAsync(collUri, MosPlantDoc,new RequestOptions { PreTriggerInclude = new List<string> { "ValidatePlant" } });
                var document = result.Resource;
                
                Console.WriteLine($"Created Jones document with ID: {document.Id}");
                Console.WriteLine($"Created Jones document with RequestCharge: {result.RequestCharge}");
                Console.WriteLine($"Created Jones document with TriggersUsage: {result.TriggersUsage}");
                Console.WriteLine($"Created Jones document with RequestLatency: {result.RequestLatency}");
                Console.WriteLine($"Created Jones document with CollectionSizeUsage: {result.CollectionSizeUsage}");
                Console.Read();
            }
            catch (Exception ex)
            {

            }
        }

        public async static Task CreateLocSupDoc()
        {
            try
            {
                
                DocumentClient client = new DocumentClient(new Uri(CosmosDBUrl), CosmosDBKey);
                MOS_Supplier_Detail Loc_Supp_Doc = new MOS_Supplier_Detail()
                {
                    SupplierName = "A.MAURI (Italy)",
                    PlantName = "Huang (Brakes)",
                    region = "AP",
                    DataType = "Supplier",
                    DUNSNumber= "428668597"
                };
                Uri collUri = UriFactory.CreateDocumentCollectionUri(CosmosDBName, CosmosDBColl);
                //var options = new RequestOptions { PreTriggerInclude = new[] { "ValidateSupplierName" } };
                ResourceResponse<Document> result = await client.CreateDocumentAsync(collUri, Loc_Supp_Doc);
                var document = result.Resource;
                
                Console.WriteLine($"Created Jones document with ID: {document.Id}");
                Console.WriteLine($"Created Jones document with RequestCharge: {result.RequestCharge}");
                Console.WriteLine($"Created Jones document with RequestLatency: {result.RequestLatency}");
                Console.WriteLine($"Created Jones document with CollectionSizeUsage: {result.CollectionSizeUsage}");
                Console.Read();
            }
            catch (Exception ex)
            {

            }
        }

        public async static Task CreateLocSupRejDoc()
        {
            try
            {

                DocumentClient client = new DocumentClient(new Uri(CosmosDBUrl), CosmosDBKey);

                MOS_Supplier_Rejection Loc_Supp_Doc = new MOS_Supplier_Rejection()
                {
                    SupplierName = "A.MAURI (Italy)",
                    Date = DateTime.Now,
                    region = "EU",
                    PartNumber = "2132B",
                    Reason = "Doors latches functions",
                    DataType = "SupplierRejection"
                };
                Uri collUri = UriFactory.CreateDocumentCollectionUri(CosmosDBName, CosmosDBColl);
                ResourceResponse<Document> result = await client.CreateDocumentAsync(collUri, Loc_Supp_Doc);
                var document = result.Resource;

                Console.WriteLine($"Created Jones document with ID: {document.Id}");
                Console.WriteLine($"Created Jones document with RequestCharge: {result.RequestCharge}");
                Console.WriteLine($"Created Jones document with RequestLatency {result.RequestLatency}");
                Console.WriteLine($"Created Jones document with CollectionSizeUsage: {result.CollectionSizeUsage}");
                Console.Read();
            }
            catch (Exception ex)
            {

            }
        }

        private static void ExecuteSimpleQuery()
        {
            try
            {
                DocumentClient client = new DocumentClient(new Uri("https://meritorplants.documents.azure.com:443/"), "mJ1hHsHH9P84Wjw6SBEquFOui7YJgWLuldrujX34OwOQ5MfvIXmW3RsmKLPJKntLD4wDSNxkcua6xrKEI4MUTA==");
                
                

                FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };


                IQueryable<MOS_Local_Supplier> familyQuery = client.CreateDocumentQuery<MOS_Local_Supplier>(
                        UriFactory.CreateDocumentCollectionUri("MOSPlantsSuppliers", "SupplierRejection"), queryOptions)
                        .Where(f => f.PlantName == "Asheville (Fletcher), NC2");


                Console.WriteLine("Running LINQ query...");
                foreach (MOS_Local_Supplier family in familyQuery)
                {
                    Console.WriteLine("\tRead {0}", family.PlantName);
                }


                IQueryable<MOS_Local_Supplier> familyQueryInSql = client.CreateDocumentQuery<MOS_Local_Supplier>(
                        UriFactory.CreateDocumentCollectionUri("MOSPlantsSuppliers", "SupplierRejection"),
                        "SELECT DISTINCT(MOS_Local_Supplier.DUNSNumber) FROM MOS_Local_Supplier WHERE MOS_Local_Supplier.PlantName = 'Asheville (Fletcher), NC2'",
                        queryOptions);

                Console.WriteLine("Running direct SQL query...");
                foreach (MOS_Local_Supplier family in familyQueryInSql)
                {
                    Console.WriteLine("\tRead {0}", family.DUNSNumber);
                }

                Console.WriteLine("Press any key to continue ...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {

            }
        }

        private static void GetPlantNames(string region)
        {
            try
            {
                ConnectionPolicy policy = new ConnectionPolicy
                {
                    // Note: These aren't required settings for multi-homing,
                    // just suggested defaults
                    ConnectionMode = ConnectionMode.Direct,                    
                    ConnectionProtocol = Protocol.Tcp
                };

                policy.PreferredLocations.Add("Central India");
                DocumentClient client = new DocumentClient(new Uri(CosmosDBUrl), CosmosDBKey, policy);
                
                FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, PartitionKey = new PartitionKey(region) };

                IEnumerable<MOS_Supplier_Plant> familyQuery = client.CreateDocumentQuery<MOS_Supplier_Plant>(
                            UriFactory.CreateDocumentCollectionUri(CosmosDBName, CosmosDBColl), queryOptions)
                            .Where(f => f.DataType == "Plant");


                Console.WriteLine("Running LINQ query... " + client.Session);
                foreach (MOS_Supplier_Plant family in familyQuery)
                {
                    Console.WriteLine("\tPlant: {0}", family.PlantName);
                }
                Console.WriteLine(client.Session);
                Console.ReadKey();
            }
            catch (Exception ex)
            {

            }
        }

        private static void GetAllPlantNames()
        {
            try
            {
                ConnectionPolicy policy = new ConnectionPolicy
                {
                    // Note: These aren't required settings for multi-homing,
                    // just suggested defaults
                    ConnectionMode = ConnectionMode.Direct,
                    ConnectionProtocol = Protocol.Tcp
                };

                policy.PreferredLocations.Add("Central India");
                DocumentClient client = new DocumentClient(new Uri(CosmosDBUrl), CosmosDBKey, policy);

                FeedOptions queryOptions = new FeedOptions { EnableCrossPartitionQuery = true, MaxDegreeOfParallelism = 3 };

                var familyQuery = client.CreateDocumentQuery<MOS_Supplier_Plant>(
                            UriFactory.CreateDocumentCollectionUri(CosmosDBName, CosmosDBColl), queryOptions)
                            .Where(f => f.DataType == "Plant").Select(s=>s.PlantName).Distinct();


                Console.WriteLine("Running LINQ query... " + client.Session);
                foreach (var family in familyQuery)
                {
                    Console.WriteLine("\tPlant: {0}", family);
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {

            }
        }


        private async static Task GetPartitionKeyRange()
        {
            try
            {
                ConnectionPolicy policy = new ConnectionPolicy
                {
                    // Note: These aren't required settings for multi-homing,
                    // just suggested defaults
                    ConnectionMode = ConnectionMode.Direct,
                    ConnectionProtocol = Protocol.Tcp
                };

                policy.PreferredLocations.Add("Central India");
                DocumentClient client = new DocumentClient(new Uri(CosmosDBUrl), CosmosDBKey, policy);
                DocumentCollection collection = await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(CosmosDBName, CosmosDBColl)
    ,
    new RequestOptions { PopulatePartitionKeyRangeStatistics = true });

                foreach (var partitionKeyRangeStatistics in collection.PartitionKeyRangeStatistics)
                {
                    foreach (var partitionKeyStatistics in partitionKeyRangeStatistics.PartitionKeyStatistics)
                    {
                        Console.WriteLine(partitionKeyStatistics.PartitionKey.ToString());
                    }
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
