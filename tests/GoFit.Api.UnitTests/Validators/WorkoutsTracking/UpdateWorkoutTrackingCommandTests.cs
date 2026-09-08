using FluentValidation.TestHelper;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Dtos;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;

namespace GoFit.Api.UnitTests.Validators.WorkoutsTracking;
public class UpdateWorkoutTrackingCommandTests
{
    private const string ValidAppUserId = "app-user-1";
    private static readonly Guid AthleteId = Guid.NewGuid();

    private readonly UpdateWorkoutTrackingCommandValidator _validator;

    private readonly MockRepository _mockRepository;
    private readonly Mock<IWorkoutTrackingRepository> _workoutTrakingRepositoryMock;
    private readonly Mock<IAthleteRepository> _athleteRepositoryMock;

    public UpdateWorkoutTrackingCommandTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _workoutTrakingRepositoryMock = _mockRepository.Create<IWorkoutTrackingRepository>();
        _athleteRepositoryMock = _mockRepository.Create<IAthleteRepository>();

        _athleteRepositoryMock
            .Setup(x => x.GetByAppUserIdAsync(ValidAppUserId))
            .ReturnsAsync(new Athlete { Id = AthleteId, Name = "Test Athlete" });

        _validator = new UpdateWorkoutTrackingCommandValidator(
            _workoutTrakingRepositoryMock.Object,
            _athleteRepositoryMock.Object);
    }

    [Fact]
    [Trait("WorkoutTracking", "Note")]
    public async Task WhenNoteIsFilledWithLessThan300Chars_ShouldNotFail()
    {
        var command = new UpdateWorkoutTrackingCommand(
            WorkoutsTrackingId: Guid.NewGuid(),
            StartWorkoutDate: DateTime.Now,
            EndWorkoutDate: null,
            Note: "ab",
            Sets: Enumerable.Empty<WorkoutSetTrackingDto>(),
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Note);
    }

    [Fact]
    [Trait("WorkoutTracking", "Note")]
    public async Task WhenNoteIsFilledWithMoreThan300Chars_ShouldFail()
    {
        var command = new UpdateWorkoutTrackingCommand(
            WorkoutsTrackingId: Guid.NewGuid(),
            StartWorkoutDate: DateTime.Now,
            EndWorkoutDate: null,
            Note: "Lorem ipsum dolor sit amet consectetur adipisicing elit. Aut odio tempore maxime quia. Repellendus totam quae amet ducimus nostrum, error odio vero laborum consequatur quisquam repellat. Quis voluptatibus ullam assumenda adipisci ipsam itaque? Distinctio saepe reiciendis vel eum, deleniti quisquam perferendis voluptatibus temporibus laudantium maiores eos, ab aperiam qui officiis odio labore necessitatibus enim illo. Quasi, laboriosam iusto, voluptatem omnis, sequi a delectus facere sunt rerum ipsum dignissimos optio ea odio hic. Quod debitis, velit ducimus quibusdam animi hic expedita asperiores corrupti! Asperiores, obcaecati laboriosam commodi consectetur distinctio reiciendis dignissimos praesentium deleniti nam quis facere, sed autem iusto, excepturi animi illum soluta sit aliquid! Ex et incidunt est assumenda fugit, ducimus velit beatae eligendi, ratione reiciendis sit odit! Architecto iure saepe doloremque voluptas illum temporibus iusto, sit nostrum voluptate soluta! Voluptatum repudiandae vero beatae fugiat quibusdam voluptates non. Veritatis, dolore? Quisquam perspiciatis, placeat sapiente omnis nisi animi quod dignissimos esse nemo, dolorum necessitatibus quae error dolores iure deleniti. Fugiat, cumque reprehenderit perferendis excepturi tempore consequatur illo laborum aut! Fuga asperiores natus maxime explicabo illo delectus sint numquam recusandae, nobis quis eius, culpa quidem optio cupiditate perspiciatis nesciunt? Labore ipsum dolores aspernatur ut tenetur molestiae eveniet necessitatibus cumque culpa natus aut illum vitae, ducimus amet adipisci quibusdam blanditiis in reiciendis nam sequi! Tempore, autem? Autem sunt accusamus adipisci quas laudantium eos dolorum expedita fugit dolores, quo aliquam provident voluptatem culpa, officia est alias, facilis vero dolore asperiores error vel quasi facere? Voluptatem eos asperiores repudiandae reprehenderit voluptas incidunt quaerat harum minus delectus mollitia ab quasi, odit alias. Exercitationem dolor architecto fuga. Possimus numquam pariatur alias, expedita sunt, ut error tempora, blanditiis dolores ducimus minus totam. Dolores dicta maiores atque architecto repudiandae? Accusamus in, alias odit deleniti fugit earum eaque perspiciatis hic officia corporis laborum nam. At incidunt placeat culpa aliquid provident.",
            Sets: Enumerable.Empty<WorkoutSetTrackingDto>(),
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Note);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenAppUserIdIsEmpty_ShouldFail()
    {
        var command = new UpdateWorkoutTrackingCommand(
            WorkoutsTrackingId: Guid.NewGuid(),
            StartWorkoutDate: DateTime.Now,
            EndWorkoutDate: null,
            Note: null,
            Sets: [new WorkoutSetTrackingDto { Repetitions = 10, Weight = 10, Order = 0 }],
            AppUserId: "");

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.AppUserId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenTrackingBelongsToAnotherAthlete_ShouldFail()
    {
        var trackingId = Guid.NewGuid();

        _workoutTrakingRepositoryMock
            .Setup(x => x.GetAsync(trackingId))
            .ReturnsAsync(new WorkoutTracking { Id = trackingId, AthleteId = Guid.NewGuid() });

        var command = new UpdateWorkoutTrackingCommand(
            WorkoutsTrackingId: trackingId,
            StartWorkoutDate: DateTime.Now,
            EndWorkoutDate: null,
            Note: null,
            Sets: [new WorkoutSetTrackingDto { Repetitions = 10, Weight = 10, Order = 0 }],
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.WorkoutsTrackingId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenTrackingAlreadyFinished_ShouldFail()
    {
        var trackingId = Guid.NewGuid();

        _workoutTrakingRepositoryMock
            .Setup(x => x.GetAsync(trackingId))
            .ReturnsAsync(new WorkoutTracking { Id = trackingId, AthleteId = AthleteId, EndWorkoutDate = DateTime.UtcNow });

        var command = new UpdateWorkoutTrackingCommand(
            WorkoutsTrackingId: trackingId,
            StartWorkoutDate: DateTime.Now,
            EndWorkoutDate: null,
            Note: null,
            Sets: [new WorkoutSetTrackingDto { Repetitions = 10, Weight = 10, Order = 0 }],
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.WorkoutsTrackingId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenTrackingAlreadyCancelled_ShouldFail()
    {
        var trackingId = Guid.NewGuid();

        _workoutTrakingRepositoryMock
            .Setup(x => x.GetAsync(trackingId))
            .ReturnsAsync(new WorkoutTracking { Id = trackingId, AthleteId = AthleteId, CancelledDate = DateTime.UtcNow });

        var command = new UpdateWorkoutTrackingCommand(
            WorkoutsTrackingId: trackingId,
            StartWorkoutDate: DateTime.Now,
            EndWorkoutDate: null,
            Note: null,
            Sets: [new WorkoutSetTrackingDto { Repetitions = 10, Weight = 10, Order = 0 }],
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.WorkoutsTrackingId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Ownership")]
    public async Task WhenTrackingIsOwnedAndInProgress_ShouldNotFail()
    {
        var trackingId = Guid.NewGuid();

        _workoutTrakingRepositoryMock
            .Setup(x => x.GetAsync(trackingId))
            .ReturnsAsync(new WorkoutTracking { Id = trackingId, AthleteId = AthleteId });

        var command = new UpdateWorkoutTrackingCommand(
            WorkoutsTrackingId: trackingId,
            StartWorkoutDate: DateTime.Now,
            EndWorkoutDate: null,
            Note: null,
            Sets: [new WorkoutSetTrackingDto { Repetitions = 10, Weight = 10, Order = 0 }],
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.WorkoutsTrackingId);
        result.ShouldNotHaveValidationErrorFor(x => x.AppUserId);
    }

    [Fact]
    [Trait("WorkoutTracking", "Sets")]
    public async Task WhenSetsIsEmpty_ShouldNotFail()
    {
        var trackingId = Guid.NewGuid();

        _workoutTrakingRepositoryMock
            .Setup(x => x.GetAsync(trackingId))
            .ReturnsAsync(new WorkoutTracking { Id = trackingId, AthleteId = AthleteId });

        var command = new UpdateWorkoutTrackingCommand(
            WorkoutsTrackingId: trackingId,
            StartWorkoutDate: DateTime.Now,
            EndWorkoutDate: null,
            Note: null,
            Sets: Enumerable.Empty<WorkoutSetTrackingDto>(),
            AppUserId: ValidAppUserId);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Sets);
    }
}
