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
		_output2 = OutputBase with { SomeEncapsulatedId = SomeEncapsulatedId.CreateUnique() };
	}

	[Test]
	public void FaWithoutOptions_Should_FindEquivalence()
	{
		_output2.Should().BeEquivalentTo(_output1);
	}

	[Test]
	public void FaWithOptions_ComparingByMember_Still_Fails_To_Find_Difference_Between_The_Two_EncapsulatedIds()
	{
		Assert.That(
			() => {_output2.Should().NotBeEquivalentTo(_output1, options => options.ComparingByMembers<SomeEncapsulatedId>()); },
			Throws.Exception
				.With.Message.Contain("Expected _output2 not to be equivalent to Output")
				.With.Message.Contain(", but they are.")
		);
	}

	[Test]
	public void FaWithOptions_ComparingByMember_Applied_On_Property_SomeEncapsulatedId_Only_Still_Fails_To_Find_Difference_Between_The_Two_EncapsulatedIds()
	{
		var someEncapsulatedId1 = _output1.SomeEncapsulatedId;
		var someEncapsulatedId2 = _output2.SomeEncapsulatedId;

		Assert.That(
			() => { someEncapsulatedId2.Should().NotBeEquivalentTo(someEncapsulatedId1, options => options.ComparingByMembers<SomeEncapsulatedId>()); },
			Throws.TypeOf<InvalidOperationException>()
				.With.Message.Contain("No members were found for comparison. Please specify some members to include in the comparison or choose a more meaningful assertion.")
		);
	}
}
