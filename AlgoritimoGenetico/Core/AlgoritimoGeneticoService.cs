namespace AlgoritimoGenetico.Core
{
    public class AlgoritimoGeneticoService : IAlgoritimoGeneticoService
    {
        // Parametros Geneticos
        private const int QtdRainhas = 8;
        private const int TamanhoPopulacao = 10;
        private const int NumeroMaximoDeGeracoes = 10;
        private const double TaxaDeCruzamento = 0.8;
        private const double TaxaDeMutacao = 0.1;
        private const int MaxGeracoesSemMelhora = 10;

        private List<int[]> Populacao;
        private List<double> FitnessIndices;
        private readonly Random Aleatorio = new();

        public AlgoritimoGeneticoService()
        {
            Populacao = new List<int[]>();
            FitnessIndices = new List<double>();
        }

        public void Run()
        {
            int geracaoAtual = 0;
            int geracoesSemMelhora = 0;
            int melhorGeracao = 0;
            int[] melhorIndividuo = null;

            GerarPopulacaoInicial();
            Console.WriteLine("População Inicial:\n");
            MostrarPopulacao();

            while (geracaoAtual < NumeroMaximoDeGeracoes)
            {
                var pai1 = SelecaoPorTorneio();
                var pai2 = SelecaoPorTorneio();
                var descendente = CruzamentoPMX(pai1, pai2);
                descendente = Mutacao(descendente);

                if (SubstituirPiorIndividuo(descendente))
                {
                    // Atualize o FitnessIndices apenas quando necessário
                    CalcularFitness();
                }

                geracaoAtual++;
                Console.WriteLine($"\n{geracaoAtual}° Geração:\n");
                MostrarPopulacao();

                // Verifique se o melhor indivíduo mudou
                if (FitnessIndices.Max() > melhorIndividuo?.Max())
                {
                    geracoesSemMelhora = 0; // Resetar contagem
                    melhorIndividuo = Populacao[Array.IndexOf(FitnessIndices.ToArray(), FitnessIndices.Max())];
                    melhorGeracao = geracaoAtual;
                }
                else
                {
                    geracoesSemMelhora++;
                }

                if (melhorIndividuo != null)
                {
                    Console.WriteLine($"Melhor indivíduo na geração {geracaoAtual}: {FormatarCromossomo(melhorIndividuo)}");
                    Console.WriteLine($"Aptidão do melhor indivíduo: {FitnessIndices.Max()}");
                }
                else
                {
                    Console.WriteLine($"Melhor indivíduo na geração {geracaoAtual}: Nenhum indivíduo encontrado.");
                }

                if (geracoesSemMelhora >= MaxGeracoesSemMelhora)
                {
                    Console.WriteLine($"Convergência alcançada na {melhorGeracao}° geração.");
                    break;
                }
            }

            Console.ReadLine();
        }

        private void GerarPopulacaoInicial()
        {
            Populacao = new List<int[]>(TamanhoPopulacao);
            FitnessIndices = new List<double>(TamanhoPopulacao);

            for (int i = 0; i < TamanhoPopulacao; i++)
            {
                int[] cromossomo = GerarCromossomoAleatorio();
                Populacao.Add(cromossomo);
                FitnessIndices.Add(CalcularAptidao(cromossomo));
            }
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
            var aptidao = new double[TamanhoPopulacao];

            for (int i = 0; i < TamanhoPopulacao; i++)
            {
                var individuo = Populacao[i];

                var colisoes = CalcularColisoes(individuo);
                var aptidaoIndividuo = colisoes != 0 ? 1.0 / colisoes : 99999.0;

                aptidao[i] = aptidaoIndividuo;
                totalAptidao += aptidaoIndividuo;

                // Imprimir a aptidão do indivíduo
                Console.WriteLine($"Aptidão do Indivíduo {i + 1}: {aptidaoIndividuo}");
            }

            aptidao = aptidao.Select(a => a / totalAptidao).ToArray();

            FitnessIndices = aptidao.ToList();
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

            double aptidao1 = FitnessIndices[indiceIndividuo1];
            double aptidao2 = FitnessIndices[indiceIndividuo2];

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
            // Crie uma lista dos números que já estão no cromossomo
            var numerosNoCromossomo = new List<int>(individuo);

            for (int rainha1 = 0; rainha1 < QtdRainhas; rainha1++)
            {
                // Se encontrarmos um número repetido no cromossomo
                if (numerosNoCromossomo.Count(n => n == individuo[rainha1]) > 1)
                {
                    int numeroRepetido = individuo[rainha1];

                    // Encontre o menor número que não esteja no cromossomo
                    int menorNumero = 1;
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


        private bool SubstituirPiorIndividuo(int[] descendente)
        {
            double piorAptidao = double.MaxValue; // Inicialize com um valor alto para garantir que qualquer aptidão será menor.
            int indicePiorIndividuo = -1;

            // Encontrar o índice do pior indivíduo na lista FitnessIndices
            for (int i = 0; i < FitnessIndices.Count; i++)
            {
                if (FitnessIndices[i] < piorAptidao)
                {
                    piorAptidao = FitnessIndices[i];
                    indicePiorIndividuo = i;
                }
            }

            // Verificar se o descendente é melhor que o pior indivíduo e não é igual a nenhum indivíduo existente
            if (indicePiorIndividuo != -1 && CalcularAptidao(descendente) < piorAptidao)
            {
                Populacao[indicePiorIndividuo] = descendente;
                FitnessIndices[indicePiorIndividuo] = CalcularAptidao(descendente);
                return true;
            }

            return false;
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
                Console.WriteLine($"Indivíduo {individuoIndex++}: {FormatarCromossomo(individuo)}");
            }
        }

        private string FormatarCromossomo(int[] cromossomo)
        {
            return "[" + string.Join(", ", cromossomo) + "]";
        }
    }
}
