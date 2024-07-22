using AutoFixture.Xunit2;

namespace SaanSoft.Tests.CorrelationId.Common.AutoFixture;

[AttributeUsage(AttributeTargets.Method)]
public class AutoFakeDataAttribute() : AutoDataAttribute(AutoFixtureExtensions.Create);
