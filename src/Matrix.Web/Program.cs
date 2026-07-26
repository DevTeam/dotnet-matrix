var builder = WebAssemblyHostBuilder.CreateDefault(args);

var httpClient = new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
};
var composition = new Composition(httpClient);
builder.ConfigureContainer(composition);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(_ => httpClient);

await builder.Build().RunAsync();
