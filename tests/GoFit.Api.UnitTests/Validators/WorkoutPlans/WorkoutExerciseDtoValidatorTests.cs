using FluentValidation.TestHelper;
using GoFit.Application.EntitiesActions.WorkoutPlans.Dtos;
using GoFit.Application.EntitiesActions.WorkoutPlans.Validators;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Api.UnitTests.Validators.WorkoutPlans;
public class WorkoutExerciseDtoValidatorTests
{
    private readonly WorkoutExerciseDtoValidator _validator;

    private readonly MockRepository _mockRepository;
    private readonly Mock<IExerciseRepository> _exerciseRepositoryMock;

    public WorkoutExerciseDtoValidatorTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _exerciseRepositoryMock = _mockRepository.Create<IExerciseRepository>();

        _validator = new WorkoutExerciseDtoValidator(_exerciseRepositoryMock.Object);
    }

    [Fact]
    [Trait("WorkoutExerciseDto", "Title")]
    public async Task WhenExerciseIdDoesNotExists_ShouldFail()
    {
        _exerciseRepositoryMock
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Exercise?)null);

        var dto = new WorkoutExerciseDto
        {
            ExerciseId = Guid.NewGuid()
        };

        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.ExerciseId);
    }
}
