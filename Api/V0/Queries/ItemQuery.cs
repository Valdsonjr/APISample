using System;

namespace Api.V0.Queries
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
        public DateTime? CreationDateInit { get; set; }
        /// <summary>
        /// Procura por itens com data de criação menor ou igual ao valor passado
        /// </summary>
        public DateTime? CreationDateEnd { get; set; }
    }
}
