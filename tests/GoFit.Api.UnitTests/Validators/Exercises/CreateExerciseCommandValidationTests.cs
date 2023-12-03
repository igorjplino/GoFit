using FluentValidation.TestHelper;
using GoFit.Application.EntitiesActions.Exercises.Commands;
using GoFit.Application.EntitiesActions.Exercises.Validators;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using System.Xml.Linq;

namespace GoFit.Api.UnitTests.Validators.Exercises;

public class CreateExerciseCommandValidationTests
{
    private readonly CreateExerciseCommandValidation _validator;

    private readonly MockRepository _mockRepository;
    private readonly Mock<IExerciseRepository> _exerciseRepositoryMock;

    public CreateExerciseCommandValidationTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _exerciseRepositoryMock = _mockRepository.Create<IExerciseRepository>();

        _validator = new CreateExerciseCommandValidation(_exerciseRepositoryMock.Object);
    }

    [Fact]
    [Trait("Exercise", "Name")]
    public async Task WhenNameIsNull_ShouldFail()
    {
        var command = new CreateExerciseCommand(Name: null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Exercise", "Name")]
    public async Task WhenNameIsEmpty_ShouldFail()
    {
        var command = new CreateExerciseCommand(Name: string.Empty);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Exercise", "Name")]
    public async Task WhenNameHasLessThan3Chars_ShouldFail()
    {
        var command = new CreateExerciseCommand(Name: "ab");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Exercise", "Name")]
    public async Task WhenNameHasMoreThan100Chars_ShouldFail()
    {
        var command = new CreateExerciseCommand(Name: "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa.");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Exercise", "Name")]
    public async Task WhenNameAlreadyExists_ShouldFail()
    {
        _exerciseRepositoryMock
            .Setup(x => x.GetByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new Exercise());

        var command = new CreateExerciseCommand(Name: "Pull Up");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Exercise", "Name")]
    public async Task WhenNameIsCorrect_ShouldNotFail()
    {
        _exerciseRepositoryMock
            .Setup(x => x.GetByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((Exercise?)null);

        var command = new CreateExerciseCommand(Name: "Pull Up");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Exercise", "Description")]
    public async Task WhenDescriptionIsNull_ShouldFail()
    {
        var command = new CreateExerciseCommand(Name: string.Empty, Description: null);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("Exercise", "Description")]
    public async Task WhenDescriptionIsEmpty_ShouldFail()
    {
        var command = new CreateExerciseCommand(Name: string.Empty, Description: string.Empty);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("Exercise", "Description")]
    public async Task WhenDescriptionHasLessThan3Chars_ShouldFail()
    {
        var command = new CreateExerciseCommand(Name: string.Empty, Description: "ab");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("Exercise", "Description")]
    public async Task WhenDescriptionHasMoreThan300Chars_ShouldFail()
    {
        var command = new CreateExerciseCommand(Name: string.Empty, Description: "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo.");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("Exercise", "Description")]
    public async Task WhenDescriptionIsCorrect_ShouldNotFail()
    {
        var command = new CreateExerciseCommand(Name: string.Empty, Description: "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem.");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }
}
