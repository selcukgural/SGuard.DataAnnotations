using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace SGuard.DataAnnotations.Tests.Attributes;

/// <summary>
/// The collection attributes never enumerate an <see cref="IQueryable"/>: that would run the query during validation.
/// </summary>
public sealed class CollectionQueryableTests
{
    public static TheoryData<ValidationAttribute> CollectionAttributes => new()
    {
        new SGuardMinCountAttribute(1, typeof(Resources.SGuardDataAnnotations), "Common_Collection_MinCount"),
        new SGuardMaxCountAttribute(1, typeof(Resources.SGuardDataAnnotations), "Common_Collection_MaxCount"),
        new SGuardRequiredCollectionAttribute(typeof(Resources.SGuardDataAnnotations), "Common_Collection_Required"),
        new SGuardCollectionItemsMatchAttribute(typeof(RequiredAttribute), typeof(Resources.SGuardDataAnnotations), "Username_Required"),
    };

    [Theory]
    [MemberData(nameof(CollectionAttributes))]
    public void Queryable_Throws_WithoutRunningTheQuery(ValidationAttribute attribute)
    {
        var query = new TrackingQueryable();
        var ctx = new ValidationContext(new object()) { MemberName = "Items" };

        var ex = Assert.Throws<InvalidOperationException>(() => attribute.GetValidationResult(query, ctx));

        Assert.Contains("IQueryable", ex.Message);
        Assert.False(query.Enumerated);
    }

    private sealed class TrackingQueryable : IQueryable<int>
    {
        private readonly IQueryable<int> _inner = new[] { 1, 2, 3 }.AsQueryable();

        public bool Enumerated { get; private set; }

        public Type ElementType => _inner.ElementType;

        public Expression Expression => _inner.Expression;

        public IQueryProvider Provider => _inner.Provider;

        public IEnumerator<int> GetEnumerator()
        {
            Enumerated = true;
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
