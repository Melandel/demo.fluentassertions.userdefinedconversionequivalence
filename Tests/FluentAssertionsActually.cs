using FluentAssertions;

namespace Tests;

public class Tests
{
	Output _output1;
	Output _output2;
	readonly Output OutputBase = new Output(
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

	[SetUp]
	public void SetUp()
	{
		_output1 = OutputBase;
		_output2 = OutputBase with { SomeEncapsulatedId = SomeEncapsulatedId.CreateUnique() }; // Different SomeEncapsulatedId
	}

	[Test]
	public void FailsToFindDifferencesBetweenOutputs_When_NoConfigurationIsGiven()
	{
		var whatIWishWasTrue = () => { _output2.Should().NotBeEquivalentTo(_output1); };

		Assert.That(
			whatIWishWasTrue,
			Throws.Exception
				.With.Message.Contain("Expected _output2 not to be equivalent to Output")
				.With.Message.Contain(", but they are.")
		);
	}

	[Test]
	public void FailsToFindDifferencesBetweenEncapsulatedIDs_When_Comparing_SomeEncapsulatedId_ByMember()
	{

		var someEncapsulatedId1 = _output1.SomeEncapsulatedId;
		var someEncapsulatedId2 = _output2.SomeEncapsulatedId;

		var whatDennisDoomenSuggested = () => { // // https://github.com/fluentassertions/fluentassertions/issues/2016#issuecomment-1279928376
			someEncapsulatedId2.Should().NotBeEquivalentTo(
				someEncapsulatedId1,
				options => options.ComparingByMembers<SomeEncapsulatedId>()
			); };

		Assert.That(
			whatDennisDoomenSuggested,
			Throws.TypeOf<InvalidOperationException>()
				.With.Message.Contain("No members were found for comparison. Please specify some members to include in the comparison or choose a more meaningful assertion.")
		);
	}

	[Test]
	public void FailsToFindDifferencesBetweenOutputs_When_Comparing_SomeEncapsulatedId_ByMember()
	{
		var whatIWishWasTrue = () => {
			_output2.Should().NotBeEquivalentTo(
				_output1,
				options => options.ComparingByMembers<SomeEncapsulatedId>()
			);
		};
		Assert.That(
			whatIWishWasTrue,
			Throws.Exception
				.With.Message.Contain("Expected _output2 not to be equivalent to Output")
				.With.Message.Contain(", but they are.")
		);
	}
}
