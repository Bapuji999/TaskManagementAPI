namespace Yrefy.UnitTest.Helper
{
  public class TestDbAsyncEnumerator<T> : IAsyncEnumerator<T>
  {
    private readonly IEnumerator<T> _innerEnumerator;

    public TestDbAsyncEnumerator(IEnumerator<T> innerEnumerator)
    {
      _innerEnumerator = innerEnumerator;
    }

    public T Current => _innerEnumerator.Current;

    public ValueTask DisposeAsync() => new ValueTask(Task.CompletedTask);

    public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_innerEnumerator.MoveNext());
  }
}
