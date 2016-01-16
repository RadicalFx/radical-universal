using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Radical.ChangeTracking;
using Radical.ComponentModel.ChangeTracking;


namespace RadicalTests.ChangeTracking
{
    [TestClass]
    public class ChangeSetDistinctVisitorTests
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

            public Func<IEnumerable<Object>> GetChangedEntitiesFunc;

            public override IEnumerable<object> GetChangedEntities()
            {
                return this.GetChangedEntitiesFunc();
            }
        }

        [TestMethod()]
        [TestCategory( "ChangeTracking" )]
        public void changeSetDistinctVisitor_visit()
        {
            ChangeSetDistinctVisitor target = new ChangeSetDistinctVisitor();

            var entities = new Object[] { new Object() };
            var c1 = new FakeChange<String>(this,"",v=> { }, v => { });
            c1.GetChangedEntitiesFunc = ()=> entities;

            var c2 = new FakeChange<String>(this, "", v => { }, v => { });
            c2.GetChangedEntitiesFunc = () => entities;

            IChangeSet cSet = new ChangeSet( new IChange[] { c1, c2 } );

            var result = target.Visit( cSet );

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(c2, result[ entities[ 0 ] ]);
        }

        [TestMethod()]
        [TestCategory( "ChangeTracking" )]
        public void changeSetDistinctVisitor_visit_null_changeSet_reference()
        {
            Assert.ThrowsException<ArgumentNullException>(() => 
            {
                ChangeSetDistinctVisitor target = new ChangeSetDistinctVisitor();
                target.Visit(null);
            });
        }
    }
}
