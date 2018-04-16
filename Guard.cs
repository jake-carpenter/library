/// <summary>
/// Guard clause definition.
/// </summary>
public interface IGuardAgainst
{
}

/// <summary>
/// Set of Guard clause helpers provided in a fluent interface.
/// </summary>
public class Guard : IGuardAgainst
{
    private Guard()
    {
    }

    /// <summary>
    /// Set of Guard clause helpers provided in a fluent interface.
    /// </summary>
    public static IGuardAgainst Against { get; set; } = new Guard();
}

public static class GuardAgainstExtensions
{
    /// <summary>
    /// Guard against null.
    /// </summary>
    /// <param name="guard">Guard clause.</param>
    /// <param name="value">Id to guard.</param>
    /// <param name="name">Name to provide as parameter for exception (optional).</param>
    public static void Null(this IGuardAgainst guard, object value, string name = null)
    {
        if (value != null) return;
        throw new ArgumentNullException(name);
    }

    /// <summary>
    /// Guard against null or empty string.
    /// </summary>
    /// <param name="guard">Guard clause.</param>
    /// <param name="value">Id to guard.</param>
    /// <param name="name">Name to provide as parameter for exception (optional).</param>
    public static void EmptyString(this IGuardAgainst guard, string value, string name = null)
    {
        Guard.Against.Null(value, name);
        if (!string.IsNullOrWhiteSpace(value)) return;
        throw new ArgumentException("Required input was empty.", name);
    }

    /// <summary>
    /// Guard against integer value outside of provided range.
    /// </summary>
    /// <param name="guard">Guard clause.</param>
    /// <param name="value">Id to guard.</param>
    /// <param name="name">Name to provide as parameter for exception (optional).</param>
    public static void OutOfRange(this IGuardAgainst guard, int value, int min, int max,
        string name = null)
    {
        if (value >= min && value <= max) return;
        throw new ArgumentOutOfRangeException(name, "Input value was out or range.");
    }

    /// <summary>
    /// Guard against integer value outside of provided range.
    /// </summary>
    /// <param name="guard">Guard clause.</param>
    /// <param name="value">Id to guard.</param>
    /// <param name="name">Name to provide as parameter for exception (optional).</param>
    public static void OutOfRange(this IGuardAgainst guard, uint value, uint min, uint max,
        string name = null)
    {
        if (value >= min && value <= max) return;
        throw new ArgumentOutOfRangeException(name, "Input value was out or range.");
    }
}

public class GuardAgainstOutOfRangeUIntShould
{
    [Fact]
    void ThrowArgumentOutOfRange_WhenValueIsBelowMin()
    {
        // Arrange
        const uint min = 1;
        const uint max = 5;
        const uint value = 0;

        // Act
        var exception = Record.Exception(() => Guard.Against.OutOfRange(value, min, max));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Fact]
    void ThrowArgumentOutOfRange_WhenValueIsAboveMax()
    {
        // Arrange
        const uint min = 1;
        const uint max = 5;
        const uint value = 6;

        // Act
        var exception = Record.Exception(() => Guard.Against.OutOfRange(value, min, max));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Fact]
    void IncludeParamNameInException_WhenNameIsProvided()
    {
        // Arrange
        const uint min = 1;
        const uint max = 5;
        const uint value = 0;

        // Act
        var exception = Record.Exception(() =>
            Guard.Against.OutOfRange(value, min, max, nameof(value))
        );

        // Assert
        Assert.NotNull(exception);
        var outOfRangeException = Assert.IsType<ArgumentOutOfRangeException>(exception);
        Assert.Equal(nameof(value), outOfRangeException.ParamName);
    }

    [Fact]
    void NotIncludeParamNameInException_WhenNameIsNotProvided()
    {
        // Arrange
        const uint min = 1;
        const uint max = 5;
        const uint value = 0;

        // Act
        var exception = Record.Exception(() => Guard.Against.OutOfRange(value, min, max));

        // Assert
        Assert.NotNull(exception);
        var outOfRangeException = Assert.IsType<ArgumentOutOfRangeException>(exception);
        Assert.Null(outOfRangeException.ParamName);
    }

    [Fact]
    void NotThrow_WhenValueIsWithinRange()
    {
        // Arrange
        const uint min = 1;
        const uint max = 5;
        const uint value = 3;

        // Act
        var exception = Record.Exception(() => Guard.Against.OutOfRange(value, min, max));

        // Assert
        Assert.Null(exception);
    }
}

public class GuardAgainstEmptyStringShould
{
    [Fact]
    void ThrowArgument_WhenInputIsEmpty()
    {
        // Arrange
        var str = string.Empty;

        // Act
        var exception = Record.Exception(() => Guard.Against.EmptyString(str));

        // Assert
        Assert.NotNull(exception);
        var argumentException = Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    void ThrowArgumentNull_WhenInputIsNull()
    {
        // Arrange
        string str = null;

        // Act
        var exception = Record.Exception(() => Guard.Against.EmptyString(str));

        // Assert
        Assert.NotNull(exception);
        var argumentNullException = Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    void IncludeParamNameInException_WhenNameIsProvided()
    {
        // Arrange
        string str = string.Empty;

        // Act
        var exception = Record.Exception(() => Guard.Against.EmptyString(str, nameof(str)));

        // Assert
        Assert.NotNull(exception);
        var argumentException = Assert.IsType<ArgumentException>(exception);
        Assert.Equal(nameof(str), argumentException.ParamName);
    }

    [Fact]
    void NotIncludeParamNameInException_WhenParamNameIsNotProvided()
    {
        // Arrange
        var str = string.Empty;

        // Act
        var exception = Record.Exception(() => Guard.Against.EmptyString(str));

        // Assert
        Assert.NotNull(exception);
        var argumentException = Assert.IsType<ArgumentException>(exception);
        Assert.Null(argumentException.ParamName);
    }

    [Fact]
    void NotThrow_WhenValueIsNotEmptyOrNull()
    {
        // Arrange
        var str = "test string";

        // Act
        var exception = Record.Exception(() => Guard.Against.EmptyString(str));

        // Assert
        Assert.Null(exception);
    }
}

public class GuardAgainstNullShould
{
    [Fact]
    void ThrowArgumentNull_WhenInputIsNull()
    {
        // Arrange
        object obj = null;

        // Act
        var exception = Record.Exception(() => Guard.Against.Null(obj));

        // Assert
        Assert.NotNull(exception);
        var argumentNullException = Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    void IncludeParamNameInException_WhenNameIsProvided()
    {
        // Arrange
        object obj = null;

        // Act
        var exception = Record.Exception(() => Guard.Against.Null(obj, nameof(obj)));

        // Assert
        Assert.NotNull(exception);
        var argumentNullException = Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal(nameof(obj), argumentNullException.ParamName);
    }

    [Fact]
    void NotIncludeParamNameInException_WhenParamNameIsNotProvided()
    {
        // Arrange
        object obj = null;

        // Act
        var exception = Record.Exception(() => Guard.Against.Null(obj));

        // Assert
        Assert.NotNull(exception);
        var argumentNullException = Assert.IsType<ArgumentNullException>(exception);
        Assert.Null(argumentNullException.ParamName);
    }

    [Fact]
    void NotThrow_WhenValueIsNotNull()
    {
        // Arrange
        var obj = new object();

        // Act
        var exception = Record.Exception(() => Guard.Against.Null(obj));

        // Assert
        Assert.Null(exception);
    }
}

public class GuardAgainstOutOfRangeIntShould
{
    [Fact]
    void ThrowArgumentOutOfRange_WhenValueIsBelowMin()
    {
        // Arrange
        const int min = 1;
        const int max = 5;
        const int value = 0;

        // Act
        var exception = Record.Exception(() => Guard.Against.OutOfRange(value, min, max));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Fact]
    void ThrowArgumentOutOfRange_WhenValueIsAboveMax()
    {
        // Arrange
        const int min = 1;
        const int max = 5;
        const int value = 6;

        // Act
        var exception = Record.Exception(() => Guard.Against.OutOfRange(value, min, max));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Fact]
    void IncludeParamNameInException_WhenNameIsProvided()
    {
        // Arrange
        const int min = 1;
        const int max = 5;
        const int value = 0;

        // Act
        var exception = Record.Exception(() =>
            Guard.Against.OutOfRange(value, min, max, nameof(value))
        );

        // Assert
        Assert.NotNull(exception);
        var outOfRangeException = Assert.IsType<ArgumentOutOfRangeException>(exception);
        Assert.Equal(nameof(value), outOfRangeException.ParamName);
    }

    [Fact]
    void NotIncludeParamNameInException_WhenNameIsNotProvided()
    {
        // Arrange
        const int min = 1;
        const int max = 5;
        const int value = 0;

        // Act
        var exception = Record.Exception(() => Guard.Against.OutOfRange(value, min, max));

        // Assert
        Assert.NotNull(exception);
        var outOfRangeException = Assert.IsType<ArgumentOutOfRangeException>(exception);
        Assert.Null(outOfRangeException.ParamName);
    }

    [Fact]
    void NotThrow_WhenValueIsWithinRange()
    {
        // Arrange
        const int min = 1;
        const int max = 5;
        const int value = 3;

        // Act
        var exception = Record.Exception(() => Guard.Against.OutOfRange(value, min, max));

        // Assert
        Assert.Null(exception);
    }
}