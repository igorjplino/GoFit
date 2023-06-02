﻿using GoFit.Application.Exercises.Dtos;
using GoFit.Application.Interfaces;
using GoFit.Domain.Entities;
using MediatR;

namespace GoFit.Application.Exercises.Queries;

public record GetExerciseDtoByIdQuery : IRequest<ExerciseDto?>
{
    public Guid Id { get; set; }
}

internal class GetExerciseDtoByIdQueryHandler : IRequestHandler<GetExerciseDtoByIdQuery, ExerciseDto?>
{
    private readonly IExerciseRepository _exerciseRepository;

    public GetExerciseDtoByIdQueryHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    Task<ExerciseDto?> IRequestHandler<GetExerciseDtoByIdQuery, ExerciseDto?>.Handle(GetExerciseDtoByIdQuery request, CancellationToken cancellationToken)
    {
        Exercise? exercise = _exerciseRepository.Get(request.Id);

        if (exercise is null)
            return Task.FromResult(null as ExerciseDto);

        var exerciseDto = new ExerciseDto
        {
            Name = exercise.Name,
            Description = exercise.Description
        };

        return Task.FromResult<ExerciseDto?>(exerciseDto);
    }
}