using Cocona;
using CorePublishSubscribe;
using CorePublishSubscribe.Publisher;
using CorePublishSubscribe.Subscriber;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.Contracts;


var builder = CoconaApp.CreateBuilder();

 builder.Services.AddCommunication();
 builder.Services.AddTtmHostedServices();
 builder.Services.AddSingleton<IPublisherService, SimplePublisherService>();
 builder.Services.AddSingleton<ISubscriberService, SimpleSubscriberService>();
 builder.Services.AddLogging(configure => configure.AddConsole());

 var app = builder.Build();

app.AddCommands<RunCommands>();

app.RunAsync();


