using FluentValidation;
using GoFit.Application.Interfaces;
using GoFit.Domain.Common;

namespace GoFit.Application.Common.Validators;
public class EntityMustExistsValidator<T> : AbstractValidator<Guid> where T : BaseEntity
{
    public EntityMustExistsValidator(IBaseRepository<T> baseRepository)
    {
        RuleFor(x => x)
            .MustAsync(async (id, ct) =>
            {
                T? entity = await baseRepository.GetAsync(id);

                return entity is not null;
            });
    }
}
