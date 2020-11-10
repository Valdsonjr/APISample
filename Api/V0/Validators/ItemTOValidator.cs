using Api.V0.Models;
using Domain.Repositories;
using Domain.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;

namespace Api.V0.Validators
{
    /// <summary>
    /// Validador de itens
    /// </summary>
    public class ItemTOValidator : AbstractValidator<ItemTO>
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
        public ItemTOValidator(IStringLocalizer<ErrorMessages> localizer, IItemRepository repository)
        {
            RuleSet("Common", () =>
            {
                RuleFor(i => i.Key).NotEmpty().WithMessage(localizer["ItemErrorEmptyKey"])
                                   .MaximumLength(KeyMaxLength).WithMessage(localizer["ItemErrorKeyMaxSize"]);

                RuleFor(i => i.Value).NotEmpty().WithMessage(localizer["ItemErrorEmptyValue"])
                                     .MaximumLength(ValueMaxLength).WithMessage(localizer["ItemErrorValueMaxSize"]);

                RuleFor(i => i.CreationDate).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(localizer["ItemErrorInvalidDate"]);
            });

            RuleSet("Post", () =>
            {
                RuleFor(i => i.Key).Must(key => repository.Obter().FirstOrDefault(i => i.Key == key) == null)
                                   .WithMessage(localizer["ItemErrorAlreadyExists"]);
            });
        }
    }
}

