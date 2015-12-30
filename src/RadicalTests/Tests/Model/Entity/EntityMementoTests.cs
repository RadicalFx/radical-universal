//extern alias tpx;

namespace RadicalTests.Model.Entity
{
    using Radical.Model;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Radical.ComponentModel;
    using Radical.ComponentModel.ChangeTracking;
    using System.ComponentModel;
    using Radical.ChangeTracking;

    [TestClass()]
    public class EntityMementoTests : EntityTests
    {
        class SutMementoEntity : MementoEntity
        {
            public SutMementoEntity()
            {
            }

            public SutMementoEntity(bool registerAsrTransient)
                : base(registerAsrTransient)
            {

            }

            public SutMementoEntity(IChangeTrackingService memento)
                : base(memento)
            {

            }

            public SutMementoEntity(IChangeTrackingService memento, bool registerAsrTransient)
                : base(memento, registerAsrTransient)
            {

            }
        }

        protected override Entity CreateMock()
        {
            var entity = new SutMementoEntity();

            return entity;
        }

        protected virtual MementoEntity CreateMock(bool registerAsrTransient)
        {
            var entity = new SutMementoEntity(registerAsrTransient);

            return entity;
        }

        protected virtual MementoEntity CreateMock(IChangeTrackingService memento)
        {
            var entity = new SutMementoEntity(memento);

            return entity;
        }

        protected virtual MementoEntity CreateMock(IChangeTrackingService memento, bool registerAsrTransient)
        {
            var entity = new SutMementoEntity(memento, registerAsrTransient);

            return entity;
        }

        [TestMethod]
        public void entityMemento_ctor_default_set_default_values()
        {
            var target = (IMemento)this.CreateMock();

            Assert.IsNull(target.Memento);
        }

        [TestMethod]
        public void entityMemento_ctor_memento_set_default_values()
        {
            var expected = new ChangeTrackingService();
            var target = (IMemento)this.CreateMock(expected);
            var actual = target.Memento;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void entityMemento_ctor_registerAsTransient_true_set_default_values()
        {
            var target = (IMemento)this.CreateMock(true);
            var actual = target.Memento;
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void entityMemento_ctor_registerAsTransient_false_set_default_values()
        {
            var target = (IMemento)this.CreateMock(false);
            var actual = target.Memento;
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void entityMemento_ctor_memento_registerAsTransient_false_set_default_values()
        {
            var expected = new ChangeTrackingService();
            var target = (IMemento)this.CreateMock(expected, false);
            var actual = target.Memento;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void entityMemento_ctor_memento_registerAsTransient_true_set_default_values()
        {
            var expected = new ChangeTrackingService();
            var target = (IMemento)this.CreateMock(expected, true);
            var actual = target.Memento;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void entityMemento_ctor_requesting_transient_registration_successfully_register_entity_as_transient()
        {
            EntityTrackingStates expected = EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove;
            using(ChangeTrackingService svc = new ChangeTrackingService())
            {
                var target = this.CreateMock(true);
                ((IMemento)target).Memento = svc;

                EntityTrackingStates actual = svc.GetEntityState(target);

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void entityMemento_ctor_requesting_transient_registration_to_suspended_memento_do_not_register_entity_as_transient()
        {
            EntityTrackingStates expected = EntityTrackingStates.None;
            using(ChangeTrackingService svc = new ChangeTrackingService())
            {
                svc.Suspend();

                var target = this.CreateMock(true);
                ((IMemento)target).Memento = svc;

                EntityTrackingStates actual = svc.GetEntityState(target);

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void entityMemento_ctor_requesting_transient_registration_without_memento_do_not_fail()
        {
            var target = this.CreateMock(true);
        }

        [TestMethod]
        public void entityMemento_memento_using_base_iMemento_can_be_set_to_null()
        {
            var target = (IMemento)this.CreateMock(new ChangeTrackingService());
            target.Memento = null;

            Assert.IsNull(target.Memento);
        }
    }
}
