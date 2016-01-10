using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Radical.ComponentModel.ChangeTracking;
using Radical.ChangeTracking;

namespace RadicalTests
{
    [TestClass()]
    public class ChangeArgsTests
    {
        public class FakeChange<T> : Change<T>
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

        protected virtual ChangeEventArgs<T> CreateMock<T>( Object entity, T cachedValue, IChange source )
        {
            return new ChangeEventArgs<T>( entity, cachedValue, source );
        }

        [TestMethod]
        public void changeArgs_generic_ctor_normal_should_correctly_set_values()
        {
            var entity = new Object();
            var cachedValue = "foo";
            var iChange = new FakeChange<String>(entity,cachedValue, v=> { }, v => { });

            var target = this.CreateMock( entity, cachedValue, iChange );

            Assert.AreEqual(entity, target.Entity);
            Assert.AreEqual(cachedValue, target.CachedValue);
            Assert.AreEqual(iChange, target.Source);
        }

        [TestMethod]
        public void changeArgs_ctor_using_null_reference_entity_should_raise_ArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                Object entity = null;
                var iChange = new FakeChange<String>(entity, "foo", v => { }, v => { });

                var target = this.CreateMock(entity, "foo", iChange);
            });
        }

        [TestMethod]
        public void changeArgs_ctor_using_null_reference_iChange_should_raise_ArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => 
            {
                var entity = new Object();
                var cachedValue = "foo";
                IChange iChange = null;

                var target = this.CreateMock(entity, cachedValue, iChange);
            });
        }
    }
}
