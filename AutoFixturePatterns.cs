public class TestCustomizationsAttribute : AutoDataAttribute
{
    public TestCustomizationsAttribute(params Type[] customizationTypes)
        : base(() => InitializeFixture(customizationTypes))
    {
    }

    private static IFixture InitializeFixture(params Type[] customizationTypes)
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoMoqCustomization());

        foreach (var type in customizationTypes)
        {
            var customization = (ICustomization)Activator.CreateInstance(type);
            fixture.Customize(customization);
        }

        return fixture;
    }
}

[Theory]
[TestCustomizations(typeof(AccountHasAccessCustomization))]
public void FlipsTheFakeFlag(CartService sut)
{
    var cart = sut.Get(1);
    Assert.True(cart.FakeProperty);
}
