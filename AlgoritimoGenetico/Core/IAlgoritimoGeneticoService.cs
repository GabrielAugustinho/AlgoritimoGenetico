namespace AlgoritimoGenetico.Core
{
    public interface IAlgoritimoGeneticoService
    {
        void Run(int tamanhoPopulacao, int numeroMaximoDeGeracoes, double taxaDeCruzamento, double taxaDeMutacao, double intervaloGeracional);
    }
}
