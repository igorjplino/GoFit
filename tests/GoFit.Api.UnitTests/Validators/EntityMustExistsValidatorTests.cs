using FluentValidation.TestHelper;
using GoFit.Application.Common.Validators;
using GoFit.Application.Interfaces;
using GoFit.Domain.Common;

namespace GoFit.Api.UnitTests.Validators;

public class EntityMustExistsValidatorTests
{
    private readonly EntityMustExistsValidator<BaseEntity> _validator;

    private readonly MockRepository _mockRepository;
    private readonly Mock<IBaseRepository<BaseEntity>> _baseRepositoryMock;

    public EntityMustExistsValidatorTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _baseRepositoryMock = _mockRepository.Create<IBaseRepository<BaseEntity>>();

        _validator = new EntityMustExistsValidator<BaseEntity>(_baseRepositoryMock.Object);
    }

    [Fact]
    [Trait("BaseEntity", "Id")]
    public async Task WhenEntityDoesNotExists_ShouldFail()
    {
        var entityId = It.IsAny<Guid>();

        _baseRepositoryMock
            .Setup(x => x.GetAsync(entityId))
            .ReturnsAsync((BaseEntity?)null);

        var result = await _validator.TestValidateAsync(entityId);

        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact]
    [Trait("BaseEntity", "Id")]
    public async Task WhenEntityExists_ShouldNotFail()
    {
        var entityId = It.IsAny<Guid>();

        var mockBaseEntity = _mockRepository.Create<BaseEntity>();

        _baseRepositoryMock
            .Setup(x => x.GetAsync(entityId))
            .ReturnsAsync(mockBaseEntity.Object);

        var result = await _validator.TestValidateAsync(entityId);

        result.ShouldNotHaveValidationErrorFor(x => x);
    }
}
