using FluentAssertions;

namespace Tests;

public class Tests
{
	static readonly RootObject RootObjectTemplate = new RootObject(
		SomeEncapsulatedId.CreateUnique(),
		new[] { PositiveInteger.CreateFromInteger(1), PositiveInteger.CreateFromInteger(2) },
		new Dictionary<SomeEnum, NegativeInteger[]>
		{
			{ SomeEnum.Value,        new[] { NegativeInteger.CreateFromInteger(-1), NegativeInteger.CreateFromInteger(-2) } },
			{ SomeEnum.AnotherValue, new[] { NegativeInteger.CreateFromInteger(-3), NegativeInteger.CreateFromInteger(-4) } },
		},
		new SomeObjectContainingPositiveIntegerProperties(
			PositiveInteger.CreateFromInteger(3),
			NegativeInteger.CreateFromInteger(-5)
		)
	);

	[Test]
	public void FailsToFindDifferencesBetweenOutputs_When_NoConfigurationIsGiven()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with { SomeEncapsulatedId = SomeEncapsulatedId.CreateUnique() };
		var whatIWishWasTrue = () => { o2.Should().NotBeEquivalentTo(o1); };

		// Act & Assert
		Assert.That(
			whatIWishWasTrue,
			Throws.Exception
				.With.Message.Contain("Expected o2 not to be equivalent to RootObject")
				.With.Message.Contain(", but they are.")
		);
	}

	[Test]
	public void FailsToFindDifferencesBetweenEncapsulatedIDs_When_Comparing_SomeEncapsulatedId_ByMember()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with { SomeEncapsulatedId = SomeEncapsulatedId.CreateUnique() };
		var id1 = o1.SomeEncapsulatedId;
		var id2 = o2.SomeEncapsulatedId;
		var whatDennisDoomenSuggested = () => { // https://github.com/fluentassertions/fluentassertions/issues/2016#issuecomment-1279928376
			id2.Should().NotBeEquivalentTo(
				id1,
				options => options.ComparingByMembers<SomeEncapsulatedId>()
			); };

		// Act & Assert
		Assert.That(
			whatDennisDoomenSuggested,
			Throws.TypeOf<InvalidOperationException>()
				.With.Message.Contain("No members were found for comparison. Please specify some members to include in the comparison or choose a more meaningful assertion.")
		);
	}

	[Test]
	public void FailsToFindDifferencesBetweenOutputs_When_Comparing_SomeEncapsulatedId_ByMember()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with { SomeEncapsulatedId = SomeEncapsulatedId.CreateUnique() };
		var whatIWishWasTrue = () => {
			o2.Should().NotBeEquivalentTo(
				o1,
				options => options.ComparingByMembers<SomeEncapsulatedId>()
			);
		};

		// Act & Assert
		Assert.That(
			whatIWishWasTrue,
			Throws.Exception
				.With.Message.Contain("Expected o2 not to be equivalent to RootObject")
				.With.Message.Contain(", but they are.")
		);
	}
}
