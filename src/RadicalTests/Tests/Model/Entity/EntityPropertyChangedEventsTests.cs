//extern alias tpx;

namespace RadicalTests.Model.Entity
{
    using Radical.Model;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using System.ComponentModel;
    using System;

    [TestClass()]
    public class EntityPropertyChangedEventsTests : EntityTests
    {
        public sealed class TestableEntity : Entity
        {
            internal void RaisePropertyChanged(PropertyChangedEventArgs e)
            {
                this.OnPropertyChanged(e);
            }

            internal void RaisePropertyChanged(String propertyName)
            {
                this.OnPropertyChanged(propertyName);
            }
        }

        protected override Entity CreateMock()
        {
            return this.CreateTestableEntityMock();
        }

        protected virtual TestableEntity CreateTestableEntityMock()
        {
            return new TestableEntity();
        }

        [TestMethod]
        public void entity_propertyChanged_event_using_propertyChangedEventArgs_raised_with_expected_values()
        {
            var expected = "Foo";
            var actual = String.Empty;

            var target = this.CreateTestableEntityMock();
            target.PropertyChanged += (s, e) => { actual = e.PropertyName; };
            target.RaisePropertyChanged(new PropertyChangedEventArgs(expected));

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void entity_propertyChanged_event_using_propertyName_raised_with_expected_values()
        {
            var expected = "Foo";
            var actual = String.Empty;

            var target = this.CreateTestableEntityMock();
            target.PropertyChanged += (s, e) => { actual = e.PropertyName; };
            target.RaisePropertyChanged(expected);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void entity_propertyChanged_event_on_disposed_entity_using_propertyChangedEventArgs_should_raise_ObjectDisposedException()
        {
            Assert.ThrowsException<ObjectDisposedException>(() =>
            {
                var expected = "Foo";

                var target = this.CreateTestableEntityMock();
                target.Dispose();
                target.RaisePropertyChanged(new PropertyChangedEventArgs(expected));
            });
        }

        [TestMethod]
        public void entity_propertyChanged_event_on_disposed_entity_using_propertyName_should_raise_ObjectDisposedException()
        {
            Assert.ThrowsException<ObjectDisposedException>(() => 
            {
                var expected = "Foo";

                var target = this.CreateTestableEntityMock();
                target.Dispose();
                target.RaisePropertyChanged(expected);
            });
        }
    }
}
