using AlgoritimoGenetico.Core;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped<IAlgoritimoGeneticoService, AlgoritimoGeneticoService>();
var serviceProvider = serviceCollection.BuildServiceProvider();
var solver = serviceProvider.GetRequiredService<IAlgoritimoGeneticoService>();

var tp = new int[] { 20, 50, 100 };
var ng = new int[] { 20, 80 };
var tc = new double[] { 0.8 };
var tm = new double[] { 0.0, 0.1 };
var ig = new double[] { 0.0, 0.1, 0.4 };

Console.WriteLine("Iniciando Algoritmo Genético...\n");


Console.WriteLine("TP NG TC TM IG Resultado -> Aptidão");
for (int i1 = 0; i1 < 3; i1++)
    for (int i2 = 0; i2 < 2; i2++)
        for (int i3 = 0; i3 < 1; i3++)
            for (int i4 = 0; i4 < 2; i4++)
                for (int i5 = 0; i5 < 3; i5++)
                    solver.Run(tp[i1], ng[i2], tc[i3], tm[i4], ig[i5]);

Console.ReadLine();