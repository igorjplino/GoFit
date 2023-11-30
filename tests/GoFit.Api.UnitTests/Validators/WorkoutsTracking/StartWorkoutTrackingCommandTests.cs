using FluentValidation.TestHelper;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Commands;
using GoFit.Application.EntitiesActions.WorkoutsTracking.Validators;
using GoFit.Application.Interfaces;

namespace GoFit.Api.UnitTests.Validators.WorkoutsTracking;
public class StartWorkoutTrackingCommandTests
{
    private readonly StartWorkoutTrackingCommandValidator _validator;

    private readonly MockRepository _mockRepository;
    private readonly Mock<IWorkoutRepository> _workoutRepositoryMock;

    public StartWorkoutTrackingCommandTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);

        _workoutRepositoryMock = _mockRepository.Create<IWorkoutRepository>();

        _validator = new StartWorkoutTrackingCommandValidator(_workoutRepositoryMock.Object);
    }

    [Fact]
    [Trait("WorkoutTracking", "Note")]
    public async Task WhenNoteIsFilledWithLessThan300Chars_ShouldNotFail()
    {
        var command = new StartWorkoutTrackingCommand
        {
            Note = "ab"
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Note);
    }

    [Fact]
    [Trait("WorkoutTracking", "Note")]
    public async Task WhenNoteIsFilledWithMoreThan300Chars_ShouldFail()
    {
        var command = new StartWorkoutTrackingCommand
        {
            Note = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Aut odio tempore maxime quia. Repellendus totam quae amet ducimus nostrum, error odio vero laborum consequatur quisquam repellat. Quis voluptatibus ullam assumenda adipisci ipsam itaque? Distinctio saepe reiciendis vel eum, deleniti quisquam perferendis voluptatibus temporibus laudantium maiores eos, ab aperiam qui officiis odio labore necessitatibus enim illo. Quasi, laboriosam iusto, voluptatem omnis, sequi a delectus facere sunt rerum ipsum dignissimos optio ea odio hic. Quod debitis, velit ducimus quibusdam animi hic expedita asperiores corrupti! Asperiores, obcaecati laboriosam commodi consectetur distinctio reiciendis dignissimos praesentium deleniti nam quis facere, sed autem iusto, excepturi animi illum soluta sit aliquid! Ex et incidunt est assumenda fugit, ducimus velit beatae eligendi, ratione reiciendis sit odit! Architecto iure saepe doloremque voluptas illum temporibus iusto, sit nostrum voluptate soluta! Voluptatum repudiandae vero beatae fugiat quibusdam voluptates non. Veritatis, dolore? Quisquam perspiciatis, placeat sapiente omnis nisi animi quod dignissimos esse nemo, dolorum necessitatibus quae error dolores iure deleniti. Fugiat, cumque reprehenderit perferendis excepturi tempore consequatur illo laborum aut! Fuga asperiores natus maxime explicabo illo delectus sint numquam recusandae, nobis quis eius, culpa quidem optio cupiditate perspiciatis nesciunt? Labore ipsum dolores aspernatur ut tenetur molestiae eveniet necessitatibus cumque culpa natus aut illum vitae, ducimus amet adipisci quibusdam blanditiis in reiciendis nam sequi! Tempore, autem? Autem sunt accusamus adipisci quas laudantium eos dolorum expedita fugit dolores, quo aliquam provident voluptatem culpa, officia est alias, facilis vero dolore asperiores error vel quasi facere? Voluptatem eos asperiores repudiandae reprehenderit voluptas incidunt quaerat harum minus delectus mollitia ab quasi, odit alias. Exercitationem dolor architecto fuga. Possimus numquam pariatur alias, expedita sunt, ut error tempora, blanditiis dolores ducimus minus totam. Dolores dicta maiores atque architecto repudiandae? Accusamus in, alias odit deleniti fugit earum eaque perspiciatis hic officia corporis laborum nam. At incidunt placeat culpa aliquid provident."
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Note);
    }

    [Fact]
    [Trait("WorkoutTracking", "Note")]
    public async Task WhenNoteIsNull_ShouldNotFail()
    {
        var command = new StartWorkoutTrackingCommand
        {
            Note = null
        };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Note);
    }
}
