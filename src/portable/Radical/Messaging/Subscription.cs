using System;
using Radical.ComponentModel;
using Radical.ComponentModel.Messaging;
using Radical.Validation;

namespace Radical.Messaging
{
	interface ISubscription
	{
		object Subscriber { get; }
		object Sender { get; }

		InvocationModel InvocationModel { get; }

		Delegate GetAction();

		/// <summary>
		/// The subscriber invocation model is based on the <see cref="InvocationModel"/> property.
		/// </summary>
		/// <param name="sender">The message sender.</param>
		/// <param name="message">The message.</param>
		void Invoke( object sender, object message );
	}

    class Subscription : ISubscription
    {
        readonly IDispatcher dispatcher;
        readonly Delegate action;

        public Subscription( object subscriber, Delegate action, InvocationModel invocationModel, IDispatcher dispatcher )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( action ).Named( () => action ).IsNotNull();
            Ensure.That( dispatcher ).Named( () => dispatcher ).IsNotNull();

            this.Subscriber = subscriber;
            this.action = action;
            this.Sender = null;
            this.InvocationModel = invocationModel;
            this.dispatcher = dispatcher;
        }

        public Subscription( object subscriber, object sender, Delegate action, InvocationModel invocationModel, IDispatcher dispatcher )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( sender ).Named( () => sender ).IsNotNull();
            Ensure.That( action ).Named( () => action ).IsNotNull();
            Ensure.That( dispatcher ).Named( () => dispatcher ).IsNotNull();

            this.Subscriber = subscriber;
            this.action = action;
            this.Sender = sender;
            this.InvocationModel = invocationModel;
            this.dispatcher = dispatcher;
        }

        public Delegate GetAction()
        {
            return this.action;
        }

        public object Subscriber { get; private set; }

        public object Sender { get; private set; }

        public InvocationModel InvocationModel { get; private set; }

        public void Invoke( object sender, object message )
        {
            if ( this.InvocationModel == InvocationModel.Safe && !dispatcher.IsSafe )
            {
                //dispatcher.RunAsync( CoreDispatcherPriority.Normal, () => this.action.DynamicInvoke( sender, message ) )
                //    .AsTask()
                //    .Wait();
	            dispatcher.Dispatch(() => this.action.DynamicInvoke(sender, message));
            }
            else
            {
                this.action.DynamicInvoke( sender, message );
            }
        }
    }
}