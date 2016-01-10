namespace RadicalTests.ChangeTracking
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Radical.ComponentModel.ChangeTracking;
    using Radical.ChangeTracking;

    [TestClass]
    public class AtomicChangesChangeTrackingServiceTests
    {
        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void changeTrackingService_beginAtomicOperation_normal_should_create_valid_atomic_operation()
        {
            var target = new ChangeTrackingService();
            var actual = target.BeginAtomicOperation();

            Assert.IsNotNull(actual);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void changeTrackingService_using_beginAtomicOperation_should_set_is_changed_only_on_operation_completed()
        {
            var target = new ChangeTrackingService();

            var person = new Person(target);
            var list = new PersonCollection(target);

            using(var actual = target.BeginAtomicOperation())
            {
                person.Name = "Mauro";
                list.Add(person);
                person.Name = "Mauro Servienti";

                Assert.IsFalse(target.IsChanged);

                actual.Complete();
            }

            Assert.IsTrue(target.IsChanged);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void changeTrackingService_using_atomicOperation_after_operation_complete_entityState_should_be_changed()
        {
            var target = new ChangeTrackingService();

            var person = new Person(target);

            target.AcceptChanges();

            using(var op = target.BeginAtomicOperation())
            {
                person.Name = "Mauro";

                op.Complete();
            }

            var state = target.GetEntityState(person);

            var actual = (state & EntityTrackingStates.HasBackwardChanges) == EntityTrackingStates.HasBackwardChanges;
            Assert.IsTrue(actual);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void changeTrackingService_using_beginAtomicOperation_should_fully_rollback_on_single_undo()
        {
            var target = new ChangeTrackingService();

            var person = new Person(target);
            var list = new PersonCollection(target);

            using(var actual = target.BeginAtomicOperation())
            {
                person.Name = "Mauro";
                list.Add(person);
                person.Name = "Mauro Servienti";

                actual.Complete();
            }

            target.Undo();

            Assert.AreEqual(0, list.Count);
            Assert.AreEqual(String.Empty, person.Name);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void changeTrackingService_using_beginAtomicOperation_redo_should_reapply_all_changes_with_one_pass()
        {
            var target = new ChangeTrackingService();

            var person = new Person(target);
            var list = new PersonCollection(target);

            using(var actual = target.BeginAtomicOperation())
            {
                person.Name = "Mauro";
                list.Add(person);
                person.Name = "Mauro Servienti";

                actual.Complete();
            }

            target.Undo();
            target.Redo();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Mauro Servienti", person.Name);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void changeTrackingService_using_beginAtomicOperation_undo_redo_should_restore_in_one_pass()
        {
            var target = new ChangeTrackingService();

            var person = new Person(target);
            var list = new PersonCollection(target);

            using(var actual = target.BeginAtomicOperation())
            {
                person.Name = "Mauro";
                list.Add(person);
                person.Name = "Mauro Servienti";

                actual.Complete();
            }

            target.Undo();
            target.Redo();
            target.Undo();

            Assert.AreEqual(0, list.Count);
            Assert.AreEqual(String.Empty, person.Name);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void changeTrackingService_using_beginAtomicOperation_getEntityState_should_return_valid_entity_state()
        {
            var expected = EntityTrackingStates.HasBackwardChanges | EntityTrackingStates.IsTransient | EntityTrackingStates.AutoRemove;
            var target = new ChangeTrackingService();

            var person = new Person(target);
            var list = new PersonCollection(target);

            using(var op = target.BeginAtomicOperation())
            {
                person.Name = "Mauro";
                list.Add(person);
                person.Name = "Mauro Servienti";

                op.Complete();
            }

            var actual = target.GetEntityState(person);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void changeTrackingService_using_beginAtomicOperation_getEntities_should_return_all_changed_entities()
        {
            var target = new ChangeTrackingService();

            var person = new Person(target);
            var list = new PersonCollection(target);

            using(var op = target.BeginAtomicOperation())
            {
                person.Name = "Mauro";
                list.Add(person);
                person.Name = "Mauro Servienti";

                op.Complete();
            }

            var actual = target.GetEntities(EntityTrackingStates.HasBackwardChanges, false);

            /*
             * Non funziona perchè non funziona GetEntityState()
             */

            Assert.IsTrue(actual.Contains(person));
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void changeTrackingService_using_beginAtomicOperation_getEntities_should_return_all_transient_entities()
        {
            var target = new ChangeTrackingService();

            var list = new PersonCollection(target);
            var person = new Person(target, false);

            using(var op = target.BeginAtomicOperation())
            {
                target.RegisterTransient(person);
                person.Name = "Mauro";
                list.Add(person);
                person.Name = "Mauro Servienti";

                op.Complete();
            }

            var actual = target.GetEntities(EntityTrackingStates.IsTransient, false);

            Assert.IsTrue(actual.Contains(person));
        }

        [TestMethod]
        [TestCategory("ChangeTracking")]
        public void changeTrackingService_using_beginAtomicOperation_hasTransientEntities_should_return_true_even_for_entities_created_in_the_atomic_operation()
        {
            var target = new ChangeTrackingService();

            var list = new PersonCollection(target);
            var person = new Person(target, false);

            using(var op = target.BeginAtomicOperation())
            {
                target.RegisterTransient(person);
                person.Name = "Mauro";
                list.Add(person);
                person.Name = "Mauro Servienti";

                op.Complete();
            }


            Assert.IsTrue(target.HasTransientEntities);
        }
    }
}
