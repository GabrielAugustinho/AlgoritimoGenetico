using AlgoritimoGenetico.Extensions;

namespace AlgoritimoGenetico.Core
{
    public class AlgoritimoGeneticoService : IAlgoritimoGeneticoService
    {
        private readonly Random Aleatorio = new();
        private const int QtdRainhas = 8;
        private const int TamanhoPopulacao = 10;

        private HashSet<int[]> Populacao = new();
        private double[] Fitness;

        public AlgoritimoGeneticoService()
        {
            Fitness = new double[QtdRainhas];
        }

        public void Run()
        {
            GerarPopulacaoInicial();
            MostrarPopulacaoInicial();

            CalcularFitness();
            MostrarFitness();

            var pai1 = SelecaoPorTorneio();
            var pai2 = SelecaoPorTorneio();
            MostrarPaisEscolhidosPorTorneio(pai1, pai2);

            // Implementar o restante da lógica do algoritmo genético aqui
            Console.ReadLine();
        }

        private void GerarPopulacaoInicial()
        {
            var conjuntoPopulacionalInicial = new HashSet<int[]>(new ArrayComparer());
            for (int i = 0; i < TamanhoPopulacao; i++)
            {
                var cromossomo = GerarCromossomoAleatorio();
                while (!conjuntoPopulacionalInicial.Add(cromossomo))
                {
                    cromossomo = GerarCromossomoAleatorio();
                }
            }
            Populacao = conjuntoPopulacionalInicial;
        }

        private int[] GerarCromossomoAleatorio()
        {
            var cromossomo = Enumerable.Range(0, QtdRainhas).ToArray();
            EmbaralharArray(cromossomo);
            return cromossomo;
        }

        private void EmbaralharArray<T>(T[] array)
        {
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int j = Aleatorio.Next(i + 1);
                (array[j], array[i]) = (array[i], array[j]);
            }
        }

        private void CalcularFitness()
        {
            double totalAptidao = 0;
            var aptidao = new double[Populacao.Count];
            int index = 0;

            foreach (var individuo in Populacao)
            {
                var colisoes = CalcularColisoes(individuo);
                var aptidaoIndividuo = colisoes != 0 ? 1.0 / colisoes : 99999.0;

                aptidao[index] = aptidaoIndividuo;
                totalAptidao += aptidaoIndividuo;
                index++;
            }

            aptidao = aptidao.Select(a => a / totalAptidao).ToArray();

            Fitness = aptidao;
        }

        private int CalcularColisoes(int[] individuo)
        {
            int n = individuo.Length;
            int colisoes = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    int xDistancia = Math.Abs(j - i);
                    int yDistancia = Math.Abs(individuo[j] - individuo[i]);

                    if (xDistancia == yDistancia)
                    {
                        colisoes++;
                    }
                }
            }

            return colisoes;
        }

        private int SelecaoPorTorneio()
        {
            int individuo = Aleatorio.Next(0, TamanhoPopulacao);
            int competidor = Aleatorio.Next(0, TamanhoPopulacao);

            if (Fitness[individuo] > Fitness[competidor])
                return individuo;

            return competidor;
        }

        private void MostrarPopulacaoInicial()
        {
            Console.WriteLine("População Inicial:\n");
            int individuoIndex = 1;
            foreach (var individuo in Populacao)
            {
                Console.WriteLine($"Indivíduo {individuoIndex++}: {FormatarCromossomo(individuo)}");
            }
        }

        private void MostrarFitness()
        {
            Console.WriteLine("\nAptidão dos Indivíduos:\n");
            for (int i = 0; i < Populacao.Count; i++)
            {
                Console.WriteLine($"Indivíduo {i + 1}: Aptidão = {Fitness[i]}");
            }
        }

        private void MostrarPaisEscolhidosPorTorneio(int pai1, int pai2)
        {
            Console.WriteLine("\nPais Escolhidos pelo Método do Torneio:\n");
            Console.WriteLine($"Pai 1: {FormatarCromossomo(Populacao.ElementAt(pai1))} -> Aptidão: {Fitness[pai1]}");
            Console.WriteLine($"Pai 2: {FormatarCromossomo(Populacao.ElementAt(pai2))} -> Aptidão: {Fitness[pai2]}");
        }

        private string FormatarCromossomo(int[] cromossomo)
        {
            return "[" + string.Join(", ", cromossomo) + "]";
        }

    }
}
