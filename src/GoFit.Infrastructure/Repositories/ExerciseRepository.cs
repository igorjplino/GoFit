using Dapper;
using GoFit.Application.EntitiesActions.Exercises.Queries;
using GoFit.Application.Interfaces;
using GoFit.Application.Models;
using GoFit.Domain.Entities;
using GoFit.Infrastructure.Contexts;
using GoFit.Infrastructure.Contexts.GoFitDb;
using GoFit.Infrastructure.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GoFit.Infrastructure.Repositories;
public class ExerciseRepository : BaseRepository<Exercise>, IExerciseRepository
{
    private readonly string _goFitDb;

    public ExerciseRepository(
        GoFitDbContext context,
        IOptions<Connections> options)
        : base(context)
    {
        _goFitDb = options.Value.GoFitDb;
    }

    public async Task<Exercise?> GetByNameAsync(string name)
    {
        return await Context.Set<Exercise>().FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<(IEnumerable<Exercise> Exercises, int Total)> ListExercisesAsync(GetAllExercisesQuery filters)
    {
        var sql = @"
            SELECT *, COUNT(*) OVER() AS Total
            FROM Exercises
            WHERE 1=1";

        if (!string.IsNullOrWhiteSpace(filters.Name))
        {
            sql += " AND Name LIKE @Name";
        }
        
        sql += @" 
            ORDER BY Name
            OFFSET @Offset ROWS 
            FETCH NEXT @PageSize ROWS ONLY";
        
        var param = new
        {
            Offset = (filters.PageNumber - 1) * filters.PageSize,
            PageSize = filters.PageSize,
            Name = $"%{filters.Name}%",
        };

        using var cnn = new SqlConnection(_goFitDb);
        
        var result = await cnn.QueryAsync<Exercise, int, (Exercise Exercise, int Total)>(
            sql: sql,
            param: param,
            splitOn: "Total",
            map: (exercise, total) => (exercise, total));
        
        var exercises = result.Select(r => r.Exercise);
        var total = result.FirstOrDefault().Total;

        return (exercises, total);
    }
}
