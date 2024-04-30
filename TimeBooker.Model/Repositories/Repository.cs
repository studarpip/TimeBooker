using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace TimeBooker.Model.Repositories;

public class Repository : IRepository
{
    private readonly IConfiguration _configuration;

	public Repository(
		IConfiguration configuration
		)
	{
		_configuration = configuration;
	}

    public async Task<IEnumerable<TRes>> QueryListAsync<TRes, T>(string command, T parameters)
    {
        using IDbConnection connection = new MySqlConnection(_configuration.GetConnectionString("Database"));
        return await connection.QueryAsync<TRes>(command, parameters, commandType: CommandType.Text);
    }

    public async Task<TRes> QueryAsync<TRes, T>(string command, T parameters)
    {
        using IDbConnection connection = new MySqlConnection(_configuration.GetConnectionString("Database"));
        return await connection.QuerySingleOrDefaultAsync<TRes>(command, parameters, commandType: CommandType.Text);
    }

    public async Task ExecuteAsync<T>(string command, T parameters)
    {
        using IDbConnection connection = new MySqlConnection(_configuration.GetConnectionString("Database"));
        await connection.ExecuteAsync(command, parameters, commandType: CommandType.Text);
    }
}