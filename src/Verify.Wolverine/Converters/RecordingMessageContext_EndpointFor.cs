namespace VerifyTests.Wolverine;

public partial class RecordingMessageContext
{
    public IDestinationEndpoint EndpointFor(string endpointName) =>
        new DestinationEndpoint(this, endpointName);

    public IDestinationEndpoint EndpointFor(Uri uri) =>
        new DestinationEndpoint(this, uri.ToString());
}