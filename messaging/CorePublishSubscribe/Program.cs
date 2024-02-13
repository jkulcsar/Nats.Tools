using CorePublishSubscribe;
using CorePublishSubscribe.Publisher;
using CorePublishSubscribe.Subscriber;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.Contracts;


var host = Host.CreateApplicationBuilder(args);

host.Services.AddCommunication();
host.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblyContaining<Program>();
});
host.Services.AddTtmHostedServices();
host.Services.AddSingleton<IPublisherService, SimplePublisherService>();
host.Services.AddSingleton<ISubscriberService, SimpleSubscriberService>();
host.Services.AddLogging(configure => configure.AddConsole());

host.Services.AddHostedService<ScenarioRunner>();

var app = host.Build();

await app.RunAsync();