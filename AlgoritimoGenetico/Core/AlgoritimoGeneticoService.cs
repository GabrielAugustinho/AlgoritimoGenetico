using AlgoritimoGenetico.Extensions;
using Newtonsoft.Json;

namespace AlgoritimoGenetico.Core
{
    public class AlgoritimoGeneticoService : IAlgoritimoGeneticoService
    {
        private readonly Random Aleatorio = new();
        private readonly int QtdRainhas = 8;
        private int TamanhoPopulacao;
        private HashSet<int[]> Populacao = new();

        public void Run()
        {
            InicializarPopulacao();
            MostrarProbabilidadesDeSobrevivencia();
            // Implementar o restante da lógica do algoritmo genético aqui
            Console.ReadLine();
        }

        private void InicializarPopulacao()
        {
            CalcularTamanhoPopulacao();
            GerarPopulacaoInicial();
            MostrarPopulacaoInicial();
        }

        private void CalcularTamanhoPopulacao()
        {
            TamanhoPopulacao = 10;
        }

        private void GerarPopulacaoInicial()
        {
            Populacao = GerarConjuntoPopulacionalInicial();
        }

        private HashSet<int[]> GerarConjuntoPopulacionalInicial()
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
            return conjuntoPopulacionalInicial;
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

        private void MostrarPopulacaoInicial()
        {
            Console.WriteLine("População inicial:\n");
            foreach (var individuo in Populacao)
            {
                Console.WriteLine(JsonConvert.SerializeObject(individuo));
            }
        }

        private double[] CalcularProbabilidadesDeSobrevivencia()
        {
            double totalAptidao = 0;
            var aptidao = new double[Populacao.Count];
            int index = 0;

            foreach (var individuo in Populacao)
            {
                var colisoes = CalcularColisoes(individuo);
                var aptidaoIndividuo = colisoes != 0 ? 1.0 / colisoes : 9999.0;

                aptidao[index] = aptidaoIndividuo;
                totalAptidao += aptidaoIndividuo;
                index++;
            }

            for (int i = 0; i < aptidao.Length; i++)
            {
                aptidao[i] /= totalAptidao;
            }

            return aptidao;
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

        private void MostrarProbabilidadesDeSobrevivencia()
        {
            var aptidoesPopulacionais = CalcularProbabilidadesDeSobrevivencia();

            Console.WriteLine("\nProbabilidades de sobrevivência:\n");
            foreach (var aptidao in aptidoesPopulacionais)
            {
                Console.WriteLine(JsonConvert.SerializeObject(aptidao));
            }
        }
    }
}
