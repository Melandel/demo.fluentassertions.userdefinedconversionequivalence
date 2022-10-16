namespace Tests;

record RootObject(
	SomeEncapsulatedId SomeEncapsulatedId,
	IEnumerable<PositiveInteger> Values,
	Dictionary<SomeEnum, IEnumerable<NegativeInteger>> ExtraValuesByType,
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
	public static SomeEncapsulatedId CreateUnique() => new(Guid.NewGuid());
	public static SomeEncapsulatedId CreateFromGuid(Guid guid) => new(guid);
	public static implicit operator Guid(SomeEncapsulatedId someEncapsulatedId) => someEncapsulatedId._encapsulated;
}

enum SomeEnum
{
	Value,
	AnotherValue
}

