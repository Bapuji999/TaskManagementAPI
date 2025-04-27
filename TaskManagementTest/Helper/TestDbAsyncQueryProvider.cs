using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Yrefy.UnitTest.Helper
{
  public class TestDbAsyncQueryProvider<T> : IAsyncQueryProvider
  {
    private readonly IQueryProvider _inner;

    public TestDbAsyncQueryProvider(IQueryProvider inner)
    {
      _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
    {
      return new TestDbAsyncEnumerable<T>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
      return new TestDbAsyncEnumerable<TElement>(expression);
    }

    public object Execute(Expression expression)
    {
      return _inner.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
      return _inner.Execute<TResult>(expression);
    }

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
      var expectedResultType = typeof(TResult).GetGenericArguments()[0];
      var executionResult = typeof(IQueryProvider)
                           .GetMethod(
                                name: nameof(IQueryProvider.Execute),
                                genericParameterCount: 1,
                                types: new[] { typeof(Expression) })
                           .MakeGenericMethod(expectedResultType)
                           .Invoke(this, new[] { expression });

      return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
                                  ?.MakeGenericMethod(expectedResultType)
                                   .Invoke(null, new[] { executionResult });
    }
  }
}
