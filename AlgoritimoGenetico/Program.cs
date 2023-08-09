using AlgoritimoGenetico.Core;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped<IAlgoritimoGeneticoService, AlgoritimoGeneticoService>();
var serviceProvider = serviceCollection.BuildServiceProvider();
var solver = serviceProvider.GetRequiredService<IAlgoritimoGeneticoService>();

solver.Run();