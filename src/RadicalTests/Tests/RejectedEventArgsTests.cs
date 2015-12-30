using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Radical.ComponentModel.ChangeTracking;


namespace RadicalTests
{
    [TestClass()]
    public class RejectedEventArgsTests
    {
        [TestMethod]
        public void rejectedEventArgs_ctor_normal_should_set_values()
        {
            var expected = RejectReason.RejectChanges;
            RejectedEventArgs target = new RejectedEventArgs(expected);

            Assert.AreEqual(expected, target.Reason);
        }
    }
}
