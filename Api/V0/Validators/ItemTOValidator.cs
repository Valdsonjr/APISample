using Api.v0.Models;
using Domain.Repositories;
using Domain.Resources;
using Domain.Types;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;

namespace Api.v0.Validators
{
    /// <summary>
    /// Validador de itens
    /// </summary>
    public class ItemTOValidator : AbstractValidator<ItemTO>
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public ItemTOValidator(IStringLocalizer<ErrorMessages> localizer, IItemRepository repository)
        {
            RuleFor(i => i.Key).NotEmpty().WithMessage(localizer["ItemErrorEmptyKey"])
                               .MaximumLength(Item.KeyMaxLength).WithMessage(localizer["ItemErrorKeyMaxSize"]);

            RuleFor(i => i.Value).NotEmpty().WithMessage(localizer["ItemErrorEmptyValue"])
                                 .MaximumLength(Item.ValueMaxLength).WithMessage(localizer["ItemErrorValueMaxSize"]);

            RuleFor(i => i.CreationDate).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(localizer["ItemErrorInvalidDate"]);

            RuleSet("Post", () =>
            {
                RuleFor(i => i.Key).Must(key => !repository.Obter().Any(i => i.Key == key))
                                   .WithMessage(localizer["ItemErrorAlreadyExists"]);
            });
        }
    }
}

