public static class TimeContext
{
    private static readonly ThreadLocal<Func<DateTime>> Implementation
        = new ThreadLocal<Func<DateTime>>();

    public static DateTime UtcNow => Now.ToUniversalTime();

    public static DateTime Now
    {
        get
        {
            if (Implementation.Value == null)
                Reset();

            return Implementation.Value();
        }
    }

    public static void Init(Func<DateTime> implementation)
    {
        Implementation.Value = implementation
            ?? throw new InvalidOperationException($"Implementation of {nameof(TimeContext)} not provided");
    }

    public static void Reset()
    {
        Implementation.Value = () => DateTime.Now;
    }
}


public class TimeContextShould
{
    [Fact]
    private void AllowInitialization()
    {
        var expected = new DateTime(2000, 1, 1);
        TimeContext.Init(() => expected);

        var actual = TimeContext.Now;

        Assert.Equal(expected, actual);
    }

    [Fact]
    private void AllowResettingInitalization()
    {
        var fakeTime = new DateTime(1900, 1, 1);
        TimeContext.Init(() => fakeTime);

        var fakeTimeResult = TimeContext.Now;
        TimeContext.Reset();
        var resetTimeResult = TimeContext.Now;

        Assert.NotEqual(fakeTimeResult.Year, resetTimeResult.Year);
    }

    [Fact]
    private void NotProvideStaticTimeByDefault()
    {
        var realTime = DateTime.Now;
        Thread.Sleep(2);
        var contextTime = TimeContext.Now;

        Assert.True(realTime < contextTime);
    }

    [Fact]
    private void AllowReturningUtcTime()
    {
        var fakeTime = new DateTime(2000, 1, 1);
        TimeContext.Init(() => fakeTime);

        var actual = TimeContext.UtcNow;

        Assert.Equal(fakeTime.ToUniversalTime(), actual);
    }

    [Fact]
    private void ThrowInvalidOperation_WhenInitImplementationIsNull()
    {
        var exception = Record.Exception(() => TimeContext.Init(null));

        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    private void ProvideLocalValuesAcrossThreads()
    {
        var expectedA = DateTime.MinValue;
        var expectedB = DateTime.MaxValue;
        var actualA = DateTime.Today;
        var actualB = DateTime.Today;

        var threadA = new Thread(() =>
        {
            TimeContext.Init(() => expectedA);
            actualA = TimeContext.Now;
        });

        var threadB = new Thread(() =>
        {
            TimeContext.Init(() => expectedB);
            actualB = TimeContext.Now;
        });

        threadA.Start();
        threadB.Start();
        threadA.Join();
        threadB.Join();

        Assert.Equal(expectedA, actualA);
        Assert.Equal(expectedB, actualB);
    }
}