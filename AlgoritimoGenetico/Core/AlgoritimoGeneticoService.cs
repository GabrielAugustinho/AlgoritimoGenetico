using AlgoritimoGenetico.Extensions;
using Newtonsoft.Json;

namespace AlgoritimoGenetico.Core
{
    public class AlgoritimoGeneticoService : IAlgoritimoGeneticoService
    {
        private readonly Random Random = new();
        private readonly int QtdRainhas = 8;
        private int TamanhoPopulacao;
        private HashSet<int[]> Populacao = new();

        public void Run()
        {
            IniciarProcesso();
            MostrarPopulacaoInicial();
            // Implementar o restante da lógica do algoritmo genético aqui
        }

        private void IniciarProcesso()
        {
            CalcularTamanhoPopulacao();
            CriarPopulacaoInicial();
        }

        private void CalcularTamanhoPopulacao()
        {
            // Verificar se o tamanho está ok
            // TamanhoPopulacao = Enumerable.Range(1, QtdRainhas).Aggregate(1, (p, item) => p * item) / 2;
            TamanhoPopulacao = 100;
        }

        private void CriarPopulacaoInicial()
        {
            Populacao = GerarPopulacaoInicial();
        }

        private HashSet<int[]> GerarPopulacaoInicial()
        {
            var populacaoInicial = new HashSet<int[]>(new ArrayComparer());
            for (int i = 0; i <= TamanhoPopulacao; i++)
            {
                var cromossomo = GerarCromossomoAleatorio();
                while (!populacaoInicial.Add(cromossomo))
                {
                    cromossomo = GerarCromossomoAleatorio();
                }
            }
            return populacaoInicial;
        }

        private int[] GerarCromossomoAleatorio()
        {
            var cromossomo = new int[QtdRainhas];
            for (int i = 0; i < QtdRainhas; i++)
            {
                cromossomo[i] = i;
            }
            return cromossomo.OrderBy(x => Random.Next()).ToArray(); // Embaralha
        }

        private void MostrarPopulacaoInicial()
        {
            Console.WriteLine("População inicial:\n");
            foreach (var item in Populacao)
            {
                Console.WriteLine(JsonConvert.SerializeObject(item));
            }
            Console.ReadLine();
        }

        // Implementar outras funções relacionadas ao algoritmo genético
    }
}
