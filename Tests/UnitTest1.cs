using FluentAssertions;

namespace Tests;

record Output(
	SomeEncapsulatedId SomeEncapsulatedId,
	PositiveInteger[] Values,
	Dictionary<SomeEnum, NegativeInteger[]> ExtraValuesByType,
	SomeObjectContainingPositiveIntegerProperties ComplexObjectWithPositiveIntegers
);

record SomeObjectContainingPositiveIntegerProperties(
	PositiveInteger PositiveInteger,
	NegativeInteger NegativeInteger
);


record PositiveInteger
{
	readonly int _encapsulated;
	PositiveInteger(int encapsulated) => _encapsulated = encapsulated;
	public static PositiveInteger CreateFromInteger(int i) => new(i);
	public static implicit operator int(PositiveInteger someEncapsulatedId) => someEncapsulatedId._encapsulated;
}

record NegativeInteger
{
	readonly int _encapsulated;
	NegativeInteger(int encapsulated) => _encapsulated = encapsulated;
	public static NegativeInteger CreateFromInteger(int i) => new(i);
	public static implicit operator int(NegativeInteger someEncapsulatedId) => someEncapsulatedId._encapsulated;
}

record SomeEncapsulatedId
{
	readonly Guid _encapsulated;
	SomeEncapsulatedId(Guid encapsulated) => _encapsulated = encapsulated;
	public static SomeEncapsulatedId Create() => new(Guid.NewGuid());
	public static implicit operator Guid(SomeEncapsulatedId someEncapsulatedId) => someEncapsulatedId._encapsulated;
}

enum SomeEnum
{
	Value,
	AnotherValue
}

public class Tests
{
	Output _output1;
	Output _output2;
	readonly Output OutputBase = new Output(
		SomeEncapsulatedId.Create(),
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
		_output2 = OutputBase with { SomeEncapsulatedId = SomeEncapsulatedId.Create() };
	}

	[Test]
	public void FaWithoutOptions_Should_FindEquivalence()
	{
		_output2.Should().BeEquivalentTo(_output1);
	}

	[Test]
	public void FaWithOptions_ComparingByMember_Should_FindDifference_From_DifferentGuid()
	{
		_output2.Should().NotBeEquivalentTo(_output1, options => options.ComparingByMembers<SomeEncapsulatedId>());
	}

	[Test]
	public void FaWithOptions_ComparingByMember_Should_FindDifference_When_Applied_On_SomeEncapsulatedId()
	{
		var someEncapsulatedId1 = _output1.SomeEncapsulatedId;
		var someEncapsulatedId2 = _output2.SomeEncapsulatedId;
		someEncapsulatedId2.Should().NotBeEquivalentTo(someEncapsulatedId1, options => options.ComparingByMembers<SomeEncapsulatedId>());
	}
}
