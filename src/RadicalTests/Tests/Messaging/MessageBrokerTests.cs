using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Radical.Messaging;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;

namespace RadicalTests.Windows.Messaging
{
    [TestClass]
	public class MessageBrokerTests
	{
		class PocoTestMessage
		{

		}

		class PocoMessageDerivedFromTestMessage : PocoTestMessage
		{

		}

		class AnotherPocoTestMessage
		{

		}

		[TestMethod]
		[TestCategory( "MessageBroker" )]
		public async Task messageBroker_POCO_unsubscribe_specific_subscriber_should_remove_only_subscriptions_for_that_subscriber()
		{
			const int expected = 1;
			var actual = 0;

            var target = new MessageBroker(CoreApplication.CreateNewView().CoreWindow.Dispatcher);

			var subscriber1 = new Object();
			var subscriber2 = new Object();

			target.Subscribe<PocoTestMessage>( subscriber1, ( s, msg ) => { actual++; } );
			target.Subscribe<PocoTestMessage>( subscriber1, ( s, msg ) => { actual++; } );
			target.Subscribe<PocoTestMessage>( subscriber1, ( s, msg ) => { actual++; } );

			target.Subscribe<PocoTestMessage>( subscriber2, ( s, msg ) => { actual++; } );

			target.Unsubscribe( subscriber1 );

			await target.DispatchAsync( this, new PocoTestMessage() );

			Assert.AreEqual(expected, actual);
		}

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task messageBroker_POCO_unsubscribe_specific_subscriber_and_specific_messageType_should_remove_only_subscriptions_for_that_subscriber()
        {
            const int expected = 1;
            var actual = 0;
            
            var target = new MessageBroker(CoreApplication.CreateNewView().CoreWindow.Dispatcher);

            var subscriber = new Object();

            target.Subscribe<PocoTestMessage>(subscriber, (s, msg) => { actual++; });
            target.Subscribe<AnotherPocoTestMessage>(subscriber, (s, msg) => { actual++; });

            target.Unsubscribe<AnotherPocoTestMessage>(subscriber);

            await target.DispatchAsync(this, new PocoTestMessage());

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task messageBroker_POCO_Dispatch_valid_message_should_not_fail()
        {
            var broker = new MessageBroker(CoreApplication.CreateNewView().CoreWindow.Dispatcher);

            await broker.DispatchAsync(this, new PocoTestMessage());
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public void messageBroker_POCO_subscribe_using_null_action_should_raise_ArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => 
            {
                var broker = new MessageBroker(CoreApplication.CreateNewView().CoreWindow.Dispatcher);

                broker.Subscribe(this, (Action<Object, PocoTestMessage>)null);
            });
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public void messageBroker_POCO_Subscribe_based_on_message_type_should_not_fail()
        {
            
            var broker = new MessageBroker(CoreApplication.CreateNewView().CoreWindow.Dispatcher);

            broker.Subscribe<PocoTestMessage>(this, (s, m) => { });
        }

        [TestMethod]
        public async Task messageBroker_POCO_subscribe_normal_should_notify()
        {
            var expected = true;
            var actual = false;

            
            var broker = new MessageBroker(CoreApplication.CreateNewView().CoreWindow.Dispatcher);

            broker.Subscribe<PocoTestMessage>(this, (s, msg) => actual = true);
            await broker.DispatchAsync(this, new PocoTestMessage());

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task MessageBroker_POCO_subscriber_using_a_base_class_should_be_dispatched_using_a_derived_class_message()
        {
            var actual = false;

            
            var broker = new MessageBroker(CoreApplication.CreateNewView().CoreWindow.Dispatcher);

            broker.Subscribe<Object>(this, (s, msg) => actual = true);
            await broker.DispatchAsync(this, new PocoTestMessage());

            Assert.IsTrue(actual);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task MessageBroker_POCO_subscriber_using_a_base_class_should_be_dispatched_using_a_derived_class_message_even_using_different_messages()
        {
            var actual = 0;

            
            var broker = new MessageBroker(CoreApplication.CreateNewView().CoreWindow.Dispatcher);

            broker.Subscribe<Object>(this, (s, msg) => actual++);
            await broker.DispatchAsync(this, new PocoTestMessage());
            await broker.DispatchAsync(this, new AnotherPocoTestMessage());

            Assert.AreEqual(2, actual);
        }
        
        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task MessageBroker_POCO_subscriber_using_a_base_class_should_be_dispatched_only_to_the_expected_inheritance_chain()
        {
            var actual = 0;

            
            var broker = new MessageBroker(CoreApplication.CreateNewView().CoreWindow.Dispatcher);

            broker.Subscribe<PocoTestMessage>(this, (s, m) => actual++);
            await broker.DispatchAsync(this, new PocoMessageDerivedFromTestMessage());
            await broker.DispatchAsync(this, new AnotherPocoTestMessage());

            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        [TestCategory("MessageBroker")]
        public async Task messageBroker_POCO_broadcast_async_should_not_fail()
        {
            var expected = 4;
            var actual = 0;

            
            var broker = new MessageBroker(CoreApplication.CreateNewView().CoreWindow.Dispatcher);

            broker.Subscribe<PocoTestMessage>(this, (s, msg) =>
            {
                actual++;
            });

            broker.Subscribe<PocoTestMessage>(this, (s, msg) =>
            {
                actual++;
            });

            broker.Subscribe<PocoTestMessage>(this, (s, msg) =>
            {
                actual++;
            });

            await broker.BroadcastAsync(this, new PocoTestMessage());

            actual++;

            Assert.AreEqual(expected, actual);
        }
    }
}