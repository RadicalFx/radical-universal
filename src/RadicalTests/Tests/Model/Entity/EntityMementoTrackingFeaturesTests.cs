//extern alias tpx;

namespace RadicalTests.Model.Entity
{
    using Radical.Model;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Radical.ComponentModel.ChangeTracking;
    using Radical.ChangeTracking;
    using System;
    using System.Collections.Generic;
    using Radical;
    [TestClass()]
    public class EntityMementoTrackingFeaturesTests : EntityMementoTests
    {
        class FakeChange<T> : Change<T>
        {
            public FakeChange(Object owner, T valueToCache, RejectCallback<T> rejectCallback, CommitCallback<T> commitCallback, String description)
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

        public sealed class TestableMementoEntity : MementoEntity
        {
            public TestableMementoEntity()
                : base()
            {

            }

            public TestableMementoEntity(IChangeTrackingService memento)
                : base(memento, true)
            {

            }

            public TestableMementoEntity(Boolean registerAsTransient)
                : base(null, registerAsTransient)
            {

            }

            public TestableMementoEntity(IChangeTrackingService memento, Boolean registerAsTransient)
                : base(memento, registerAsTransient)
            {

            }

            internal Boolean GetIsTracking()
            {
                return base.IsTracking;
            }

            internal IChange InvokeCacheChange<T>(T value, RejectCallback<T> rc)
            {
                return base.CacheChange("property-name", value, rc);
            }

            internal IChange InvokeCacheChange<T>(T value, RejectCallback<T> rc, CommitCallback<T> cc)
            {
                return base.CacheChange("property-name", value, rc, cc);
            }

            internal IChange InvokeCacheChange<T>(T value, RejectCallback<T> rc, CommitCallback<T> cc, AddChangeBehavior behavior)
            {
                return base.CacheChange("property-name", value, rc, cc, behavior);
            }

            internal IChange InvokeCacheChangeOnRejectCallback<T>(T value, RejectCallback<T> rejectCallback, CommitCallback<T> commitCallback, ChangeRejectedEventArgs<T> args)
            {
                return base.CacheChangeOnRejectCallback("property-name", value, rejectCallback, commitCallback, args);
            }

            internal event EventHandler<MementoChangedEventArgs> MementoChanged;

            protected override void OnMementoChanged(IChangeTrackingService newMemento, IChangeTrackingService oldMemento)
            {
                base.OnMementoChanged(newMemento, oldMemento);

                if(this.MementoChanged != null)
                {
                    this.MementoChanged(this, new MementoChangedEventArgs(newMemento, oldMemento));
                }
            }
        }

        class ChangeTrackingServiceStub : ChangeTrackingService
        {

        }

        public class MementoChangedEventArgs : EventArgs
        {
            internal MementoChangedEventArgs(IChangeTrackingService newMemento, IChangeTrackingService oldMemento)
            {
                this.NewMemento = newMemento;
                this.OldMemento = oldMemento;
            }

            public readonly IChangeTrackingService NewMemento;
            public readonly IChangeTrackingService OldMemento;
        }

        protected override Entity CreateMock()
        {
            return this.CreateTestableEntityMock();
        }

        protected virtual TestableMementoEntity CreateTestableEntityMock()
        {
            return new TestableMementoEntity();
        }

        protected override MementoEntity CreateMock(bool registerAsrTransient)
        {
            return this.CreateTestableEntityMock(registerAsrTransient);
        }

        protected virtual TestableMementoEntity CreateTestableEntityMock(bool registerAsrTransient)
        {
            return new TestableMementoEntity(registerAsrTransient);
        }

        protected override MementoEntity CreateMock(IChangeTrackingService memento)
        {
            return this.CreateTestableEntityMock(memento);
        }

        protected virtual TestableMementoEntity CreateTestableEntityMock(IChangeTrackingService memento)
        {
            return new TestableMementoEntity(memento);
        }

        protected override MementoEntity CreateMock(IChangeTrackingService memento, bool registerAsrTransient)
        {
            return this.CreateTestableEntityMock(memento, registerAsrTransient);
        }

        protected virtual TestableMementoEntity CreateTestableEntityMock(IChangeTrackingService memento, bool registerAsrTransient)
        {
            return new TestableMementoEntity(memento, registerAsrTransient);
        }

        [TestMethod]
        public void entityMemento_isTracking_without_service_should_be_false()
        {
            var target = this.CreateTestableEntityMock();
            Assert.IsFalse(target.GetIsTracking());
        }

        [TestMethod]
        public void entityMemento_isTracking_without_service_and_with_registerAsTransient_request_true_should_be_false()
        {
            var target = this.CreateTestableEntityMock(true);
            Assert.IsFalse(target.GetIsTracking());
        }

        [TestMethod]
        public void entityMemento_isTracking_without_service_and_with_registerAsTransient_request_false_should_be_false()
        {
            var target = this.CreateTestableEntityMock(false);
            Assert.IsFalse(target.GetIsTracking());
        }

        [TestMethod]
        public void entityMemento_isTracking_with_service_should_be_true()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock(memento);
            Assert.IsTrue(target.GetIsTracking());
        }

        [TestMethod]
        public void entityMemento_isTracking_with_service_and_registerAsTransient_true_should_be_true()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock(memento, true);
            Assert.IsTrue(target.GetIsTracking());
        }

        [TestMethod]
        public void entityMemento_isTracking_with_service_and_registerAsTransient_false_should_be_true()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock(memento, false);
            Assert.IsTrue(target.GetIsTracking());
        }

        [TestMethod]
        public void entityMemento_isTracking_with_suspended_service_should_be_false()
        {
            var memento = new ChangeTrackingService();
            memento.Suspend();

            var target = this.CreateTestableEntityMock(memento);
            Assert.IsFalse(target.GetIsTracking());
        }

        [TestMethod]
        public void entityMemento_isTracking_with_suspended_serviceand_registerAsTransient_false_should_be_false()
        {
            var memento = new ChangeTrackingService();
            memento.Suspend();

            var target = this.CreateTestableEntityMock(memento, false);
            Assert.IsFalse(target.GetIsTracking());
        }

        [TestMethod]
        public void entityMemento_isTracking_with_suspended_serviceand_registerAsTransient_true_should_be_false()
        {
            var memento = new ChangeTrackingService();
            memento.Suspend();

            var target = this.CreateTestableEntityMock(memento, true);
            Assert.IsFalse(target.GetIsTracking());
        }

        [TestMethod]
        public void entityMemento_memento_normal_onMementoChanged_is_invoked_with_expected_values()
        {
            IChangeTrackingService expectedNew = new ChangeTrackingService();
            IChangeTrackingService expectedOld = null;
            IChangeTrackingService actualNew = null;
            IChangeTrackingService actualOld = null;

            var target = this.CreateTestableEntityMock();

            target.MementoChanged += (s, e) =>
            {
                actualNew = e.NewMemento;
                actualOld = e.OldMemento;
            };
            ((IMemento)target).Memento = expectedNew;

            Assert.AreEqual(expectedNew, actualNew);
            Assert.AreEqual(expectedOld, actualOld);
        }

        [TestMethod]
        public void entityMemento_memento_changing_current_onMementoChanged_is_invoked_with_expected_values()
        {
            IChangeTrackingService expectedNew = new ChangeTrackingService();
            IChangeTrackingService expectedOld = new ChangeTrackingService();
            IChangeTrackingService actualNew = null;
            IChangeTrackingService actualOld = null;

            var target = this.CreateTestableEntityMock(expectedOld);

            target.MementoChanged += (s, e) =>
            {
                actualNew = e.NewMemento;
                actualOld = e.OldMemento;
            };
            ((IMemento)target).Memento = expectedNew;

            Assert.AreEqual(expectedNew, actualNew);
            Assert.AreEqual(expectedOld, actualOld);
        }

        [TestMethod]
        public void entityMemento_memento_normal_onMementoChanged_is_not_invoked_setting_same_reference()
        {
            var expected = 0;
            var actual = 0;

            var memento = new ChangeTrackingService();

            var target = this.CreateTestableEntityMock(memento);

            target.MementoChanged += (s, e) => { actual++; };
            ((IMemento)target).Memento = memento;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void entityMemento_memento_changing_on_disposed_entity_should_raise_ObjectDisposedException()
        {
            Assert.ThrowsException<ObjectDisposedException>(() =>
            {
                var memento = new ChangeTrackingService();
                var target = this.CreateTestableEntityMock(memento);
                target.Dispose();
                ((IMemento)target).Memento = null;
            });
        }

        [TestMethod]
        public void entityMemento_memento_changing_on_disposed_entity_using_base_iMemento_should_raise_ObjectDisposedException()
        {
            Assert.ThrowsException<ObjectDisposedException>(() =>
            {
                var memento = new ChangeTrackingService();
                var target = this.CreateTestableEntityMock(memento);
                target.Dispose();
                ((IMemento)target).Memento = null;
            });
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_should_be_called()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock(memento);

            var change = target.InvokeCacheChange("Foo", cv => { });

            Assert.IsNotNull(change);
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_should_be_called_with_expected_values()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock(memento);

            var change = target.InvokeCacheChange("Foo", cv => { });

            Assert.IsFalse(change.IsCommitSupported);
            Assert.AreEqual(target, change.Owner);
            Assert.AreEqual(String.Empty, change.Description);
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_on_disposed_object_should_raise_ObjectDisposedException()
        {
            Assert.ThrowsException<ObjectDisposedException>(() =>
            {
                var memento = new ChangeTrackingService();
                var target = this.CreateTestableEntityMock(memento);
                target.Dispose();

                target.InvokeCacheChange("Foo", cv => { });
            });
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_without_service_should_not_be_called()
        {
            var target = this.CreateTestableEntityMock();

            var change = target.InvokeCacheChange("Foo", cv => { });

            Assert.IsNull(change);
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_with_suspended_service_should_not_be_called()
        {
            var memento = new ChangeTrackingService();
            memento.Suspend();

            var target = this.CreateTestableEntityMock(memento);

            var change = target.InvokeCacheChange("Foo", cv => { });

            Assert.IsNull(change);
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_should_be_called()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock(memento);

            var change = target.InvokeCacheChange("Foo", cv => { }, cv => { });

            Assert.IsNotNull(change);
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_should_be_called_with_expected_values()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock(memento);

            var change = target.InvokeCacheChange("Foo", cv => { }, cv => { });

            Assert.IsTrue(change.IsCommitSupported);
            Assert.AreEqual(target, change.Owner);
            Assert.AreEqual(String.Empty, change.Description);
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_on_disposed_object_should_raise_ObjectDisposedException()
        {
            Assert.ThrowsException<ObjectDisposedException>(() =>
            {
                var memento = new ChangeTrackingService();
                var target = this.CreateTestableEntityMock(memento);
                target.Dispose();

                target.InvokeCacheChange("Foo", cv => { }, cv => { });
            });
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_without_service_should_not_be_called()
        {
            var target = this.CreateTestableEntityMock();

            var change = target.InvokeCacheChange("Foo", cv => { }, cv => { });

            Assert.IsNull(change);
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_with_suspended_service_should_not_be_called()
        {
            var memento = new ChangeTrackingService();
            memento.Suspend();

            var target = this.CreateTestableEntityMock(memento);

            var change = target.InvokeCacheChange("Foo", cv => { }, cv => { });

            Assert.IsNull(change);
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_addChangeBeahvior_should_be_called()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock(memento);

            var change = target.InvokeCacheChange("Foo", cv => { }, cv => { }, AddChangeBehavior.Default);

            Assert.IsNotNull(change);
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_addChangeBeahvior_should_be_called_with_expected_values()
        {
            var memento = new ChangeTrackingService();
            var target = this.CreateTestableEntityMock(memento);

            var change = target.InvokeCacheChange("Foo", cv => { }, cv => { }, AddChangeBehavior.Default);

            Assert.IsTrue(change.IsCommitSupported);
            Assert.AreEqual(target, change.Owner);
            Assert.AreEqual(String.Empty, change.Description);
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_addChangeBeahvior_on_disposed_object_should_raise_ObjectDisposedException()
        {
            Assert.ThrowsException<ObjectDisposedException>(() =>
            {
                var memento = new ChangeTrackingService();
                var target = this.CreateTestableEntityMock(memento);
                target.Dispose();

                target.InvokeCacheChange("Foo", cv => { }, cv => { }, AddChangeBehavior.Default);
            });
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_addChangeBeahvior_without_service_should_not_be_called()
        {
            var target = this.CreateTestableEntityMock();

            var change = target.InvokeCacheChange("Foo", cv => { }, cv => { }, AddChangeBehavior.Default);

            Assert.IsNull(change);
        }

        [TestMethod]
        public void entityMemento_chacheChange_value_rejectCallback_commitCallback_addChangeBeahvior_with_suspended_service_should_not_be_called()
        {
            var memento = new ChangeTrackingService();
            memento.Suspend();

            var target = this.CreateTestableEntityMock(memento);

            var change = target.InvokeCacheChange("Foo", cv => { }, cv => { }, AddChangeBehavior.Default);

            Assert.IsNull(change);
        }

        [TestMethod]
        public void entityMemento_cacheChangeOnRejectCallback_rejectReason_Undo_should_return_valid_iChange()
        {
            var memento = new ChangeTrackingService();
            var value = "foo";
            var target = this.CreateTestableEntityMock(memento);
            var change = new FakeChange<String>(target, value, rc => { }, cc => { }, "");

            var rejArgs = new ChangeRejectedEventArgs<String>(target, value, change, RejectReason.Undo);

            var actual = target.InvokeCacheChangeOnRejectCallback(value, cv => { }, cv => { }, rejArgs);

            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void entityMemento_cacheChangeOnRejectCallback_rejectReason_Redo_should_return_valid_iChange()
        {
            var memento = new ChangeTrackingService();
            var value = "foo";
            var target = this.CreateTestableEntityMock(memento);
            var change = new FakeChange<String>(target, value, rc => { }, cc => { }, "");

            var rejArgs = new ChangeRejectedEventArgs<String>(target, value, change, RejectReason.Redo);

            var actual = target.InvokeCacheChangeOnRejectCallback(value, cv => { }, cv => { }, rejArgs);

            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void entityMemento_cacheChangeOnRejectCallback_rejectReason_Revert_should_return_null_iChange()
        {
            var memento = new ChangeTrackingService();
            var value = "foo";
            var target = this.CreateTestableEntityMock(memento);
            var change = new FakeChange<String>(target, value, rc => { }, cc => { }, "");

            var rejArgs = new ChangeRejectedEventArgs<String>(target, value, change, RejectReason.Revert);

            var actual = target.InvokeCacheChangeOnRejectCallback(value, cv => { }, cv => { }, rejArgs);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void entityMemento_cacheChangeOnRejectCallback_rejectReason_RejectChanges_should_return_null_iChange()
        {
            var memento = new ChangeTrackingService();
            var value = "foo";
            var target = this.CreateTestableEntityMock(memento);
            var change = new FakeChange<String>(target, value, rc => { }, cc => { }, "");

            var rejArgs = new ChangeRejectedEventArgs<String>(target, value, change, RejectReason.RejectChanges);

            var actual = target.InvokeCacheChangeOnRejectCallback(value, cv => { }, cv => { }, rejArgs);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void entityMemento_chacheChangeOnRejectCallback_on_disposed_entity_should_raise_ObjectDisposedException()
        {
            Assert.ThrowsException<ObjectDisposedException>(() =>
            {
                var memento = new ChangeTrackingService();
                var value = "foo";
                var target = this.CreateTestableEntityMock(memento);
                var change = new FakeChange<String>(target, value, rc => { }, cc => { }, "");


                target.Dispose();

                var rejArgs = new ChangeRejectedEventArgs<String>(target, value, change, RejectReason.Revert);

                target.InvokeCacheChangeOnRejectCallback(value, cv => { }, cv => { }, rejArgs);
            });
        }

        [TestMethod]
        public void entityMemento_cacheChangeOnRejectCallback_rejectReason_none_should_raise_ArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var value = "foo";
                var memento = new ChangeTrackingService();
                var target = this.CreateTestableEntityMock(memento);
                var change = new FakeChange<String>(target, value, rc => { }, cc => { }, "");

                var rejArgs = new ChangeRejectedEventArgs<String>(target, value, change, RejectReason.None);

                target.InvokeCacheChangeOnRejectCallback(value, cv => { }, cv => { }, rejArgs);
            });
        }

        [TestMethod]
        public void entityMemento_cacheChangeOnRejectCallback_invalid_rejectReason_should_raise_EnumValueOutOfRangeException()
        {
            Assert.ThrowsException<EnumValueOutOfRangeException>(() =>
            {
                var value = "foo";
                var memento = new ChangeTrackingService();
                var target = this.CreateTestableEntityMock(memento);
                var change = new FakeChange<String>(target, value, rc => { }, cc => { }, "");

                var rejArgs = new ChangeRejectedEventArgs<String>(target, value, change, (RejectReason)1000);

                target.InvokeCacheChangeOnRejectCallback(value, cv => { }, cv => { }, rejArgs);
            });
        }
    }
}
