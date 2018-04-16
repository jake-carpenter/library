public abstract class Entity<TId>
{
    public TId Id { get; }

    public ICollection<IDomainEvent> Events { get; }

    protected Entity(TId id)
    {
        if (object.Equals(id, default(TId)))
            throw new ArgumentException("Entity id cannot be the default value for type.");

        Id = id;
    }

    public override bool Equals(object obj)
    {
        var entity = obj as Entity<TId>;

        if (entity is null)
            return false;

        if (ReferenceEquals(this, entity))
            return true;

        return Id.Equals(entity.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity<TId> a, Entity<TId> b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TId> a, Entity<TId> b) => !(a == b);
}

[TestFixture]
public class EntityShould
{
    #region Equals Tests

    [Test]
    public void CompareEqualValuesCorrectlyUsingEquals()
    {
        var a = new EntityStub(1);
        var b = new EntityStub(1);

        var result = a.Equals(b);

        Assert.True(result);
    }

    [Test]
    public void CompareReferenceValuesCorrectlyUsingEquals()
    {
        var a = new EntityStub(1);
        var b = a;

        var result = a.Equals(b);

        Assert.True(result);
    }

    [Test]
    public void CompareUnequalValuesCorrecltyUsingEquals()
    {
        var a = new EntityStub(1);
        var b = new EntityStub(2);

        var result = a.Equals(b);

        Assert.False(result);
    }

    [Test]
    public void CompareValueToNullCorrectlyUsingEquals()
    {
        var a = new EntityStub(1);
        EntityStub b = null;

        var result = a.Equals(b);

        Assert.False(result);
    }

    [Test]
    public void ThrowArgumentWhenIdIsTypeDefaultValue()
    {
        Assert.Throws<ArgumentException>(() => new EntityStub(0));
    }

    #endregion

    #region == Tests

    [Test]
    public void CompareEqualValuesCorrectlyUsingEqualOperator()
    {
        var a = new EntityStub(1);
        var b = new EntityStub(1);

        var result = a == b;

        Assert.True(result);
    }

    [Test]
    public void CompareReferenceValuesCorrectlyUsingEqualOperator()
    {
        var a = new EntityStub(1);
        var b = a;

        var result = a == b;

        Assert.True(result);
    }

    [Test]
    public void CompareUnequalValuesCorrectlyUsingEqualOperator()
    {
        var a = new EntityStub(1);
        var b = new EntityStub(2);

        var result = a == b;

        Assert.False(result);
    }

    [Test]
    public void CompareValueToNullCorrectlyUsingEqualOperator()
    {
        var a = new EntityStub(1);
        EntityStub b = null;

        var result = a == b;

        Assert.False(result);
    }

    [Test]
    public void CompareNullValuesCorrectlyUsingEqualOperator()
    {
        EntityStub a = null;
        EntityStub b = null;

        var result = a == b;

        Assert.True(result);
    }

    #endregion

    #region != Tests

    [Test]
    public void CompareEqualValuesCorrectlyUsingNotEqualOperator()
    {
        var a = new EntityStub(1);
        var b = new EntityStub(1);

        var result = a != b;

        Assert.False(result);
    }

    [Test]
    public void CompareReferenceValuesCorrectlyUsingNotEqualOperator()
    {
        var a = new EntityStub(1);
        var b = a;

        var result = a != b;

        Assert.False(result);
    }

    [Test]
    public void CompareUnequalValuesCorrectlyUsingNotEqualOperator()
    {
        var a = new EntityStub(1);
        var b = new EntityStub(2);

        var result = a != b;

        Assert.True(result);
    }

    [Test]
    public void CompareValueToNullCorrectlyUsingNotEqualOperator()
    {
        var a = new EntityStub(1);
        EntityStub b = null;

        var result = a != b;

        Assert.True(result);
    }

    [Test]
    public void CompareNullValuesCorrectlyUsingNotEqualOperator()
    {
        EntityStub a = null;
        EntityStub b = null;

        var result = a != b;

        Assert.False(result);
    }

    #endregion

    #region GetHashCode Tests

    [Test]
    public void ReturnEqualHashForIdenticalValues()
    {
        var a = new EntityStub(1);
        var b = new EntityStub(1);

        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    [Test]
    public void ReturnDifferentHashForDifferentValues()
    {
        var a = new EntityStub(1);
        var b = new EntityStub(2);

        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }

    #endregion
}

public class EntityStub : Entity<int>
{
    public EntityStub(int id) : base(id)
    {
    }
}