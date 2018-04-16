public abstract class ValueObject<T> where T : ValueObject<T>
{
    public override bool Equals(object obj)
    {
        var valueObject = obj as T;

        if (valueObject is null)
            return false;

        return EqualsCore(valueObject);
    }

    public override int GetHashCode() => GetHashCodeCore();

    protected abstract bool EqualsCore(T other);

    protected abstract int GetHashCodeCore();

    public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject<T> a, ValueObject<T> b) => !(a == b);
}

[TestFixture]
public class ValueObjectShould
{
    [Test]
    public void CompareValueToNullCorrectlyUsingEquals()
    {
        var a = new ValueObjectStub(1, "Test");
        ValueObjectStub b = null;

        var result = a.Equals(b);

        Assert.False(result);
    }

    [Test]
    public void CompareValueToNullCorrectlyUsingEqualOperator()
    {
        var a = new ValueObjectStub(1, "Test");
        ValueObjectStub b = null;

        var result = a == b;

        Assert.False(result);
    }

    [Test]
    public void CompareNullValuesCorrectlyUsingEqualOperator()
    {
        ValueObjectStub a = null;
        ValueObjectStub b = null;

        var result = a == b;

        Assert.True(result);
    }

    [Test]
    public void CompareValueToNullCorrectlyUsingNotEqualOperator()
    {
        var a = new ValueObjectStub(1, "Test");
        ValueObjectStub b = null;

        var result = a != b;

        Assert.True(result);
    }

    [Test]
    public void CompareNullValuesCorrectlyUsingNotEqualOperator()
    {
        ValueObjectStub a = null;
        ValueObjectStub b = null;

        var result = a != b;

        Assert.False(result);
    }
}

public class ValueObjectStub : ValueObject<ValueObjectStub>
{
    public int ValueA { get; }
    public string ValueB { get; }

    public ValueObjectStub(int a, string b)
    {
        ValueA = a;
        ValueB = b;
    }

    protected override bool EqualsCore(ValueObjectStub other)
    {
        return ValueA == other.ValueA
            && ValueB == other.ValueB;
    }

    protected override int GetHashCodeCore()
    {
        int hash = 521;
        hash = (hash * 467) ^ ValueA.GetHashCode();
        hash = (hash * 467) ^ ValueB.GetHashCode();

        return hash;
    }
}