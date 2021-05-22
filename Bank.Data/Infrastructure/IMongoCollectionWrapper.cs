using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Bank.Data.Infrastructure
{
  public interface IMongoCollectionWrapper<T>
  {
    Task<T> FindAsync(
      Expression<Func<T, bool>> filter,
      FindOptions options = null
    );

    Task<T> FindOneAndUpdateAsync(
      Expression<Func<T, bool>> filter,
      UpdateDefinition<T> update,
      FindOneAndUpdateOptions<T, T> options = null,
      CancellationToken cancellationToken = default
    );

    Task InsertOneAsync(
      T document,
      InsertOneOptions options = null,
      CancellationToken cancellationToken = default
    );
  }
}
