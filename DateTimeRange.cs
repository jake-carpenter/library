public class DateTimeRange : ValueObject<DateTimeRange>
{
    public DateTimeOffset Start { get; }
    public DateTimeOffset End { get; }

    public DateTimeRange(DateTimeOffset start, DateTimeOffset end)
    {
        var startUtc = start.ToUniversalTime();
        var endUtc = end.ToUniversalTime();

        if (startUtc > endUtc)
            throw new Exception("Invalid date range.");

        Start = startUtc;
        End = endUtc;
    }

    public DateTimeRange(DateTimeOffset start, TimeSpan duration)
        : this(start, start.Add(duration))
    {
    }

    protected override bool EqualsCore(DateTimeRange other)
    {
        return Start == other.Start
            && End == other.End;
    }

    protected override int GetHashCodeCore()
    {
        var hash = 599;
        hash = (hash * 379) ^ Start.GetHashCode();
        return (hash * 379) ^ End.GetHashCode();
    }
}

[TestFixture]
public class DateTimeRangeShould
{
    [Test]
    public void SetsStartAsUtc()
    {
        var offset = new TimeSpan(-6, 0, 0);
        var start = new DateTimeOffset(2000, 1, 1, 12, 0, 0, offset);
        var end = new DateTimeOffset(2000, 1, 1, 13, 0, 0, offset);

        var sut = new DateTimeRange(start, end);

        Assert.That(sut.Start, Is.EqualTo(start.ToUniversalTime()));
    }

    [Test]
    public void SetsEndAsUtc()
    {
        var offset = new TimeSpan(-6, 0, 0);
        var start = new DateTimeOffset(2000, 1, 1, 12, 0, 0, offset);
        var end = new DateTimeOffset(2000, 1, 1, 13, 0, 0, offset);

        var sut = new DateTimeRange(start, end);

        Assert.That(sut.End, Is.EqualTo(end.ToUniversalTime()));
    }

    [TestCase(2, 1, -7, -7)]
    [TestCase(2, 2, -7, -6)]
    public void ThrowInvalidDateTimeRangeWhenStartDateIsBeforeEndDate(int startHour, int endHour, int startOffsetHour,
        int endOffsetHour)
    {
        var startOffset = new TimeSpan(startOffsetHour, 0, 0);
        var endOffset = new TimeSpan(endOffsetHour, 0, 0);
        var start = new DateTimeOffset(2000, 1, 1, startHour, 0, 0, startOffset);
        var end = new DateTimeOffset(2000, 1, 1, endHour, 59, 59, endOffset);

        Assert.Throws<Exception>(() => new DateTimeRange(start, end));
    }

    [Test]
    public void ConvertsDurationToEndDate()
    {
        var offset = new TimeSpan(-6, 0, 0);
        var start = new DateTimeOffset(2000, 1, 1, 12, 0, 0, offset);
        var duration = new TimeSpan(1, 0, 0); // 1 hour

        var sut = new DateTimeRange(start, duration);

        Assert.That(sut.End, Is.EqualTo(start.Add(duration)));
    }

    [Test]
    public void CompareCorrectlyToAnEqualDateTimeRange()
    {
        var nowTicks = DateTimeOffset.UtcNow.Ticks;
        var offset = DateTimeOffset.UtcNow.Offset;
        var a = new DateTimeRange(
            new DateTimeOffset(nowTicks, offset),
            new DateTimeOffset(nowTicks + 1, offset));
        var b = new DateTimeRange(
            new DateTimeOffset(nowTicks, offset),
            new DateTimeOffset(nowTicks + 1, offset));

        var result = a == b;

        Assert.True(result);
    }

    [Test]
    public void CompareCorrectlyToAnUnEqualDateTimeRange()
    {
        var nowTicks = DateTimeOffset.UtcNow.Ticks;
        var offset = DateTimeOffset.UtcNow.Offset;
        var a = new DateTimeRange(
            new DateTimeOffset(nowTicks, offset),
            new DateTimeOffset(nowTicks + 1, offset));
        var b = new DateTimeRange(
            new DateTimeOffset(nowTicks, offset),
            new DateTimeOffset(nowTicks + 2, offset));

        var result = a == b;

        Assert.False(result);
    }

    [Test]
    public void CompareCorrectlyToANullDateTimeRange()
    {
        var nowTicks = DateTimeOffset.UtcNow.Ticks;
        var offset = DateTimeOffset.UtcNow.Offset;
        var a = new DateTimeRange(
            new DateTimeOffset(nowTicks, offset),
            new DateTimeOffset(nowTicks + 1, offset));
        DateTimeRange b = null;

        var result = a == b;

        Assert.False(result);
    }

    [Test]
    public void ReturnEqualHashForIdenticalValues()
    {
        var nowTicks = DateTimeOffset.UtcNow.Ticks;
        var offset = DateTimeOffset.UtcNow.Offset;
        var a = new DateTimeRange(
            new DateTimeOffset(nowTicks, offset),
            new DateTimeOffset(nowTicks + 1, offset));
        var b = new DateTimeRange(
            new DateTimeOffset(nowTicks, offset),
            new DateTimeOffset(nowTicks + 1, offset));

        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    [Test]
    public void ReturnDifferentHashForDifferentValues()
    {
        var nowTicks = DateTimeOffset.UtcNow.Ticks;
        var offset = DateTimeOffset.UtcNow.Offset;
        var a = new DateTimeRange(
            new DateTimeOffset(nowTicks, offset),
            new DateTimeOffset(nowTicks + 1, offset));
        var b = new DateTimeRange(
            new DateTimeOffset(nowTicks, offset),
            new DateTimeOffset(nowTicks + 2, offset));

        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }
}