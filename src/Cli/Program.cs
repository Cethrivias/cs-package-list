// See https://aka.ms/new-console-template for more information

using Cli.Commands;
using CliFx;
using Core;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddTransient<PackagesFinder>();
services.AddTransient<ListCommand>();

var serviceProvider = services.BuildServiceProvider();

await new CliApplicationBuilder()
    .AddCommandsFromThisAssembly()
    .UseTypeActivator(serviceProvider.GetService)
    .Build()
    .RunAsync(args);