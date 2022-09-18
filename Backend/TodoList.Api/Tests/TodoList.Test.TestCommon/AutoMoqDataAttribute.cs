using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace TodoList.Test.TestCommon;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute()
        : base(Factory)
    {
    }

    private static IFixture Factory()
    {
        var fixture = new Fixture();
        var customization = new AutoMoqCustomization
        {
            ConfigureMembers = true
        };
        fixture.Customize(customization);
        return fixture;
    }
}
