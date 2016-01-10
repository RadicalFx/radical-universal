using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Radical.ComponentModel.ChangeTracking;


namespace RadicalTests
{
    [TestClass()]
    public class CommittedEventArgsTests
    {
        [TestMethod]
        public void committedEventArgs_ctor_normal_should_set_values()
        {
            var expected = CommitReason.AcceptChanges;
            CommittedEventArgs target = new CommittedEventArgs( expected );

            Assert.AreEqual(expected, target.Reason);
        }
    }
}
