using System.Collections;

namespace Tests;

record RootObject(
	SomeEncapsulatedId SomeEncapsulatedId,
	Values<PositiveInteger> Values,
	Mapping<SomeEnum, Values<NegativeInteger>> ExtraValuesByType,
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

record Values<T> : IEnumerable<T>
{
	readonly T[] _encapsulated;
	Values(IEnumerable<T> encapsulated)
	{
		_encapsulated = encapsulated.ToArray();
	}
	public static Values<T> From(IEnumerable<T> enumerable)                            => new(enumerable ?? Array.Empty<T>());
	public static Values<T> Gather(params T[] array)                                   => new(array);
	public static Values<T> From(T item)                                               => new(new[] { item });
	public static Values<T> From<S>(IEnumerable<S> enumerable, Func<S,T> lambda) => new(enumerable?.Select(lambda));

	public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_encapsulated).GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => _encapsulated.GetEnumerator();
	public virtual bool Equals(Values<T> other) => _encapsulated.SequenceEqual(other ?? new(Array.Empty<T>()));
	public override int GetHashCode()
	{
		return unchecked(this.Aggregate(19, (h, i) => h * 19 + i.GetHashCode()));
	}
}

record Mapping<T, U> : Values<KeyValuePair<T, U>>
{
	protected Mapping(Values<KeyValuePair<T, U>> original) : base(original)
	{
	}
	public new static Mapping<T, U> From(IEnumerable<KeyValuePair<T, U>> enumerable)                           => new(Values<KeyValuePair<T,U>>.From(enumerable ?? Array.Empty<KeyValuePair<T, U>>()));
	public new static Mapping<T, U> Gather(params KeyValuePair<T, U>[] array)                                  => new(Values<KeyValuePair<T,U>>.Gather(array));
	public new static Mapping<T, U> From(KeyValuePair<T, U> item)                                              => new(Values<KeyValuePair<T,U>>.From(new[] { item }));
	public new static Mapping<T, U> From<S>(IEnumerable<S> enumerable, Func<S,KeyValuePair<T,U>> lambda)       => new(Values<KeyValuePair<T,U>>.From(enumerable?.Select(lambda)));
	public static Mapping<T, U> From<S>(IEnumerable<S> enumerable, Func<S,T> keyLambda, Func<S,U> valueLambda) => new(Values<KeyValuePair<T,U>>.From(enumerable?.ToDictionary(_ => keyLambda(_), _ => valueLambda(_))));
}
