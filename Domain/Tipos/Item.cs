using System;

namespace Domain.Tipos
{
    /// <summary>
    /// Item de teste da api
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Chave de identificação do item
        /// </summary>
        public String Key { get; set; } = "";
        /// <summary>
        /// Texto do item
        /// </summary>
        public String Value { get; set; } = "";
        /// <summary>
        /// Data de criação do item
        /// </summary>
        public DateTime CreationDate { get; protected set; } = DateTime.UtcNow;
    }
}
