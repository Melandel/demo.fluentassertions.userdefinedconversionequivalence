namespace Tests;

//NOTE: fluentAssertion still provides value regarding the details returned whenever equivalence is not achieved
public class ValuesObjectProvidesNativeDeepEqualityBecauseIt
{
	static readonly RootObject RootObjectTemplate = new RootObject(
		SomeEncapsulatedId.CreateUnique(),
		Values<PositiveInteger>.Gather(PositiveInteger.CreateFromInteger(1), PositiveInteger.CreateFromInteger(2)),
		Mapping<SomeEnum, Values<NegativeInteger>>.Gather(
			KeyValuePair.Create(SomeEnum.Value,        Values<NegativeInteger>.Gather(NegativeInteger.CreateFromInteger(-1), NegativeInteger.CreateFromInteger(-2))),
			KeyValuePair.Create(SomeEnum.AnotherValue, Values<NegativeInteger>.Gather(NegativeInteger.CreateFromInteger(-1), NegativeInteger.CreateFromInteger(-4)))
		),
		new SomeObjectContainingPositiveIntegerProperties(
			PositiveInteger.CreateFromInteger(3),
			NegativeInteger.CreateFromInteger(-5)
		)
	);

	[Test]
	public void FindsEquivalenceBetweenSameRootObjects()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate;

		// Act & Assert
		Assert.That(o1, Is.EqualTo(o2));
	}

	[Test]
	public void FindsEquivalenceBetweenRootObjects_With_Same_PositiveIntegersInsideCollection_In_Different_Instances()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with
		{
			Values = Values<PositiveInteger>.Gather(
				PositiveInteger.CreateFromInteger(o1.Values.First()),
				PositiveInteger.CreateFromInteger(o1.Values.Last())
			)
		};

		// Act & Assert
		Assert.That(o1, Is.EqualTo(o2));
	}

	[Test]
	public void FindsDifferencesBetweenRootObjects_With_Different_PositiveIntegersInsideCollection()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with
		{
			Values = Values<PositiveInteger>.Gather(
				PositiveInteger.CreateFromInteger(1337),
				PositiveInteger.CreateFromInteger(2022)
			)
		};

		// Act & Assert
		Assert.That(o1, Is.Not.EqualTo(o2));
	}

	[Test]
	public void FindsEquivalenceBetweenRootObjects_With_Same_NegativeIntegersInsideMapping_In_Different_Collection()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with
		{
			ExtraValuesByType = Mapping<SomeEnum, Values<NegativeInteger>>.From(
				o1.ExtraValuesByType,
				kvp => kvp.Key,
				kvp => Values<NegativeInteger>.From(kvp.Value, ni => NegativeInteger.CreateFromInteger(ni))),
		};

		// Act & Assert
		Assert.That(o1, Is.EqualTo(o2));
	}

	[Test]
	public void FindsDifferencesBetweenRootObjects_With_Different_NegativeIntegersInsideMapping()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with
		{
			ExtraValuesByType = Mapping<SomeEnum, Values<NegativeInteger>>.From(
				o1.ExtraValuesByType,
				kvp => kvp.Key,
				kvp => Values<NegativeInteger>.From(kvp.Value, ni => NegativeInteger.CreateFromInteger(ni-1000))),
		};

		// Act & Assert
		Assert.That(o1, Is.Not.EqualTo(o2));
	}
}
