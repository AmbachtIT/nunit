// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

#nullable enable

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;

namespace NUnit.Framework
{
    /// <summary>
    /// Provides static methods to express the assumptions
    /// that must be met for a test to give a meaningful
    /// result. If an assumption is not met, the test
    /// should produce an inconclusive result.
    /// </summary>
    public abstract class Assume
    {
        #region Equals and ReferenceEquals

        /// <summary>
        /// DO NOT USE!
        /// The Equals method throws an InvalidOperationException. This is done
        /// to make sure there is no mistake by calling this function.
        /// </summary>
        /// <param name="a">The left object.</param>
        /// <param name="b">The right object.</param>
        /// <returns>Not applicable</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool Equals(object a, object b)
        {
            throw new InvalidOperationException("Assume.Equals should not be used. Use Assume.That instead.");
        }

        /// <summary>
        /// DO NOT USE!
        /// The ReferenceEquals method throws an InvalidOperationException. This is done
        /// to make sure there is no mistake by calling this function.
        /// </summary>
        /// <param name="a">The left object.</param>
        /// <param name="b">The right object.</param>
        public static new void ReferenceEquals(object a, object b)
        {
            throw new InvalidOperationException("Assume.ReferenceEquals should not be used. Use Assume.That instead.");
        }

        #endregion

        #region Assume.That

        #region ActualValueDelegate

        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint
        /// is satisfied and throwing an InconclusiveException on failure.
        /// </summary>
        /// <typeparam name="TActual">The Type being compared.</typeparam>
        /// <param name="del">An ActualValueDelegate returning the value to be tested</param>
        /// <param name="expr">A Constraint expression to be applied</param>
        public static void That<TActual>(ActualValueDelegate<TActual> del, IResolveConstraint expr)
        {
            Assume.That(del, expr.Resolve(), null, null);
        }

        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint
        /// is satisfied and throwing an InconclusiveException on failure.
        /// </summary>
        /// <typeparam name="TActual">The Type being compared.</typeparam>
        /// <param name="del">An ActualValueDelegate returning the value to be tested</param>
        /// <param name="expr">A Constraint expression to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void That<TActual>(ActualValueDelegate<TActual> del, IResolveConstraint expr, string? message, params object?[]? args)
        {
            CheckMultipleAssertLevel();

            var constraint = expr.Resolve();
            var result = constraint.ApplyTo(del);

            if (!result.IsSuccess)
                ReportFailure(result, message, args);
        }

        private static void ReportFailure(ConstraintResult result, string? message, object?[]? args)
        {
            MessageWriter writer = new TextMessageWriter(message, args);
            result.WriteMessageTo(writer);
            throw new InconclusiveException(writer.ToString());
        }

        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint
        /// is satisfied and throwing an InconclusiveException on failure.
        /// </summary>
        /// <typeparam name="TActual">The Type being compared.</typeparam>
        /// <param name="del">An ActualValueDelegate returning the value to be tested</param>
        /// <param name="expr">A Constraint expression to be applied</param>
        /// <param name="getExceptionMessage">A function to build the message included with the Exception</param>
        public static void That<TActual>(
            ActualValueDelegate<TActual> del,
            IResolveConstraint expr,
            Func<string?> getExceptionMessage)
        {
            CheckMultipleAssertLevel();

            var constraint = expr.Resolve();

            var result = constraint.ApplyTo(del);
            if (!result.IsSuccess)
            {
                throw new InconclusiveException(getExceptionMessage());
            }
        }

        #endregion

        #region Boolean

        /// <summary>
        /// Asserts that a condition is true. If the condition is false, the method throws
        /// an <see cref="InconclusiveException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display if the condition is false</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void That([DoesNotReturnIf(false)] bool condition, string? message, params object?[]? args)
        {
            Assume.That(condition, Is.True, message, args);
        }

        /// <summary>
        /// Asserts that a condition is true. If the condition is false, the
        /// method throws an <see cref="InconclusiveException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        public static void That([DoesNotReturnIf(false)] bool condition)
        {
            Assume.That(condition, Is.True, null, null);
        }

        /// <summary>
        /// Asserts that a condition is true. If the condition is false, the method throws
        /// an <see cref="InconclusiveException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="getExceptionMessage">A function to build the message included with the Exception</param>
        public static void That([DoesNotReturnIf(false)] bool condition, Func<string?> getExceptionMessage)
        {
            Assume.That(condition, Is.True, getExceptionMessage);
        }

        #endregion

        #region Lambda returning Boolean

        /// <summary>
        /// Asserts that a condition is true. If the condition is false, the method throws
        /// an <see cref="InconclusiveException"/>.
        /// </summary>
        /// <param name="condition">A lambda that returns a Boolean</param>
        /// <param name="message">The message to display if the condition is false</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void That(Func<bool> condition, string? message, params object?[]? args)
        {
            Assume.That(condition.Invoke(), Is.True, message, args);
        }

        /// <summary>
        /// Asserts that a condition is true. If the condition is false, the method throws
        /// an <see cref="InconclusiveException"/>.
        /// </summary>
        /// <param name="condition">A lambda that returns a Boolean</param>
        public static void That(Func<bool> condition)
        {
            Assume.That(condition.Invoke(), Is.True, null, null);
        }

        /// <summary>
        /// Asserts that a condition is true. If the condition is false, the method throws
        /// an <see cref="InconclusiveException"/>.
        /// </summary>
        /// <param name="condition">A lambda that returns a Boolean</param>
        /// <param name="getExceptionMessage">A function to build the message included with the Exception</param>
        public static void That(Func<bool> condition, Func<string?> getExceptionMessage)
        {
            Assume.That(condition.Invoke(), Is.True, getExceptionMessage);
        }

        #endregion

        #region TestDelegate

        /// <summary>
        /// Asserts that the code represented by a delegate throws an exception
        /// that satisfies the constraint provided.
        /// </summary>
        /// <param name="code">A TestDelegate to be executed</param>
        /// <param name="constraint">A ThrowsConstraint used in the test</param>
        public static void That(TestDelegate code, IResolveConstraint constraint)
        {
            Assume.That((object)code, constraint);
        }

#endregion

#endregion

        #region Assume.That<TActual>

        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint
        /// is satisfied and throwing an InconclusiveException on failure.
        /// </summary>
        /// <typeparam name="TActual">The Type being compared.</typeparam>
        /// <param name="actual">The actual value to test</param>
        /// <param name="expression">A Constraint expression to be applied</param>
        public static void That<TActual>(TActual actual, IResolveConstraint expression)
        {
            Assume.That(actual, expression, null, null);
        }

        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint
        /// is satisfied and throwing an InconclusiveException on failure.
        /// </summary>
        /// <typeparam name="TActual">The Type being compared.</typeparam>
        /// <param name="actual">The actual value to test</param>
        /// <param name="expression">A Constraint expression to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void That<TActual>(TActual actual, IResolveConstraint expression, string? message, params object?[]? args)
        {
            CheckMultipleAssertLevel();

            var constraint = expression.Resolve();

            var result = constraint.ApplyTo(actual);
            if (!result.IsSuccess)
            {
                MessageWriter writer = new TextMessageWriter(message, args);
                result.WriteMessageTo(writer);
                throw new InconclusiveException(writer.ToString());
            }
        }

        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint
        /// is satisfied and throwing an InconclusiveException on failure.
        /// </summary>
        /// <typeparam name="TActual">The Type being compared.</typeparam>
        /// <param name="actual">The actual value to test</param>
        /// <param name="expression">A Constraint expression to be applied</param>
        /// <param name="getExceptionMessage">A function to build the message included with the Exception</param>
        public static void That<TActual>(
            TActual actual,
            IResolveConstraint expression,
            Func<string?> getExceptionMessage)
        {
            CheckMultipleAssertLevel();

            var constraint = expression.Resolve();

            var result = constraint.ApplyTo(actual);
            if (!result.IsSuccess)
            {
                throw new InconclusiveException(getExceptionMessage());
            }
        }

        #endregion

        #region Helper Methods

        private static void CheckMultipleAssertLevel()
        {
            if (TestExecutionContext.CurrentContext.MultipleAssertLevel > 0)
                throw new Exception("Assume.That may not be used in a multiple assertion block.");
        }

        #endregion
    }
}
