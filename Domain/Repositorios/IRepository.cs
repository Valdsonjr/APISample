namespace Domain.Repositorios
{
    /// <summary>
    /// Interface genérica de repositórios
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// "Salva a transação", note que essa operação pode significar coisas bem diferentes para repositórios diferentes
        /// </summary>
        /// <returns></returns>
        public void Commit();
    }
}
