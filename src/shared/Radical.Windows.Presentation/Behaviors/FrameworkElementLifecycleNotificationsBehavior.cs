//using System.Windows;
//using Radical.ComponentModel.Messaging;
//using Radical.Reflection;
//using Radical.Windows.Behaviors;
//using Radical.Windows.Presentation.ComponentModel;
//using Radical.Windows.Presentation.Messaging;
//using Windows.UI.Xaml;

//namespace Radical.Windows.Presentation.Behaviors
//{
//    /// <summary>
//    /// Wires the FrameworkElement lifecycle to a view model that requires lifecycle notifications.
//    /// </summary>
//    public class FrameworkElementLifecycleNotificationsBehavior : RadicalBehavior<FrameworkElement>
//    {
//        readonly IMessageBroker broker;
//        readonly IConventionsHandler conventionsHandler;

//        RoutedEventHandler loaded = null;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="FrameworkElementLifecycleNotificationsBehavior"/> class.
//        /// </summary>
//        /// <param name="broker">The broker.</param>
//        /// <param name="conventionsHandler">The conventions handler.</param>
//        public FrameworkElementLifecycleNotificationsBehavior( IMessageBroker broker, IConventionsHandler conventionsHandler )
//        {
//            this.broker = broker;
//            this.conventionsHandler = conventionsHandler;

//            if ( !DesignTimeHelper.GetIsInDesignMode() )
//            {
//                this.loaded = ( s, e ) =>
//                {
//                    if ( this.conventionsHandler.ShouldNotifyViewModelLoaded( this.AssociatedObject, this.AssociatedObject.DataContext ) )
//                    {
//                        this.broker.Broadcast( new ViewModelLoaded( this, this.AssociatedObject.DataContext ) );
//                    }

//                    var dc = this.AssociatedObject.DataContext as IExpectViewLoadedCallback;
//                    if ( dc != null )
//                    {
//                        dc.OnViewLoaded();
//                    }
//                };
//            }
//        }

//        /// <summary>
//        /// Called after the behavior is attached to an AssociatedObject.
//        /// </summary>
//        protected override void OnAttached()
//        {
//            if ( !DesignTimeHelper.GetIsInDesignMode() )
//            {
//                this.AssociatedObject.Loaded += this.loaded;
//            }
//        }

//        /// <summary>
//        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
//        /// </summary>
//        protected override void OnDetaching()
//        {
//            if ( !DesignTimeHelper.GetIsInDesignMode() )
//            {
//                this.AssociatedObject.Loaded -= this.loaded;
//            }
//        }
//    }
//}
