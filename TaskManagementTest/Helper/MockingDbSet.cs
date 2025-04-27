using Microsoft.EntityFrameworkCore;
using Moq;

namespace Yrefy.UnitTest.Helper
{
  public class MockingDbSet
  {
    public static Mock<DbSet<T>> GetQueryableMockDbSet<T>(IQueryable<T> data) where T : class
    {
      var mockSet = new Mock<DbSet<T>>();
      mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<T>(data.Provider));
      mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
      mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
      mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
      mockSet.As<IAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));
      return mockSet;
    }
  }
}
