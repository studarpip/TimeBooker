using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeBooker.Model.Repositories;

public interface IRepository
{
    Task<IEnumerable<TRes>> QueryListAsync<TRes, T>(string command, T parameters);
    Task<TRes> QueryAsync<TRes, T>(string command, T parameters);
    Task ExecuteAsync<T>(string command, T parameters);
}