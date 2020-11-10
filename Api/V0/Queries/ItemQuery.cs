using System;

namespace Api.v0.Queries
{
    /// <summary>
    /// Consulta de itens
    /// </summary>
    public class ItemQuery
    {
        /// <summary>
        /// Procura por itens com a chave igual ao valor passado
        /// </summary>
        public String? Key { get; set; }
        /// <summary>
        /// Procura por itens com data de criação maior ou igual ao valor passado
        /// </summary>
        public DateTime? CreationDateInit
        {
            get => creationDateInit;
            // REF https://github.com/dotnet/aspnetcore/issues/11584
            set => creationDateInit = value.HasValue ? value.Value.ToUniversalTime() : value;
        }
        private DateTime? creationDateInit;
        private DateTime? creationDateEnd;

        /// <summary>
        /// Procura por itens com data de criação menor ou igual ao valor passado
        /// </summary>
        public DateTime? CreationDateEnd 
        { 
            get => creationDateEnd; 
            set => creationDateEnd = value.HasValue ? value.Value.ToUniversalTime() : value;
        }
    }
}
