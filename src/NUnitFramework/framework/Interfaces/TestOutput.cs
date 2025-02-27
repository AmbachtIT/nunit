// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

#nullable enable

using System;

namespace NUnit.Framework.Interfaces
{
    /// <summary>
    /// The TestOutput class holds a unit of output from
    /// a test to a specific output stream
    /// </summary>
	public class TestOutput
	{
        /// <summary>
        /// Construct with text, output destination type and
        /// the name of the test that produced the output.
        /// </summary>
        /// <param name="text">Text to be output</param>
        /// <param name="stream">Name of the stream or channel to which the text should be written</param>
        /// <param name="testId">Id of the test that produced the output</param>
        /// <param name="testName">FullName of test that produced the output</param>
        public TestOutput(string text, string stream, string testId, string testName)
        {
            Text = text;
            Stream = stream;
            TestId = testId;
            TestName = testName;
        }

        /// <summary>
        /// Return string representation of the object for debugging
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return Stream + ": " + Text;
		}

        /// <summary>
        /// Get the text
        /// </summary>
		public string Text { get; }

        /// <summary>
        /// Get the output type
        /// </summary>
		public string Stream { get; }

        /// <summary>
        /// Get the name of the test that created the output
        /// </summary>
        public string TestName { get; }

        /// <summary>
        /// Get the id of the test that created the output
        /// </summary>
        public string TestId { get; }

        /// <summary>
        /// Convert the TestOutput object to an XML string
        /// </summary>
        public string ToXml()
        {
            TNode tnode = new TNode("test-output", Text, true);

            tnode.AddAttribute("stream", Stream);
            if (TestId != null)
                tnode.AddAttribute("testid", TestId);

            if (TestName != null)
                tnode.AddAttribute("testname", TestName);

            return tnode.OuterXml;
        }
    }
}
