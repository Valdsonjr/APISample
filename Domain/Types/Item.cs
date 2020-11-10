using System;

namespace Domain.Types
{
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

        /// <summary>
        /// Tamanho máximo de uma chave
        /// </summary>
        public static readonly int KeyMaxLength = 100;

        /// <summary>
        /// Tamanho máximo de um valor
        /// </summary>
        public static readonly int ValueMaxLength = 5000;
    }
}
