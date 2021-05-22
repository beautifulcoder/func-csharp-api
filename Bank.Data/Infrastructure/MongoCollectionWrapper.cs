using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Bank.Data.Infrastructure
{
  public class MongoCollectionWrapper<T> : IMongoCollectionWrapper<T>
  {
    private readonly IMongoCollection<T> _collection;

    public MongoCollectionWrapper(IMongoCollection<T> collection)
    {
      _collection = collection;
    }

    public Task<T> FindAsync(
      Expression<Func<T, bool>> filter,
      FindOptions options = null) =>
      _collection
        .Find(filter)
        .FirstOrDefaultAsync();

    public Task<T> FindOneAndUpdateAsync(
      Expression<Func<T, bool>> filter,
      UpdateDefinition<T> update,
      FindOneAndUpdateOptions<T, T> options = null,
      CancellationToken cancellationToken = default) =>
      _collection
        .FindOneAndUpdateAsync(
          filter,
          update,
          options,
          cancellationToken
        );

    public Task InsertOneAsync(
      T document,
      InsertOneOptions options,
      CancellationToken cancellationToken = default) =>
      _collection
        .InsertOneAsync(
          document,
          options,
          cancellationToken
        );
  }
}
