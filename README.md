Um Algoritmo Genético Especializado para o Problema das N Rainhas

“Computação Evolutiva”

Trabalho base (Um Algoritmo Genético Especializado para o Problema das N Rainhas): https://www.researchgate.net/profile/Silvia-Lopes-De-Sena-Taglialenha/publication/368591172_Um_Algoritmo_Genetico_Especializado_para_o_Problema_das_N_Rainhas/links/63ef95e319130a1a4a8960d9/Um-Algoritmo-Genetico-Especializado-para-o-Problema-das-N-Rainhas.pdf

Estrutura do cromossomo/Individuo -> Vetor V de tamanho n onde n é a quantidade de linhas ou colunas, consequentemente de rainhas. A posição do vetor onde a rainha se encontra será a linha e a rainha com o nome “3” estará na coluna “3”. Int[n] V (n = quantidade rainhas]

" - - 3 - "
" - - - 4 "
" - 2 - - "
" 1 - - - "

Método p/ gerar a população inicial -> A população de tamanho m (TP) será representada por uma matriz onde cada linha da matriz representa um cromossomo. Aleatório uniforme.

TP < n! // Impede de ter indivíduo iguais V[n] = [1,2,3...n] for(int i = 0; i <= TP; i++) V = V.embaralhar; while(população.Contains(v)) // verifica se o indivíduo já existe V = V.embaralhar; População[i] = v

Função de aptidão fitness -> A função objetivo é calculada verificando o número de colisões entre rainhas percorrendo as diagonais positiva e negativa do tabuleiro. É agregada a cada diagonal uma constante que a identifica, e se existir mais de uma rainha em uma diagonal é contabilizado mais uma colisão. A soma do número de colisões existentes nas diagonais positiva e negativa geram o cálculo da função objetivo. Dessa forma pode-se calcular a função objetivo de todos os elementos da população. Rever conceito Operação de cruzamento / corssover -> Optou-se em trabalhar com a recombinação PMX [3] que recombina os dois indivíduos sem perder o que já havíamos conquistado (configuração sem colisões entre rainhas na mesma linha e coluna). Este tipo de recombinação trabalha com dois indivíduos previamente selecionados. É escolhido aleatoriamente o tamanho da faixa (Para ter um valor, pode ser min 2 e máximo 6) que será recombinada e onde a recombinação irá começar. Os dois indivíduos geram somente um descendente, onde é escolhido aleatoriamente (pode ser o indivíduo com menor número de colisão) em qual dos dois indivíduos a recombinação será executada para a geração de um descendente.

MÉTODO RETORNA Lista ProbabilidadSobrevivencia(populacao){ ParaCadaCromossomo(){ totalColisoes = ContarColisoes() Se total de colisoes != 0 fitness[i] = 1/totalColisoes Se não fitiness[i] = 9999 SOMA += fitness[i] } ParaCadaFitnessEncontrado(){ fitness [i] = fitness[i]/SOMA }
retorna fitness }

Operador de seleção -> Método de seleção por torneio com k=2, ou seja, é escolhido para participar de cada jogo dois indivíduos (configurações) da população corrente que são analisados, o indivíduo com maior fitness sobrevive.

MÉTODO RETORNA O MAISFORTE Torneio(fitness){ individuo = random(0, tamanhoPopulacao) competidor = random(0, tamanhoPopulacao) Se fitness[individuo] > fitness[competidor]{ retorna individuo } retorna competidor }

Operação de mutação -> A mutação se dá de uma maneira muito simples. Serão escolhidas aleatoriamente duas rainhas em cada indivíduo da população corrente para se fazer à troca de posição destas rainhas no tabuleiro.

Parâmetro genético: tamanho da população -> Número de gerações -> Somente será aproveitado o indivíduo gerado na etapa de seleção, mutação e melhora local se este detém um número menor de colisões do que o indivíduo que detém o maior número de colisões da população corrente. Além disso, é verificado se esta configuração já existe na população corrente, se existir o mesmo indivíduo na população, o indivíduo descendente será descartado, caso contrário ele será aproveitado na população corrente e entrará no lugar do indivíduo que tinha o número maior de colisões. Não encontrado no arquivo mas podemos nos basear nessa frase

Taxa de cruzamento -> Verifica-se qual das diagonais, positiva ou negativa, desta configuração está com o maior número de colisões. Depois de identificá-la é simulada a troca de posição de cada par de rainhas que se encontram nesta diagonal. A troca que produzir a maior diminuição na função objetivo é escolhida para encontrar um descendente melhorado. O processo é repetido com a nova diagonal mais carregada e esse processo de melhoria local termina quando todas as trocas realizadas na diagonal mais carregada não produzirem uma nova configuração de melhor qualidade. Rever conceito

Taxa de mutação -> Não encontrado no arquivo

Intervalo de geração -> O algoritmo genético aplicado neste trabalho é baseado no Algoritmo Genético de ChuBeasley, faz uma substituição em cada geração apenas um elemento da população corrente é substituído. A recombinação é realizada e apenas um descendente é escolhido. Este descendente sofre a aplicação do operador genético de mutação, onde somente uma troca de posição entre duas rainhas escolhidas é realizada e ao final da mutação o descendente passa por um processo de melhora local. Se após o processo de melhora local o descendente é melhor que o pior elemento da população corrente e ainda, não é igual aos elementos desta população, o descendente então substitui o pior elemento, caso contrário o descendente não é aproveitado na população. Rever conceito

Algoritmo -> O algoritmo assume a seguinte forma:

Gera-se a população inicial aleatoriamente.
Transforma-se a população inicial na população corrente.
Calcula-se o valor da função objetivo.
Implementa-se o processo de seleção de dois indivíduos usando a seleção por torneio;
Escolhe-se aleatoriamente o tamanho da faixa de recombinação e a posição de início.
Escolhe-se aleatoriamente qual indivíduo sofrerá a recombinação PMX.
Implementa-se o processo de recombinação PMX.
Implementa-se a mutação escolhendo dois pontos de mutação aleatoriamente.
Verifica-se a possibilidade de melhorar o indivíduo gerado na etapa anterior. Neste caso o descendente passa por uma série de comparações. • Primeiro localiza-se a diagonal mais carregada (número maior de rainhas). • Troca-se de posição as rainhas dessa diagonal e verifica-se qual troca teve o menor número de colisões e a troca de resultado melhor é consumada.
Verifica-se o valor da função objetivo do indivíduo descendente. Se este número for menor do que o maior da população corrente o indivíduo pode ser aproveitado caso contrário volta-se para o passo 3.
Se o indivíduo descendente é melhor do que o pior encontrado na população corrente, ele passa por uma nova comparação. Este novo indivíduo (configuração) deve ser diferente de todos os outros da população, se já existir na população corrente a mesma configuração, esta será descartada e volta-se para o passo 3, caso contrário ela será aproveitada, fazendo parte da nova geração.
Atualiza-se a população corrente gerando a próxima geração e volta ao passo 2
O algoritmo para em um número pré-determinado de iterações ou se encontrar uma solução com função objetivo igual a zero.
