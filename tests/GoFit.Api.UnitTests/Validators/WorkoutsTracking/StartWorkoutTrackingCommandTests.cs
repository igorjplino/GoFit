using FluentValidation.TestHelper;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Api.UnitTests.Validators.WorkoutsTracking;
public class StartWorkoutTrackingCommandTests
{
    private const string ValidAppUserId = "app-user-1";
    private static readonly Guid AthleteId = Guid.NewGuid();

    private readonly StartWorkoutTrackingCommandValidator _validator;

    private readonly MockRepository _mockRepository;
    private readonly Mock<IWorkoutRepository> _workoutRepositoryMock;
    private readonly Mock<IWorkoutTrackingRepository> _workoutTrackingRepositoryMock;
    private readonly Mock<IAthleteRepository> _athleteRepositoryMock;

    public StartWorkoutTrackingCommandTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _workoutRepositoryMock = _mockRepository.Create<IWorkoutRepository>();
        _workoutTrackingRepositoryMock = _mockRepository.Create<IWorkoutTrackingRepository>();
        _athleteRepositoryMock = _mockRepository.Create<IAthleteRepository>();

        _athleteRepositoryMock
            .Setup(x => x.GetByAppUserIdAsync(ValidAppUserId))
            .ReturnsAsync(new Athlete { Id = AthleteId, Name = "Test Athlete" });

        _validator = new StartWorkoutTrackingCommandValidator(
            _workoutRepositoryMock.Object,
            _workoutTrackingRepositoryMock.Object,
            _athleteRepositoryMock.Object);
    }

    [Fact]
    [Trait("WorkoutTracking", "Note")]
    public async Task WhenNoteIsFilledWithLessThan300Chars_ShouldNotFail()
    {
        var command = new StartWorkoutTrackingCommand(
            WorkoutId: Guid.NewGuid(),
            Note: "ab",
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Note);
    }

    [Fact]
    [Trait("WorkoutTracking", "Note")]
    public async Task WhenNoteIsFilledWithMoreThan300Chars_ShouldFail()
    {
        var command = new StartWorkoutTrackingCommand(
            WorkoutId: Guid.NewGuid(),
            Note: "Lorem ipsum dolor sit amet consectetur adipisicing elit. Aut odio tempore maxime quia. Repellendus totam quae amet ducimus nostrum, error odio vero laborum consequatur quisquam repellat. Quis voluptatibus ullam assumenda adipisci ipsam itaque? Distinctio saepe reiciendis vel eum, deleniti quisquam perferendis voluptatibus temporibus laudantium maiores eos, ab aperiam qui officiis odio labore necessitatibus enim illo. Quasi, laboriosam iusto, voluptatem omnis, sequi a delectus facere sunt rerum ipsum dignissimos optio ea odio hic. Quod debitis, velit ducimus quibusdam animi hic expedita asperiores corrupti! Asperiores, obcaecati laboriosam commodi consectetur distinctio reiciendis dignissimos praesentium deleniti nam quis facere, sed autem iusto, excepturi animi illum soluta sit aliquid! Ex et incidunt est assumenda fugit, ducimus velit beatae eligendi, ratione reiciendis sit odit! Architecto iure saepe doloremque voluptas illum temporibus iusto, sit nostrum voluptate soluta! Voluptatum repudiandae vero beatae fugiat quibusdam voluptates non. Veritatis, dolore? Quisquam perspiciatis, placeat sapiente omnis nisi animi quod dignissimos esse nemo, dolorum necessitatibus quae error dolores iure deleniti. Fugiat, cumque reprehenderit perferendis excepturi tempore consequatur illo laborum aut! Fuga asperiores natus maxime explicabo illo delectus sint numquam recusandae, nobis quis eius, culpa quidem optio cupiditate perspiciatis nesciunt? Labore ipsum dolores aspernatur ut tenetur molestiae eveniet necessitatibus cumque culpa natus aut illum vitae, ducimus amet adipisci quibusdam blanditiis in reiciendis nam sequi! Tempore, autem? Autem sunt accusamus adipisci quas laudantium eos dolorum expedita fugit dolores, quo aliquam provident voluptatem culpa, officia est alias, facilis vero dolore asperiores error vel quasi facere? Voluptatem eos asperiores repudiandae reprehenderit voluptas incidunt quaerat harum minus delectus mollitia ab quasi, odit alias. Exercitationem dolor architecto fuga. Possimus numquam pariatur alias, expedita sunt, ut error tempora, blanditiis dolores ducimus minus totam. Dolores dicta maiores atque architecto repudiandae? Accusamus in, alias odit deleniti fugit earum eaque perspiciatis hic officia corporis laborum nam. At incidunt placeat culpa aliquid provident.",
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Note);
    }

    [Fact]
    [Trait("WorkoutTracking", "Note")]
    public async Task WhenNoteIsNull_ShouldNotFail()
    {
        var command = new StartWorkoutTrackingCommand(
            WorkoutId: Guid.NewGuid(),
            Note: null,
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Note);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenAppUserIdIsEmpty_ShouldFail()
    {
        var command = new StartWorkoutTrackingCommand(
            WorkoutId: Guid.NewGuid(),
            Note: null,
            AppUserId: "");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.AppUserId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenNoAthleteLinkedToAppUserId_ShouldFail()
    {
        var command = new StartWorkoutTrackingCommand(
            WorkoutId: Guid.NewGuid(),
            Note: null,
            AppUserId: "unknown-app-user");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.AppUserId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenWorkoutBelongsToAnotherAthlete_ShouldFail()
    {
        var workoutId = Guid.NewGuid();

        _workoutRepositoryMock
            .Setup(x => x.GetWithDetailsAsync(workoutId))
            .ReturnsAsync(new Workout
            {
                Id = workoutId,
                WorkoutPlan = new WorkoutPlan { AthleteId = Guid.NewGuid() }
            });

        var command = new StartWorkoutTrackingCommand(
            WorkoutId: workoutId,
            Note: null,
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.WorkoutId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenAthleteAlreadyHasAnActiveWorkout_ShouldFail()
    {
        _workoutTrackingRepositoryMock
            .Setup(x => x.GetActiveByAthleteIdAsync(AthleteId))
            .ReturnsAsync(new WorkoutTracking { Id = Guid.NewGuid() });

        var command = new StartWorkoutTrackingCommand(
            WorkoutId: Guid.NewGuid(),
            Note: null,
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.WorkoutId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenAthleteOwnsWorkoutAndHasNoActiveWorkout_ShouldNotFail()
    {
        var workoutId = Guid.NewGuid();
        var workout = new Workout
        {
            Id = workoutId,
            WorkoutPlan = new WorkoutPlan { AthleteId = AthleteId }
        };

        _workoutRepositoryMock
            .Setup(x => x.GetAsync(workoutId))
            .ReturnsAsync(workout);

        _workoutRepositoryMock
            .Setup(x => x.GetWithDetailsAsync(workoutId))
            .ReturnsAsync(workout);

        var command = new StartWorkoutTrackingCommand(
            WorkoutId: workoutId,
            Note: null,
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.WorkoutId);
        result.ShouldNotHaveValidationErrorFor(x => x.AppUserId);
    }
}
