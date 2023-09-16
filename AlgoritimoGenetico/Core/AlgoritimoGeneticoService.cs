using System.Diagnostics;

namespace AlgoritimoGenetico.Core
{
    public class AlgoritimoGeneticoService : IAlgoritimoGeneticoService
    {
        // Parametros Geneticos
        private const int QtdRainhas = 8;
        private const int TamanhoPopulacao = 20;
        private const int NumeroMaximoDeGeracoes = 100;
        private const double TaxaDeCruzamento = 0.5;
        private const double TaxaDeMutacao = 0.1;
        private const int MaxGeracoesSemMelhora = 20;

        private List<int[]> Populacao;
        private double[] Fitness;

        private readonly Random Aleatorio = new();

        public AlgoritimoGeneticoService()
        {
            Populacao = new List<int[]>(TamanhoPopulacao);
            Fitness = new double[TamanhoPopulacao];
        }

        public void Run()
        {
            Console.WriteLine("Iniciando Algoritmo Genético...\n");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            GerarPopulacaoInicial();

            int geracaoAtual = 0;
            int geracoesSemMelhora = 0;
            int melhorGeracao = 0;
            int[] melhorIndividuo = Populacao[Array.IndexOf(Fitness, Fitness.Max())];

            while (geracaoAtual < NumeroMaximoDeGeracoes)
            {
                DiversificarPopulacao();

                var pai1 = SelecaoPorTorneio();
                var pai2 = SelecaoPorTorneio();

                Console.WriteLine($"Geração {geracaoAtual} - Pais para Cruzamento: {FormatarCromossomo(pai1)} + {FormatarCromossomo(pai2)}");

                var descendente = CruzamentoPMX(pai1, pai2);
                descendente = Mutacao(descendente);

                Console.WriteLine($"Filho após Cruzamento e Mutação: {FormatarCromossomo(descendente)}");

                SubstituirPiorIndividuo(descendente);

                geracaoAtual++;

                Console.WriteLine($"\n{geracaoAtual}ª Geração:");
                MostrarPopulacao();

                // Verifique se o melhor indivíduo mudou
                if (melhorIndividuo != Populacao[Array.IndexOf(Fitness, Fitness.Max())])
                {
                    geracoesSemMelhora = 0; // Resetar contagem
                    melhorIndividuo = Populacao[Array.IndexOf(Fitness, Fitness.Max())];
                    melhorGeracao = geracaoAtual;
                }
                else
                {
                    geracoesSemMelhora++;
                }

                Console.WriteLine($"Melhor indivíduo na geração {geracaoAtual}: {FormatarCromossomo(melhorIndividuo)}");
                Console.WriteLine($"Melhor Aptidão: {Fitness.Max()}");
                Console.WriteLine($"Pior Aptidão: {Fitness.Min()}");
                Console.WriteLine();

                if (geracoesSemMelhora >= MaxGeracoesSemMelhora && Fitness.Max() > 1)
                {
                    Console.WriteLine($"Convergência alcançada na {melhorGeracao}ª geração.");
                    break;
                }
            }

            stopwatch.Stop();
            TimeSpan tempoDecorrido = stopwatch.Elapsed;

            Console.WriteLine("\nAlgoritmo Genético Concluído.");
            Console.WriteLine($"Melhor resultado encontrado na {melhorGeracao}ª geração.");
            Console.WriteLine($"Melhor Indivíduo: {FormatarCromossomo(melhorIndividuo)}");
            Console.WriteLine($"Melhor Aptidão: {Fitness.Max()}");
            Console.WriteLine($"Tempo total de execução: {tempoDecorrido}");

            // Imprimir o tabuleiro com as posições das rainhas do melhor indivíduo
            ImprimirTabuleiro(melhorIndividuo);

            Console.ReadLine();
        }

        private void GerarPopulacaoInicial()
        {
            // Inicializando a população com cromossomos aleatórios
            for (int i = 0; i < TamanhoPopulacao; i++)
            {
                int[] cromossomo = GerarCromossomoAleatorio();
                Populacao.Add(cromossomo);
                Fitness[i] = CalcularAptidao(cromossomo);
            }

            // Encontrando o melhor indivíduo na população inicial
            var melhorIndividuo = Populacao[Array.IndexOf(Fitness, Fitness.Max())];

            Console.WriteLine("População Inicial:\n");
            MostrarPopulacao();
            Console.WriteLine($"Melhor indivíduo na população inicial: {FormatarCromossomo(melhorIndividuo)}");
            Console.WriteLine($"Aptidão do melhor indivíduo: {Fitness.Max()}");
            Console.WriteLine($"Aptidão do pior indivíduo: {Fitness.Min()}");
            Console.WriteLine();
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

        private int[] SelecaoPorTorneio()
        {
            int indiceIndividuo1 = Aleatorio.Next(0, TamanhoPopulacao);
            int indiceIndividuo2 = Aleatorio.Next(0, TamanhoPopulacao);

            double aptidao1 = Fitness[indiceIndividuo1];
            double aptidao2 = Fitness[indiceIndividuo2];

            // Retorne o indivíduo com a maior aptidão
            return aptidao1 > aptidao2 ? Populacao[indiceIndividuo1] : Populacao[indiceIndividuo2];
        }


        private int[] CruzamentoPMX(int[] pai1, int[] pai2)
        {
            int[] descendente = new int[QtdRainhas];

            int tamanhoFaixa = (int)(TaxaDeCruzamento * QtdRainhas);
            int pontoInicio = Aleatorio.Next(0, QtdRainhas - tamanhoFaixa);

            // Copie a faixa do pai1 para o descendente
            Array.Copy(pai1, pontoInicio, descendente, pontoInicio, tamanhoFaixa);

            // Crie um mapeamento de genes da faixa do pai2 para o pai1
            var mapeamento = new Dictionary<int, int>();
            for (int i = pontoInicio; i < pontoInicio + tamanhoFaixa; i++)
            {
                mapeamento[pai2[i]] = pai1[i];
            }

            // Preencha os valores ausentes no descendente usando o mapeamento
            for (int i = 0; i < QtdRainhas; i++)
            {
                if (i < pontoInicio || i >= pontoInicio + tamanhoFaixa)
                {
                    int gene = pai2[i];
                    while (mapeamento.ContainsKey(gene))
                    {
                        gene = mapeamento[gene];
                    }
                    descendente[i] = gene;
                }
            }

            return descendente;
        }

        private int[] Mutacao(int[] individuo)
        {
            individuo = MutacaoExistensial(individuo);
            individuo = MutacaoIndividual(individuo);

            return individuo;
        }

        private int[] MutacaoExistensial(int[] individuo)
        {
            // Crie uma lista dos números que já estão no cromossomo
            var numerosNoCromossomo = new List<int>(individuo);

            for (int rainha1 = 0; rainha1 < QtdRainhas; rainha1++)
            {
                // Se encontrarmos um número repetido no cromossomo
                if (numerosNoCromossomo.Count(n => n == individuo[rainha1]) > 1)
                {
                    int numeroRepetido = individuo[rainha1];

                    // Encontre o menor número que não esteja no cromossomo
                    int menorNumero = 0;
                    while (numerosNoCromossomo.Contains(menorNumero))
                    {
                        menorNumero++;
                    }

                    // Substitua o número repetido pelo menor número encontrado
                    individuo[rainha1] = menorNumero;

                    // Atualize a lista de números no cromossomo
                    numerosNoCromossomo.Remove(numeroRepetido);
                    numerosNoCromossomo.Add(menorNumero);
                }
            }

            return individuo;
        }

        private int[] MutacaoIndividual(int[] individuo)
        {
            // Verifica se a mutação individual deve ocorrer com base na taxa de mutação
            if (Aleatorio.NextDouble() <= TaxaDeMutacao)
            {
                int rainha1 = Aleatorio.Next(QtdRainhas);
                int rainha2 = Aleatorio.Next(QtdRainhas);

                // Certifique-se de que as duas rainhas selecionadas sejam diferentes
                while (rainha1 == rainha2)
                {
                    rainha2 = Aleatorio.Next(QtdRainhas);
                }

                // Realize a troca das duas rainhas
                int temp = individuo[rainha1];
                individuo[rainha1] = individuo[rainha2];
                individuo[rainha2] = temp;
            }

            return individuo;
        }


        private void SubstituirPiorIndividuo(int[] descendente)
        {
            double piorAptidao = 99999.0;
            int indicePiorIndividuo = -1;

            // Encontrar o índice do pior indivíduo na lista Fitness
            for (int i = 0; i < Fitness.Length; i++)
            {
                if (Fitness[i] < piorAptidao)
                {
                    piorAptidao = Fitness[i];
                    indicePiorIndividuo = i;
                }
            }

            // Calcular a aptidão do descendente
            var aptidaoDecendente = CalcularAptidao(descendente);
            Console.WriteLine($"\nPior indivíduo da família e descendente:\n" +
                              $"{FormatarCromossomo(Populacao[indicePiorIndividuo])} -> {FormatarCromossomo(descendente)}\n" +
                              $"Fitness: {piorAptidao} -> {aptidaoDecendente}");

            // Verificar se o descendente é melhor que o pior indivíduo e não é igual a nenhum indivíduo existente
            bool trocaRealizada = false;
            if (indicePiorIndividuo != -1 && aptidaoDecendente >= piorAptidao)
            {
                // Verificar se o descendente não é igual a nenhum indivíduo existente
                for (int i = 0; i < Populacao.Count; i++)
                {
                    if (Enumerable.SequenceEqual(Populacao[i], descendente))
                    {
                        Console.WriteLine("TROCA DESCONSIDERADA - Descendente igual a um existente na família.");
                        trocaRealizada = false;
                        break;
                    }
                    else
                    {
                        trocaRealizada = true;
                    }
                }

                if (trocaRealizada)
                {
                    Populacao[indicePiorIndividuo] = descendente;
                    Fitness[indicePiorIndividuo] = aptidaoDecendente;
                    Console.WriteLine("TROCA REALIZADA!!!");
                }
            }
            else
            {
                Console.WriteLine("TROCA DESCONSIDERADA - Descendente não é melhor que o pior indivíduo.");
            }
        }

        private void DiversificarPopulacao()
        {
            int numeroDeIndividuosAReiniciar = (int)(0.2 * TamanhoPopulacao); // Reinicialize 20% da população, por exemplo.

            for (int i = 0; i < numeroDeIndividuosAReiniciar; i++)
            {
                int indiceAleatorio = Aleatorio.Next(TamanhoPopulacao);
                int[] novoIndividuo = GerarCromossomoAleatorio();
                double novaAptidao = CalcularAptidao(novoIndividuo);

                // Verifique se a aptidão do novo indivíduo é melhor ou igual à aptidão do indivíduo existente
                if (novaAptidao >= Fitness[indiceAleatorio])
                {
                    Populacao[indiceAleatorio] = novoIndividuo; // Substitua o indivíduo existente pelo novo
                    Fitness[indiceAleatorio] = novaAptidao; // Atualize a aptidão
                }
            }
        }


        private double CalcularAptidao(int[] individuo)
        {
            int colisoes = CalcularColisoes(individuo);
            return colisoes != 0 ? 1.0 / colisoes : 99999.0;
        }

        private void MostrarPopulacao()
        {
            int individuoIndex = 1;
            foreach (var individuo in Populacao)
            {
                Console.WriteLine($"Indivíduo {individuoIndex++}: {FormatarCromossomo(individuo)} -> Fitness: {Fitness[Populacao.IndexOf(individuo)]}");
            }
            Console.WriteLine();
        }

        private string FormatarCromossomo(int[] cromossomo)
        {
            return "[" + string.Join(", ", cromossomo) + "]";
        }

        private void ImprimirTabuleiro(int[] individuo)
        {
            int tamanhoTabuleiro = individuo.Length;

            Console.WriteLine("Tabuleiro com as posições das rainhas:\n");

            for (int linha = 0; linha < tamanhoTabuleiro; linha++)
            {
                for (int coluna = 0; coluna < tamanhoTabuleiro; coluna++)
                {
                    if (individuo[linha] == coluna)
                    {
                        Console.Write($"{coluna} "); // Imprime o número da coluna
                    }
                    else
                    {
                        Console.Write(". "); // "." representa uma posição vazia
                    }
                }
                Console.WriteLine(); // Avança para a próxima linha do tabuleiro
            }

            Console.WriteLine();
        }

    }
}
