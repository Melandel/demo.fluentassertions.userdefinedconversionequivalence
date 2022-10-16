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
		var o2 = RootObjectTemplate with { Values = new[] { o1.Values[0], o1.Values[1] } };

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
	public void FindsEquivalenceBetweenRootObjects_When_Comparing_PositiveIntegersInsideArray_ByValue_And_Collection_Property_Has_Same_Values_But_In_Different_Array()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with { Values = new[] { o1.Values[0], o1.Values[1] } };

		// Act & Assert
		o2.Should().BeEquivalentTo(o1, options => options.ComparingByValue<PositiveInteger>());
	}

	[Test]
	public void FindsEquivalenceBetweenRootObjects_When_Comparing_PositiveIntegersInsideArray_ByValue_And_Collection_Property_Has_Same_Values_But_In_Different_Array_And_Different_Instances()
	{
		// Arrange
		var o1 = RootObjectTemplate;
		var o2 = RootObjectTemplate with { Values = new[] { PositiveInteger.CreateFromInteger(1), PositiveInteger.CreateFromInteger(2) } };

		// Act & Assert
		o2.Should().BeEquivalentTo(o1, options => options.ComparingByValue<PositiveInteger>());
	}
}
