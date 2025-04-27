using System.Linq.Expressions;

namespace Yrefy.UnitTest.Helper
{
  public class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
  {
    public TestDbAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
    {
    }

    public TestDbAsyncEnumerable(Expression expression) : base(expression)
    {
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
      return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    IQueryProvider IQueryable.Provider => new TestDbAsyncQueryProvider<T>(this);
  }
}
