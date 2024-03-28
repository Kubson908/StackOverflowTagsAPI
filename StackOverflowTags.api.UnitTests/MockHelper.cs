using Microsoft.EntityFrameworkCore;
using Moq;
using StackOverflowTags.api.Model;

namespace StackOverflowTags.api.UnitTests;

public static class MockHelper
{
    public static Mock<DbSet<T>>GetMockDbSet<T>(IQueryable<T> data = null) where T : class
    {
        var mockDbSet = new Mock<DbSet<T>>();
        if (data != null)
        {
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
        }
        return mockDbSet;
    }
}
