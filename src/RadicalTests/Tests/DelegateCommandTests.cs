namespace RadicalTests.Windows
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Radical.ChangeTracking;
    using Radical.Observers;
    using Radical.Windows.Input;
    using System;
    using System.ComponentModel;

    [TestClass]
    public class DelegateCommandTests
    {
        class TestStub : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged( String name )
            {
                if( this.PropertyChanged != null )
                {
                    this.PropertyChanged( this, new PropertyChangedEventArgs( name ) );
                }
            }

            private String _value = null;
            public String Value
            {
                get { return this._value; }
                set
                {
                    if( value != this.Value )
                    {
                        this._value = value;
                        this.OnPropertyChanged( "Value" );
                    }
                }
            }

            private String _anotherValue = null;
            public String AnotherValue
            {
                get { return this._anotherValue; }
                set
                {
                    if( value != this.AnotherValue )
                    {
                        this._anotherValue = value;
                        this.OnPropertyChanged( "AnotherValue" );
                    }
                }
            }
        }

        class MockedChangeTrackingService: ChangeTrackingService
        {
            public void RaiseTrackingServiceStateChanged()
            {
                this.OnTrackingServiceStateChanged();
            }
        }

        [Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethod]
        public void delegateCommand_trigger_using_mementoMonitor_and_manually_calling_notifyChanged_should_raise_CanExecuteChanged()
        {
            var expected = 1;
            var actual = 0;

            var svc = new ChangeTrackingService();
            var monitor = new MementoMonitor( svc );

            var target = DelegateCommand.Create().AddMonitor( monitor );
            target.CanExecuteChanged += ( s, e ) => actual++;
            monitor.NotifyChanged();

            Assert.AreEqual(expected, actual);
        }

        [Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethod]
        public void delegateCommand_trigger_using_mementoMonitor_and_triggering_changes_on_the_memento_should_raise_canExecuteChanged()
        {
            var expected = 1;
            var actual = 0;

            var svc = new MockedChangeTrackingService();
            var monitor = new MementoMonitor(svc);

            var target = DelegateCommand.Create().AddMonitor(monitor);
            target.CanExecuteChanged += (s, e) => actual++;

            svc.RaiseTrackingServiceStateChanged();

            Assert.AreEqual(expected, actual);
        }

        [Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethod]
        public void delegateCommand_trigger_using_PropertyObserver_ForAllproperties_should_trigger_canExecuteChanged()
        {
            var expected = 2;
            var actual = 0;

            var stub = new TestStub();

            var target = DelegateCommand.Create().Observe(stub);
            target.CanExecuteChanged += (s, e) => actual++;

            stub.Value = "this raises PropertyChanged";
            stub.AnotherValue = "this raises PropertyChanged";

            Assert.AreEqual(expected, actual);
        }

        [Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethod]
        public void delegateCommand_trigger_using_PropertyObserver_For_a_property_should_trigger_canExecuteChanged()
        {
            var expected = 1;
            var actual = 0;

            var stub = new TestStub();

            var target = DelegateCommand.Create().Observe(stub, s => s.Value);
            target.CanExecuteChanged += (s, e) => actual++;

            stub.Value = "this raises PropertyChanged";
            stub.AnotherValue = "this raises PropertyChanged";

            Assert.AreEqual(expected, actual);
        }
    }
}
