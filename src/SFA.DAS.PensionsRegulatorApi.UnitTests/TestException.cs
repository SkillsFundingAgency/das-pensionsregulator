using System;

namespace SFA.DAS.PensionsRegulatorApi.UnitTests
{
    public class TestException : Exception
    {
        public TestException()
        {
            
        }
        public TestException(string message) : base(message)
        {
        }
    }
}