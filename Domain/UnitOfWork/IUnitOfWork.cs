namespace Domain.UnitOfWork
{
    /// <summary>
    /// Interface genérica de unidades de trabalho
    /// 
    /// Uma unidade de trabalho serve para controlar transações dentro de um contexto
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// "Salva a transação", note que essa operação pode significar coisas bem diferentes para repositórios diferentes
        /// </summary>
        /// <returns></returns>
        public void Commit();
    }
}
