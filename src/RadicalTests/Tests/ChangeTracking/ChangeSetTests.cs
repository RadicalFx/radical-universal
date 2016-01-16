namespace RadicalTests.ChangeTracking
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Radical.ChangeTracking;
    using Radical.ComponentModel.ChangeTracking;
    

    [TestClass]
    public class ChangeSetTests
    {
        class FakeChange<T> : Change<T>
        {
            public FakeChange(Object owner, T valueToCache, RejectCallback<T> rejectCallback, CommitCallback<T> commitCallback, String description = "")
                : base(owner, valueToCache, rejectCallback, commitCallback, description)
            {
            }
            public override IChange Clone()
            {
                throw new NotImplementedException();
            }

            public override ProposedActions GetAdvisedAction(object changedItem)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeSet_ctor()
        {
            var expected = new IChange[] 
            {
                new FakeChange<String>(this,"",v=> { },v=> { }),
                new FakeChange<String>(this,"",v=> { },v=> { }),
                new FakeChange<String>(this,"",v=> { },v=> { }),
                new FakeChange<String>(this,"",v=> { },v=> { })
            };

            var actual = new ChangeSet( expected );

            Assert.AreEqual(4, actual.Count);
            CollectionAssert.AreEquivalent(expected, actual.ToList());
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void changeSet_ctor_null_reference()
        {
            Assert.ThrowsException<ArgumentNullException>(() => 
            {
                var actual = new ChangeSet(null);
            });
        }
    }
}