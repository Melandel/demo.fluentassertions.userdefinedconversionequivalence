using FluentAssertions;
using FluentAssertions.Equivalency;

namespace Tests;

public class Tests
{
	static readonly RootObject RootObjectTemplate = new RootObject(
		SomeEncapsulatedId.CreateUnique(),
		new[] { PositiveInteger.CreateFromInteger(1), PositiveInteger.CreateFromInteger(2) },
		new Dictionary<SomeEnum, IEnumerable<NegativeInteger>>
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
		var whatDennisDoomenSuggested = () =>
		{ // https://github.com/fluentassertions/fluentassertions/issues/2016#issuecomment-1279928376
			id2.Should().NotBeEquivalentTo(
					id1,
					options => options.ComparingByMembers<SomeEncapsulatedId>()
			);
		};

		// Act & Assert
		Assert.That(
			whatDennisDoomenSuggested,
			Throws.TypeOf<InvalidOperationException>()
				.With.Message.Contain("No members were found for comparison.")
				.With.Message.Contain("Please specify some members to include in the comparison or choose a more meaningful assertion.")
		);
	}

	[Test]
	public void FailsToFindDifferencesBetweenOutputs_When_Comparing_SomeEncapsulatedId_ByMember()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with { SomeEncapsulatedId = SomeEncapsulatedId.CreateUnique() };
		var whatIWishWasTrue = () =>
		{
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

	[Test]
	public void FindsEquivalenceBetweenEncapsulatedIDs_When_Comparing_SomeEncapsulatedId_ByValue()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate;
		var id1 = o1.SomeEncapsulatedId;
		var id2 = o2.SomeEncapsulatedId;

		// Act & Assert
		id2.Should().BeEquivalentTo(
			id1,
			options => options.ComparingByValue<SomeEncapsulatedId>()
		);
	}

	[Test]
	public void FindsDifferencesBetweenEncapsulatedIDs_When_Comparing_SomeEncapsulatedId_ByValue()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with { SomeEncapsulatedId = SomeEncapsulatedId.CreateUnique() };
		var id1 = o1.SomeEncapsulatedId;
		var id2 = o2.SomeEncapsulatedId;

		// Act & Assert
		id2.Should().NotBeEquivalentTo(
			id1,
			options => options.ComparingByValue<SomeEncapsulatedId>()
		);
	}

	[Test]
	public void FindsEquivalenceBetweenEncapsulatedIDs_When_ComparingRecordsByValue()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate;
		var id1 = o1.SomeEncapsulatedId;
		var id2 = o2.SomeEncapsulatedId;

		// Act & Assert
		id2.Should().BeEquivalentTo(
			id1,
			options => options.ComparingRecordsByValue()
		);
	}

	[Test]
	public void FindsDifferencesBetweenEncapsulatedIDs_When_ComparingRecordsByValue()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with { SomeEncapsulatedId = SomeEncapsulatedId.CreateUnique() };
		var id1 = o1.SomeEncapsulatedId;
		var id2 = o2.SomeEncapsulatedId;

		// Act & Assert
		id2.Should().NotBeEquivalentTo(
			id1,
			options => options.ComparingByValue<SomeEncapsulatedId>()
		);
	}

	[Test]
	public void FindsEquivalenceBetweenRootObjects_When_Comparing_SomeEncapsulatedId_ByValue()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate;

		// Act & Assert
		o2.Should().BeEquivalentTo(
			o1,
			options => options.ComparingByValue<SomeEncapsulatedId>()
		);
	}

	[Test]
	public void FindsDifferencesBetweenRootObjects_When_Comparing_SomeEncapsulatedId_ByValue()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with { SomeEncapsulatedId = SomeEncapsulatedId.CreateUnique() };

		// Act & Assert
		o2.Should().NotBeEquivalentTo(
			o1,
			options => options.ComparingByValue<SomeEncapsulatedId>()
		);
	}

	[Test]
	public void FindsEquivalenceBetweenRootObjects_When_ComparingRecordsByValue()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate;

		// Act & Assert
		o2.Should().BeEquivalentTo(
			o1,
			options => options.ComparingRecordsByValue()
		);
	}

	[Test]
	public void FindsDifferencesBetweenRootObjects_When_ComparingRecordsByValue()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with { SomeEncapsulatedId = SomeEncapsulatedId.CreateUnique() };

		// Act & Assert
		o2.Should().NotBeEquivalentTo(
			o1,
			options => options.ComparingRecordsByValue()
		);
	}

	[Test]
	public void FailsToFindEquivalenceBetweenRootObjects_When_ComparingRecordsByValue_And_Collection_Property_Has_Same_Values_But_In_Different_Array()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with { Values = o1.Values.Select(_ => PositiveInteger.CreateFromInteger(_)) };

		// Act & Assert
		Assert.That(
			() => { o2.Should().BeEquivalentTo(o1, options => options.ComparingRecordsByValue()); },
			Throws.Exception
				.With.Message.Contains("Expected o2 to be RootObject")
				.With.Message.Contains("but found RootObject")
		);
	}

	[Test]
	public void FailsToFindEquivalenceBetweenRootObjects_When_ComparingRecordsByValue_And_Collection_Property_Has_Same_Values_But_In_Different_Array_And_Different_Instances()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with { Values = new[] { PositiveInteger.CreateFromInteger(1), PositiveInteger.CreateFromInteger(2) } };

		// Act & Assert
		Assert.That(
			() => { o2.Should().BeEquivalentTo(o1, options => options.ComparingRecordsByValue()); },
			Throws.Exception
				.With.Message.Contains("Expected o2 to be RootObject")
				.With.Message.Contains("but found RootObject")
		);
	}

	[Test]
	public void FindsEquivalenceBetweenRootObjects_When_Comparing_PositiveIntegersInsideArray_ByValue_And_Collection_Property_Has_Same_Values_But_In_Different_Array_And_Different_Instances()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with { Values = o1.Values.Select(_ => PositiveInteger.CreateFromInteger(_)) };

		// Act & Assert
		o2.Should().BeEquivalentTo(o1, options => options.ComparingByValue<PositiveInteger>());
	}

	[Test]
	public void FailsToFindEquivalenceBetweenRootObjects_When_ComparingRecordsByValue_Then_ComparingPositiveIntegersInsideArray_ByValue()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			Values = o1.Values.Select(_ => PositiveInteger.CreateFromInteger(_))
		};

		// Act & Assert
		Assert.That(
			() => { o2.Should().BeEquivalentTo(o1, options => options.ComparingRecordsByValue().ComparingByValue<PositiveInteger>()); },
			Throws.Exception
				.With.Message.Contain("Expected o2 to be RootObject")
				.With.Message.Contain("but found RootObject")
		);
	}

	[Test]
	public void FindsEquivalenceBetweenRootObjects_When_ComparingSomeEncapsulatedId_ByValue_Then_ComparingPositiveIntegersInsideArray_ByValue()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			SomeEncapsulatedId = SomeEncapsulatedId.CreateFromGuid(o1.SomeEncapsulatedId),
			Values = o1.Values.Select(_ => PositiveInteger.CreateFromInteger(_))
		};

		// Act & Assert
		o2.Should().BeEquivalentTo(o1, options => options
			.ComparingByValue<SomeEncapsulatedId>()
			.ComparingByValue<PositiveInteger>());
	}

	[Test]
	public void FailsToFindDifferenceBetweenRootObjects_When_Dictionary_Has_Different_NegativeInteger_Values()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			SomeEncapsulatedId = SomeEncapsulatedId.CreateFromGuid(o1.SomeEncapsulatedId),
			Values = o1.Values.Select(_ => PositiveInteger.CreateFromInteger(_)),
			ExtraValuesByType = o1.ExtraValuesByType.ToDictionary(_ => _.Key, _ => _.Value.Select(ni => NegativeInteger.CreateFromInteger(ni+1)))
		};
		var config = (EquivalencyAssertionOptions<RootObject> options) => options
			.ComparingByValue<SomeEncapsulatedId>()
			.ComparingByValue<PositiveInteger>();

		// Act & Assert
		Assert.That(
			() => { o2.Should().NotBeEquivalentTo(o1, config); },
			Throws.Exception
				.With.Message.Contain("Expected o2 not to be equivalent to RootObject")
				.With.Message.Contain(", but they are.")
		);
	}

	[Test]
	public void FindsDifferenceBetweenRootObjects_When_ComparingNegativeIntegerByValue_And_Dictionary_Has_Different_NegativeInteger_Values()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			SomeEncapsulatedId = SomeEncapsulatedId.CreateFromGuid(o1.SomeEncapsulatedId),
			Values = o1.Values.Select(_ => PositiveInteger.CreateFromInteger(_)),
			ExtraValuesByType = o1.ExtraValuesByType.ToDictionary(_ => _.Key, _ => _.Value.Select(ni => NegativeInteger.CreateFromInteger(ni+1)))
		};

		// Act & Assert
		o2.Should().NotBeEquivalentTo(o1, options => options
			.ComparingByValue<SomeEncapsulatedId>()
			.ComparingByValue<PositiveInteger>()
			.ComparingByValue<NegativeInteger>());
	}

	[Test]
	public void FindsEquivalenceBetweenRootObjects_When_ComparingNegativeIntegerByValue_And_Dictionary_Has_Different_NegativeInteger_Values()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			SomeEncapsulatedId = SomeEncapsulatedId.CreateFromGuid(o1.SomeEncapsulatedId),
			Values = o1.Values.Select(_ => PositiveInteger.CreateFromInteger(_)),
			ExtraValuesByType = o1.ExtraValuesByType.ToDictionary(_ => _.Key, _ => _.Value.Select(ni => NegativeInteger.CreateFromInteger(ni)))
		};

		// Act & Assert
		o2.Should().BeEquivalentTo(o1, options => options
			.ComparingByValue<SomeEncapsulatedId>()
			.ComparingByValue<PositiveInteger>()
			.ComparingByValue<NegativeInteger>());
	}

	[Test]
	public void FailsToFindDifferenceBetweenRootObjects_When_ComplexObject_Has_Different_Values()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			ComplexObjectWithPositiveIntegers = new SomeObjectContainingPositiveIntegerProperties(
				PositiveInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.PositiveInteger+1),
				NegativeInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.NegativeInteger-1)
			)
		};

		// Act & Assert
		Assert.That(
			() => { o2.Should().NotBeEquivalentTo(o1); },
			Throws.Exception
				.With.Message.Contain("Expected o2 not to be equivalent to RootObject")
				.With.Message.Contain(", but they are.")
		);
	}

	[Test]
	public void FindsDifferenceBetweenRootObjects_When_ComparingPositiveIntegerByValue_And_ComplexObject_Has_Different_Values()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			ComplexObjectWithPositiveIntegers = new SomeObjectContainingPositiveIntegerProperties(
				PositiveInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.PositiveInteger+1),
				NegativeInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.NegativeInteger-1)
			)
		};

		// Act & Assert
		o2.Should().NotBeEquivalentTo(o1, options => options
			.ComparingByValue<PositiveInteger>());
	}

	[Test]
	public void FindsDifferenceBetweenRootObjects_When_ComparingNegativeIntegerByValue_And_ComplexObject_Has_Different_Values()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			ComplexObjectWithPositiveIntegers = new SomeObjectContainingPositiveIntegerProperties(
				PositiveInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.PositiveInteger+1),
				NegativeInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.NegativeInteger-1)
			)
		};

		// Act & Assert
		o2.Should().NotBeEquivalentTo(o1, options => options
			.ComparingByValue<NegativeInteger>());
	}

	[Test]
	public void FindsDifferenceBetweenRootObjects_When_ComparingBothPositiveAndNegativeIntegerByValue_And_ComplexObject_Has_Different_Positive_Value()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			ComplexObjectWithPositiveIntegers = new SomeObjectContainingPositiveIntegerProperties(
				PositiveInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.PositiveInteger+1),
				NegativeInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.NegativeInteger)
			)
		};

		// Act & Assert
		o2.Should().NotBeEquivalentTo(o1, options => options
			.ComparingByValue<PositiveInteger>()
			.ComparingByValue<NegativeInteger>());
	}

	[Test]
	public void FindsDifferenceBetweenRootObjects_When_ComparingBothPositiveAndNegativeIntegerByValue_And_ComplexObject_Has_Different_Negative_Value()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			ComplexObjectWithPositiveIntegers = new SomeObjectContainingPositiveIntegerProperties(
				PositiveInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.PositiveInteger),
				NegativeInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.NegativeInteger-1)
			)
		};

		// Act & Assert
		o2.Should().NotBeEquivalentTo(o1, options => options
			.ComparingByValue<PositiveInteger>()
			.ComparingByValue<NegativeInteger>());
	}

	[Test]
	public void FindsDifferenceBetweenRootObjects_When_ComparingBothPositiveAndNegativeIntegerByValue_And_ComplexObject_Has_Different_Values()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			ComplexObjectWithPositiveIntegers = new SomeObjectContainingPositiveIntegerProperties(
				PositiveInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.PositiveInteger),
				NegativeInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.NegativeInteger)
			)
		};

		// Act & Assert
		o2.Should().BeEquivalentTo(o1, options => options
			.ComparingByValue<PositiveInteger>()
			.ComparingByValue<NegativeInteger>());
	}

	[Test]
	public void FindsDifferenceBetweenRootObjects_When_ComparingWithAllRecordsTypesManuallyByValue_And_SomeEcapsulatedId_Is_Different()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			SomeEncapsulatedId = SomeEncapsulatedId.CreateUnique(),
			Values = o1.Values.Select(_ => PositiveInteger.CreateFromInteger(_)),
			ExtraValuesByType = o1.ExtraValuesByType.ToDictionary(_ => _.Key, _ => _.Value.Select(ni => NegativeInteger.CreateFromInteger(ni))),
			ComplexObjectWithPositiveIntegers = new SomeObjectContainingPositiveIntegerProperties(
				PositiveInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.PositiveInteger),
				NegativeInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.NegativeInteger)
			)
		};
		var configThatWorksButIsNotScalable = (EquivalencyAssertionOptions<RootObject> options) => options
			.ComparingByValue<SomeEncapsulatedId>()
			.ComparingByValue<PositiveInteger>()
			.ComparingByValue<NegativeInteger>();

		// Act & Assert
		o2.Should().NotBeEquivalentTo(o1, configThatWorksButIsNotScalable);
	}

	[Test]
	public void FindsDifferenceBetweenRootObjects_When_ComparingWithAllRecordsTypesManuallyByValue_And_PositiveIntegersInsideArray_Are_Different()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			SomeEncapsulatedId = SomeEncapsulatedId.CreateFromGuid(o1.SomeEncapsulatedId),
			Values = o1.Values.Select(_ => PositiveInteger.CreateFromInteger(_+1000)),
			ExtraValuesByType = o1.ExtraValuesByType.ToDictionary(_ => _.Key, _ => _.Value.Select(ni => NegativeInteger.CreateFromInteger(ni))),
			ComplexObjectWithPositiveIntegers = new SomeObjectContainingPositiveIntegerProperties(
				PositiveInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.PositiveInteger),
				NegativeInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.NegativeInteger)
			)
		};
		var configThatWorksButIsNotScalable = (EquivalencyAssertionOptions<RootObject> options) => options
			.ComparingByValue<SomeEncapsulatedId>()
			.ComparingByValue<PositiveInteger>()
			.ComparingByValue<NegativeInteger>();

		// Act & Assert
		o2.Should().NotBeEquivalentTo(o1, configThatWorksButIsNotScalable);
	}

	[Test]
	public void FindsDifferenceBetweenRootObjects_When_ComparingWithAllRecordsTypesManuallyByValue_And_NegativeIntegersInsideDictionary_Are_Different()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			SomeEncapsulatedId = SomeEncapsulatedId.CreateFromGuid(o1.SomeEncapsulatedId),
			Values = o1.Values.Select(_ => PositiveInteger.CreateFromInteger(_)),
			ExtraValuesByType = o1.ExtraValuesByType.ToDictionary(_ => _.Key, _ => _.Value.Select(ni => NegativeInteger.CreateFromInteger(ni-1000))),
			ComplexObjectWithPositiveIntegers = new SomeObjectContainingPositiveIntegerProperties(
				PositiveInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.PositiveInteger),
				NegativeInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.NegativeInteger)
			)
		};
		var configThatWorksButIsNotScalable = (EquivalencyAssertionOptions<RootObject> options) => options
			.ComparingByValue<SomeEncapsulatedId>()
			.ComparingByValue<PositiveInteger>()
			.ComparingByValue<NegativeInteger>();

		// Act & Assert
		o2.Should().NotBeEquivalentTo(o1, configThatWorksButIsNotScalable);
	}

	[Test]
	public void FindsDifferenceBetweenRootObjects_When_ComparingWithAllRecordsTypesManuallyByValue_And_PositiveIntegerInComplexObject_Is_Different()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			SomeEncapsulatedId = SomeEncapsulatedId.CreateFromGuid(o1.SomeEncapsulatedId),
			Values = o1.Values.Select(_ => PositiveInteger.CreateFromInteger(_)),
			ExtraValuesByType = o1.ExtraValuesByType.ToDictionary(_ => _.Key, _ => _.Value.Select(ni => NegativeInteger.CreateFromInteger(ni))),
			ComplexObjectWithPositiveIntegers = new SomeObjectContainingPositiveIntegerProperties(
				PositiveInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.PositiveInteger+1000),
				NegativeInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.NegativeInteger)
			)
		};
		var configThatWorksButIsNotScalable = (EquivalencyAssertionOptions<RootObject> options) => options
			.ComparingByValue<SomeEncapsulatedId>()
			.ComparingByValue<PositiveInteger>()
			.ComparingByValue<NegativeInteger>();

		// Act & Assert
		o2.Should().NotBeEquivalentTo(o1, configThatWorksButIsNotScalable);
	}

	[Test]
	public void FindsDifferenceBetweenRootObjects_When_ComparingWithAllRecordsTypesManuallyByValue_And_NegativeIntegerInComplexObject_Is_Different()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			SomeEncapsulatedId = SomeEncapsulatedId.CreateFromGuid(o1.SomeEncapsulatedId),
			Values = o1.Values.Select(_ => PositiveInteger.CreateFromInteger(_)),
			ExtraValuesByType = o1.ExtraValuesByType.ToDictionary(_ => _.Key, _ => _.Value.Select(ni => NegativeInteger.CreateFromInteger(ni))),
			ComplexObjectWithPositiveIntegers = new SomeObjectContainingPositiveIntegerProperties(
				PositiveInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.PositiveInteger),
				NegativeInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.NegativeInteger-1000)
			)
		};
		var configThatWorksButIsNotScalable = (EquivalencyAssertionOptions<RootObject> options) => options
			.ComparingByValue<SomeEncapsulatedId>()
			.ComparingByValue<PositiveInteger>()
			.ComparingByValue<NegativeInteger>();

		// Act & Assert
		o2.Should().NotBeEquivalentTo(o1, configThatWorksButIsNotScalable);
	}

	[Test]
	public void FindEquivalenceBetweenRootObjects_When_ComparingWithAllRecordsTypesManuallyByValue_And_Values_Are_The_Same_But_From_Different_Instances()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			SomeEncapsulatedId = SomeEncapsulatedId.CreateFromGuid(o1.SomeEncapsulatedId),
			Values = o1.Values.Select(_ => PositiveInteger.CreateFromInteger(_)),
			ExtraValuesByType = o1.ExtraValuesByType.ToDictionary(_ => _.Key, _ => _.Value.Select(ni => NegativeInteger.CreateFromInteger(ni))),
			ComplexObjectWithPositiveIntegers = new SomeObjectContainingPositiveIntegerProperties(
				PositiveInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.PositiveInteger),
				NegativeInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.NegativeInteger)
			)
		};
		var configThatWorksButIsNotScalable = (EquivalencyAssertionOptions<RootObject> options) => options
			.ComparingByValue<SomeEncapsulatedId>()
			.ComparingByValue<PositiveInteger>()
			.ComparingByValue<NegativeInteger>();

		// Act & Assert
		o2.Should().BeEquivalentTo(o1, configThatWorksButIsNotScalable);
	}

	[Test]
	public void FindEquivalenceBetweenRootObjects_When_ComparingRecordsByValue_And_Values_Are_The_Same_But_In_Different_Instance_And_Collections_Reference_The_Same_Thing()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			SomeEncapsulatedId = SomeEncapsulatedId.CreateFromGuid(o1.SomeEncapsulatedId),
			ComplexObjectWithPositiveIntegers = new SomeObjectContainingPositiveIntegerProperties(
				PositiveInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.PositiveInteger),
				NegativeInteger.CreateFromInteger(o1.ComplexObjectWithPositiveIntegers.NegativeInteger)
			)
		};
		var configThatIsScalableButDoesNotWorkWhenThereAreCollectionsInvolved = (EquivalencyAssertionOptions<RootObject> options) => options.ComparingRecordsByValue();

		// Act & Assert
		o2.Should().BeEquivalentTo(o1, configThatIsScalableButDoesNotWorkWhenThereAreCollectionsInvolved);
	}

	[Test]
	public void FailstoFindEquivalenceBetweenRootObjects_When_ComparingRecordsByValue_And_Array_Does_Not_Reference_The_Same_Thing_Despite_Having_Same_Values_Inside()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			Values = o1.Values.Select(_ => PositiveInteger.CreateFromInteger(_)),
		};
		var configThatIsScalableButDoesNotWorkWhenThereAreCollectionsInvolved = (EquivalencyAssertionOptions<RootObject> options) => options.ComparingRecordsByValue();

		// Act & Assert
		Assert.That(
			() => { o2.Should().BeEquivalentTo(o1, configThatIsScalableButDoesNotWorkWhenThereAreCollectionsInvolved); },
			Throws.Exception
				.With.Message.Contain("Expected o2 to be RootObject")
				.With.Message.Contain("but found RootObject")
		);
	}

	public void FailstoFindEquivalenceBetweenRootObjects_When_ComparingRecordsByValue_And_Dictionary_Does_Not_Reference_The_Same_Thing_Despite_Having_Same_Values_Inside()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with {
			ExtraValuesByType = o1.ExtraValuesByType.ToDictionary(_ => _.Key, _ => _.Value.Select(ni => NegativeInteger.CreateFromInteger(ni))),
		};
		var configThatIsScalableButDoesNotWorkWhenThereAreCollectionsInvolved = (EquivalencyAssertionOptions<RootObject> options) => options.ComparingRecordsByValue();

		// Act & Assert
		Assert.That(
			() => { o2.Should().BeEquivalentTo(o1, configThatIsScalableButDoesNotWorkWhenThereAreCollectionsInvolved); },
			Throws.Exception
				.With.Message.Contain("Expected o2 to be RootObject")
				.With.Message.Contain("but found RootObject")
		);
	}
}
