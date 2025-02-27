// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections;
using NUnit.Framework.Constraints;

namespace NUnit.Framework
{
    /// <summary>
    /// Helper class with properties and methods that supply
    /// a number of constraints used in Asserts.
    /// </summary>
    // Abstract because we support syntax extension by inheriting and declaring new static members.
    public abstract class Is
    {
        #region Not

        /// <summary>
        /// Returns a ConstraintExpression that negates any
        /// following constraint.
        /// </summary>
        public static ConstraintExpression Not => new ConstraintExpression().Not;

        #endregion

        #region All

        /// <summary>
        /// Returns a ConstraintExpression, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if all of them succeed.
        /// </summary>
        public static ConstraintExpression All => new ConstraintExpression().All;

        #endregion

        #region Null

        /// <summary>
        /// Returns a constraint that tests for null
        /// </summary>
        public static NullConstraint Null => new NullConstraint();

        #endregion

        #region Default

        /// <summary>
        /// Returns a constraint that tests for default value
        /// </summary>
        public static DefaultConstraint Default => new DefaultConstraint();

        #endregion

        #region True

        /// <summary>
        /// Returns a constraint that tests for True
        /// </summary>
        public static TrueConstraint True => new TrueConstraint();

        #endregion

        #region False

        /// <summary>
        /// Returns a constraint that tests for False
        /// </summary>
        public static FalseConstraint False => new FalseConstraint();

        #endregion

        #region Positive

        /// <summary>
        /// Returns a constraint that tests for a positive value
        /// </summary>
        public static GreaterThanConstraint Positive => new GreaterThanConstraint(0);

        #endregion

        #region Negative

        /// <summary>
        /// Returns a constraint that tests for a negative value
        /// </summary>
        public static LessThanConstraint Negative => new LessThanConstraint(0);

        #endregion

        #region Zero

        /// <summary>
        /// Returns a constraint that tests for equality with zero
        /// </summary>
        public static EqualConstraint Zero => new EqualConstraint(0);

        #endregion

        #region NaN

        /// <summary>
        /// Returns a constraint that tests for NaN
        /// </summary>
        public static NaNConstraint NaN => new NaNConstraint();

        #endregion

        #region Empty

        /// <summary>
        /// Returns a constraint that tests for empty
        /// </summary>
        public static EmptyConstraint Empty => new EmptyConstraint();

        #endregion

        #region Unique

        /// <summary>
        /// Returns a constraint that tests whether a collection
        /// contains all unique items.
        /// </summary>
        public static UniqueItemsConstraint Unique => new UniqueItemsConstraint();

        #endregion

        /// <summary>
        /// Returns a constraint that tests whether an object graph is serializable in binary format.
        /// </summary>
        public static BinarySerializableConstraint BinarySerializable => new BinarySerializableConstraint();

        /// <summary>
        /// Returns a constraint that tests whether an object graph is serializable in XML format.
        /// </summary>
        public static XmlSerializableConstraint XmlSerializable => new XmlSerializableConstraint();

        #region EqualTo

        /// <summary>
        /// Returns a constraint that tests two items for equality
        /// </summary>
        public static EqualConstraint EqualTo(object expected)
        {
            return new EqualConstraint(expected);
        }

        #endregion

        #region SameAs

        /// <summary>
        /// Returns a constraint that tests that two references are the same object
        /// </summary>
        public static SameAsConstraint SameAs(object expected)
        {
            return new SameAsConstraint(expected);
        }

        #endregion

        #region GreaterThan

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than the supplied argument
        /// </summary>
        public static GreaterThanConstraint GreaterThan(object expected)
        {
            return new GreaterThanConstraint(expected);
        }

        #endregion

        #region GreaterThanOrEqualTo

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than or equal to the supplied argument
        /// </summary>
        public static GreaterThanOrEqualConstraint GreaterThanOrEqualTo(object expected)
        {
            return new GreaterThanOrEqualConstraint(expected);
        }

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than or equal to the supplied argument
        /// </summary>
        public static GreaterThanOrEqualConstraint AtLeast(object expected)
        {
            return new GreaterThanOrEqualConstraint(expected);
        }

        #endregion

        #region LessThan

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is less than the supplied argument
        /// </summary>
        public static LessThanConstraint LessThan(object expected)
        {
            return new LessThanConstraint(expected);
        }

        #endregion

        #region LessThanOrEqualTo

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is less than or equal to the supplied argument
        /// </summary>
        public static LessThanOrEqualConstraint LessThanOrEqualTo(object expected)
        {
            return new LessThanOrEqualConstraint(expected);
        }

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is less than or equal to the supplied argument
        /// </summary>
        public static LessThanOrEqualConstraint AtMost(object expected)
        {
            return new LessThanOrEqualConstraint(expected);
        }

        #endregion

        #region TypeOf

        /// <summary>
        /// Returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        public static ExactTypeConstraint TypeOf(Type expectedType)
        {
            return new ExactTypeConstraint(expectedType);
        }

        /// <summary>
        /// Returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        public static ExactTypeConstraint TypeOf<TExpected>()
        {
            return new ExactTypeConstraint(typeof(TExpected));
        }

        #endregion

        #region InstanceOf

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is of the type supplied as an argument or a derived type.
        /// </summary>
        public static InstanceOfTypeConstraint InstanceOf(Type expectedType)
        {
            return new InstanceOfTypeConstraint(expectedType);
        }

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is of the type supplied as an argument or a derived type.
        /// </summary>
        public static InstanceOfTypeConstraint InstanceOf<TExpected>()
        {
            return new InstanceOfTypeConstraint(typeof(TExpected));
        }

        #endregion

        #region AssignableFrom

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is assignable from the type supplied as an argument.
        /// </summary>
        public static AssignableFromConstraint AssignableFrom(Type expectedType)
        {
            return new AssignableFromConstraint(expectedType);
        }

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is assignable from the type supplied as an argument.
        /// </summary>
        public static AssignableFromConstraint AssignableFrom<TExpected>()
        {
            return new AssignableFromConstraint(typeof(TExpected));
        }

        #endregion

        #region AssignableTo

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is assignable to the type supplied as an argument.
        /// </summary>
        public static AssignableToConstraint AssignableTo(Type expectedType)
        {
            return new AssignableToConstraint(expectedType);
        }

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is assignable to the type supplied as an argument.
        /// </summary>
        public static AssignableToConstraint AssignableTo<TExpected>()
        {
            return new AssignableToConstraint(typeof(TExpected));
        }

        #endregion

        #region EquivalentTo

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is a collection containing the same elements as the
        /// collection supplied as an argument.
        /// </summary>
        public static CollectionEquivalentConstraint EquivalentTo(IEnumerable expected)
        {
            return new CollectionEquivalentConstraint(expected);
        }

        #endregion

        #region SubsetOf

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is a subset of the collection supplied as an argument.
        /// </summary>
        public static CollectionSubsetConstraint SubsetOf(IEnumerable expected)
        {
            return new CollectionSubsetConstraint(expected);
        }

        #endregion

        #region SupersetOf

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is a superset of the collection supplied as an argument.
        /// </summary>
        public static CollectionSupersetConstraint SupersetOf(IEnumerable expected)
        {
            return new CollectionSupersetConstraint(expected);
        }

        #endregion

        #region Ordered

        /// <summary>
        /// Returns a constraint that tests whether a collection is ordered
        /// </summary>
        public static CollectionOrderedConstraint Ordered => new CollectionOrderedConstraint();

        #endregion

        #region SamePath

        /// <summary>
        /// Returns a constraint that tests whether the path provided
        /// is the same as an expected path after canonicalization.
        /// </summary>
        public static SamePathConstraint SamePath(string expected)
        {
            return new SamePathConstraint(expected);
        }

        #endregion

        #region SubPath

        /// <summary>
        /// Returns a constraint that tests whether the path provided
        /// is a subpath of the expected path after canonicalization.
        /// </summary>
        public static SubPathConstraint SubPathOf(string expected)
        {
            return new SubPathConstraint(expected);
        }

        #endregion

        #region SamePathOrUnder

        /// <summary>
        /// Returns a constraint that tests whether the path provided
        /// is the same path or under an expected path after canonicalization.
        /// </summary>
        public static SamePathOrUnderConstraint SamePathOrUnder(string expected)
        {
            return new SamePathOrUnderConstraint(expected);
        }

        #endregion

        #region InRange

        /// <summary>
        /// Returns a constraint that tests whether the actual value falls
        /// inclusively within a specified range.
        /// </summary>
        /// <param name="from">Inclusive beginning of the range.</param>
        /// <param name="to">Inclusive end of the range.</param>
        /// <returns></returns>
        public static RangeConstraint InRange(object from, object to)
        {
            return new RangeConstraint(from, to);
        }

        #endregion

        #region Any Of

        /// <summary>
        /// Returns a constraint that tests if an item is equal to any of parameters
        /// </summary>
        /// <param name="expected">Expected values</param>
        public static AnyOfConstraint AnyOf(params object[] expected)
        {
            if (expected == null)
            {
                expected = new object[] { null };
            }

            return new AnyOfConstraint(expected);
        }

        #endregion

    }
}
