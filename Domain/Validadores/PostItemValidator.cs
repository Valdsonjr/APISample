using Domain.Recursos;
using Domain.Repositorios;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Domain.Validadores
{
    /// <summary>
    /// Validação de inserção de item
    /// </summary>
    public class PostItemValidator : ItemValidator
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public PostItemValidator(IStringLocalizer<ErrorMessages> localizer, IItemRepository repository) : base(localizer)
        {
            RuleFor(i => i.Key).Must(i => repository.Obter(i) == null)
                               .WithMessage(localizer["ItemErrorAlreadyExists"]);
        }
    }
}
