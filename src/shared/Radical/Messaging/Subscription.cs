using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radical.ComponentModel.Messaging;
using Radical.Validation;
using Radical.ComponentModel;
using Windows.UI.Core;

namespace Radical.Messaging
{
    interface ISubscription
    {
        Object Subscriber { get; }
        Object Sender { get; }

        InvocationModel InvocationModel { get; }

        Delegate GetAction();

        /// <summary>
        /// The subscriber invocation model is based on the <see cref="InvocationModel"/> property.
        /// </summary>
        /// <param name="message">The message.</param>
        void Invoke(Object sender, Object message);
    }

    class Subscription : ISubscription
    {
        readonly CoreDispatcher dispatcher;
        readonly Delegate action;

        public Subscription(Object subscriber, Delegate action, InvocationModel invocationModel, CoreDispatcher dispatcher)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(action).Named(() => action).IsNotNull();
            Ensure.That(dispatcher).Named(() => dispatcher).IsNotNull();

            this.Subscriber = subscriber;
            this.action = action;
            this.Sender = null;
            this.InvocationModel = invocationModel;
            this.dispatcher = dispatcher;
        }

        public Subscription(Object subscriber, Object sender, Delegate action, InvocationModel invocationModel, CoreDispatcher dispatcher)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(sender).Named(() => sender).IsNotNull();
            Ensure.That(action).Named(() => action).IsNotNull();
            Ensure.That(dispatcher).Named(() => dispatcher).IsNotNull();

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

        public Object Subscriber { get; private set; }

        public Object Sender { get; private set; }

        public InvocationModel InvocationModel { get; private set; }

        public void Invoke(Object sender, Object message)
        {
            if(this.InvocationModel == InvocationModel.Safe && !dispatcher.HasThreadAccess)
            {
                dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.action.DynamicInvoke(sender, message))
                    .AsTask()
                    .Wait();
            }
            else
            {
                this.action.DynamicInvoke(sender, message);
            }
        }
    }
}