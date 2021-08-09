using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.ApplicationInsights.Channel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(edgedelta.function.Startup))]
namespace edgedelta.function
{
    public class ForkingTelemetryChannel : ITelemetryChannel
    {
        const string defaultEndpointAddress = @"https://dc.services.visualstudio.com/v2/track";
        const string originalConnectionStringName = "APPLICATIONINSIGHTS_CONNECTION_STRING";
        const string secondaryConnectionStringName = "APPLICATIONINSIGHTS_SECONDARY_CONNECTION_STRING";
        private string secondaryInstrumentationKey;
        private InMemoryChannel orginalChannel;
        private InMemoryChannel secondaryChannel;


        private static Dictionary<String, String> toDictionary(string connectionString){
            // connection strings are in the form of "key1=value1;key2=value2;key3=value3;"
            return connectionString
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select (part  => part.Split('=', StringSplitOptions.RemoveEmptyEntries))
                .Where (part => part.Length == 2)
                .ToDictionary (sp => sp[0], sp => sp[1]);
        }

        public static ForkingTelemetryChannel CreateFromEnvironment() {
            var secondaryConnectionString = Environment.GetEnvironmentVariable(secondaryConnectionStringName, EnvironmentVariableTarget.Process);
            if (secondaryConnectionString == "") {
                throw new ArgumentException("secondary connection string must exists");
            }

            var secondaryConnectionDictionary = toDictionary(secondaryConnectionString);
            var secondaryInstrumentationKey = secondaryConnectionDictionary["InstrumentationKey"];
            if (secondaryInstrumentationKey == "") {
                throw new ArgumentException("secondary connection string must have an instrumentation key");
            }

            var secondaryEndpointAddress = defaultEndpointAddress;
            var secondaryIngestionEndpoint = secondaryConnectionDictionary["IngestionEndpoint"];
            if (secondaryIngestionEndpoint != "") {
                Uri baseUri;
                if (!Uri.TryCreate(secondaryIngestionEndpoint, UriKind.Absolute, out baseUri)) {
                    throw new ArgumentException("absolute uri for secondary ingestion endpoint is not wellformed");
                }
                var uri = new Uri(baseUri, "/v2/track");
                secondaryEndpointAddress = uri.AbsoluteUri;
            }

            var originalEndpointAddress = defaultEndpointAddress;
            var originalConnectionString = Environment.GetEnvironmentVariable(originalConnectionStringName, EnvironmentVariableTarget.Process);
            if (!String.IsNullOrEmpty(originalConnectionString)) {
                var originalConnectionDictionary = toDictionary(originalConnectionString);
                var originalIngestionEndpoint = originalConnectionDictionary["IngestionEndpoint"];
                if (originalIngestionEndpoint != "") {
                    Uri baseUri;
                    if (!Uri.TryCreate(originalIngestionEndpoint, UriKind.Absolute, out baseUri)) {
                        throw new ArgumentException("absolute uri for original ingestion endpoint is not wellformed");
                    }
                    var uri = new Uri(baseUri, "/v2/track");
                    originalEndpointAddress = uri.AbsoluteUri;
                }
            }

            return new ForkingTelemetryChannel(secondaryInstrumentationKey, secondaryEndpointAddress, originalEndpointAddress);
        }

        public ForkingTelemetryChannel(string secondaryInstrumentationKey, string secondaryEndpointAddress, string originalEndpointAddress = defaultEndpointAddress)
        {
           orginalChannel = new InMemoryChannel() { EndpointAddress = originalEndpointAddress };
           secondaryChannel = new InMemoryChannel() { EndpointAddress = secondaryEndpointAddress };
           this.secondaryInstrumentationKey = secondaryInstrumentationKey;
        }

        public bool? DeveloperMode { get; set; }

        public string EndpointAddress {
            get {
                return orginalChannel.EndpointAddress;
            }
            set {
                orginalChannel.EndpointAddress = value;
            }
        }
        public string SecondaryEndpointAddress {
            get {
                return secondaryChannel.EndpointAddress;
            }
            set {
                secondaryChannel.EndpointAddress = value;
            }
        }

        public void Send(ITelemetry item)
        {
            var itemDup = item.DeepClone();
            itemDup.Context.InstrumentationKey = this.secondaryInstrumentationKey;
            Parallel.Invoke(
                () => { orginalChannel.Send(item); },
                () => { secondaryChannel.Send(itemDup); }
            );
        }

        public void Flush()
        {
            Parallel.Invoke(
                () => { orginalChannel.Flush(); },
                () => { secondaryChannel.Flush(); }
            );
        }

        public void Dispose()
        {
            Parallel.Invoke(
                () => { orginalChannel.Dispose(); },
                () => { secondaryChannel.Dispose(); }
            );
        }
    }

    public class Startup : FunctionsStartup
    {
        public override void  Configure(IFunctionsHostBuilder builder) {
            var channel = ForkingTelemetryChannel.CreateFromEnvironment();
            builder.Services.AddSingleton<ITelemetryChannel>(channel);
        }
    }
}
