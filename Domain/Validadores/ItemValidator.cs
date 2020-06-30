using Domain.Recursos;
using Domain.Repositorios;
using Domain.Tipos;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;

namespace Domain.Validadores
{
    /// <summary>
    /// Validador de itens
    /// </summary>
    public class ItemValidator : AbstractValidator<Item>
    {
        /// <summary>
        /// Tamanho máximo de uma chave
        /// </summary>
        public static readonly int KeyMaxLength = 100;

        /// <summary>
        /// Tamanho máximo de um valor
        /// </summary>
        public static readonly int ValueMaxLength = 5000;

        /// <summary>
        /// Construtor
        /// </summary>
        public ItemValidator(IStringLocalizer<ErrorMessages> localizer)
        {
            RuleFor(i => i.Key).NotEmpty().WithMessage(localizer["ItemErrorEmptyKey"])
                               .MaximumLength(KeyMaxLength).WithMessage(localizer["ItemErrorKeyMaxSize"]);

            RuleFor(i => i.Value).NotEmpty().WithMessage(localizer["ItemErrorEmptyValue"])
                                 .MaximumLength(ValueMaxLength).WithMessage(localizer["ItemErrorValueMaxSize"]);

            RuleFor(i => i.CreationDate).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(localizer["ItemErrorInvalidDate"]);
        }
    }

    /// <summary>
    /// Validação de inserção de item
    /// </summary>
    public class PostItemValidator : ItemValidator
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public PostItemValidator(IStringLocalizer<ErrorMessages> localizer, IItemRepository repository) : base (localizer)
        {
            RuleFor(i => i.Key).Must(i => repository.ObterPorId(i) == null)
                               .WithMessage(localizer["ItemErrorAlreadyExists"]);
        }
    }
}
