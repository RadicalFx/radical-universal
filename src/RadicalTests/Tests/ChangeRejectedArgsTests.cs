//extern alias tpx;

using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Radical.ComponentModel.ChangeTracking;


namespace RadicalTests
{
    [TestClass()]
    public class ChangeRejectedArgsTests : ChangeArgsTests
    {
        protected override ChangeEventArgs<T> CreateMock<T>( object entity, T cachedValue, IChange source )
        {
            return this.CreateMock<T>( entity, cachedValue, source, RejectReason.RejectChanges );
        }

        protected ChangeRejectedEventArgs<T> CreateMock<T>( object entity, T cachedValue, IChange source, RejectReason reason )
        {
            return new ChangeRejectedEventArgs<T>( entity, cachedValue, source, reason );
        }

        [TestMethod]
        public void changeRejectedArgs_generic_ctor_normal_should_correctly_set_values()
        {
            var entity = new Object();
            var cachedValue = "foo";
            var iChange = new ChangeArgsTests.FakeChange<String>(entity, cachedValue, v => { }, v => { });
            var reason = RejectReason.Redo;

            var target = this.CreateMock(entity, cachedValue, iChange, reason);

            Assert.AreEqual(entity, target.Entity);
            Assert.AreEqual(cachedValue, target.CachedValue);
            Assert.AreEqual(iChange, target.Source);
            Assert.AreEqual(reason, target.Reason);
        }
    }
}
