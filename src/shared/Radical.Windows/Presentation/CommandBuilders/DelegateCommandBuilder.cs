using Radical.ComponentModel;
using Radical.ComponentModel.Windows.Input;
using Radical.Observers;
using Radical.Reflection;
using Radical.Windows.Behaviors;
using Radical.Windows.Input;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;

namespace Radical.Windows.CommandBuilders
{
    public class DelegateCommandBuilder
    {
        public virtual Boolean CanCreateCommand( PropertyPath path, DependencyObject target )
        {
            if ( DesignTimeHelper.GetIsInDesignMode() )
            {
                return false;
            }

            return path != null && target is FrameworkElement;
        }

        public virtual Object GetDataContext( DependencyObject target )
        {
            return ((FrameworkElement)target).DataContext;
        }

        static String GetExpectedMethodName( String path )
        {
            //WARN: questo è un bug: impedisce di chiamare effettivamente il metodo nel VM qualcosa del tipo FooCommand perchè noi cercheremmo solo Foo
            var methodName = path.EndsWith( "Command" ) ? path.Remove( path.LastIndexOf( "Command" ) ) : path;

            return methodName;
        }

        //static Object GetNestedContextIfAny( Object context, String path )
        //{
        //    var segements = path.Split( new[] { '.' }, StringSplitOptions.RemoveEmptyEntries );
        //    if ( context != null && segements.Length > 1 )
        //    {

        //    }

        //    return context;
        //}

        /// <summary>
        /// Tries to generate command data.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="dataContext">The data context.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public virtual Boolean TryGenerateCommandData( PropertyPath path, Object dataContext, out CommandData data )
        {
            var propertyPath = path.Path;
            var nestedProperties = propertyPath
                .Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var propertyLevel = 0;
            while (dataContext != null && propertyLevel < nestedProperties.Length-1)
            {
                var currentProperty = nestedProperties[propertyLevel];
                var dataContextType = dataContext.GetType();
                var pi = dataContextType.GetProperty(currentProperty);
                if (pi == null)
                {
                    //logger.Error("Cannot find any property named: {0}.", currentProperty);
                    dataContext = null;
                    break;
                }
                dataContext = pi.GetValue(dataContext, null);
                propertyLevel++;
                propertyPath = nestedProperties[propertyLevel];
            }

            if ( dataContext == null )
            {
                data = null;
                return false;
            }

            var methodName = GetExpectedMethodName( propertyPath );
            var factName = String.Concat( "Can", methodName );
            var properties = dataContext.GetType().GetProperties();

            var commandData = dataContext.GetType()
                .GetMethods()
                .Where( mi => mi.Name.Equals( methodName ) )
                .Select( mi =>
                {
                    var prms = mi.GetParameters();

                    return new CommandData()
                    {
                        DataContext = dataContext,
                        FastDelegate = mi.CreateVoidDelegate(),

                        //Fact = properties.Where( pi =>
                        //{
                        //    return pi.PropertyType == typeof( Fact ) && pi.Name.Equals( factName );
                        //} )
                        //.Select( pi =>
                        //{
                        //    var fact = ( Fact )pi.GetValue( dataContext, null );
                        //    return fact;
                        //} )
                        //.SingleOrDefault(),

                        BooleanFact = properties.Where( pi =>
                        {
                            return dataContext is INotifyPropertyChanged
                                && pi.PropertyType == typeof( Boolean )
                                && pi.Name.Equals( factName );
                        } )
                        .Select( pi => new BooleanFact
                        {
                            FastGetter = dataContext.CreateFastPropertyGetter<Boolean>( pi ),
                            Name = pi.Name
                        } )
                        .SingleOrDefault(),

                        HasParameter = prms.Length == 1,
                        ParameterType = prms.Length != 1 ? null : prms[ 0 ].ParameterType,
                        //KeyBindings = mi.GetAttributes<KeyBindingAttribute>(),
                        Description = mi.GetCustomAttribute<CommandDescriptionAttribute>()
                    };
                } )
                .SingleOrDefault();

            if ( commandData == null )
            {
                //logger.Warning( "Cannot find any method named: {0}.", methodName );
            }

            data = commandData;

            return commandData != null;
        }

        public virtual IDelegateCommand CreateCommand( CommandData commandData )
        {
            var text = ( commandData.Description == null ) ?
                        String.Empty :
                        commandData.Description.DisplayText;

            var command = ( DelegateCommand )DelegateCommand.Create( text );
            command.SetData( commandData );

            command.OnCanExecute( o =>
            {
                var data = command.GetData<CommandData>();

                if ( data.BooleanFact != null )
                {
                    var can = data.BooleanFact.FastGetter();
                    return can;
                }

                return true;
            } )
            .OnExecute( o =>
            {
                var data = command.GetData<CommandData>();
                if ( data.HasParameter )
                {
                    var prm = o;
                    if ( o is IConvertible )
                    {
                        prm = Convert.ChangeType( o, data.ParameterType );
                    }

                    data.FastDelegate( data.DataContext, new[] { prm } );
                }
                else
                {
                    data.FastDelegate( data.DataContext, null );
                }
            } );

            IMonitor monitor = null;

            if ( commandData.BooleanFact != null )
            {
                monitor = PropertyObserver.For( ( INotifyPropertyChanged )commandData.DataContext )
                    .Observe( commandData.BooleanFact.Name );
            }

            if ( command != null && monitor != null )
            {
                command.AddMonitor( monitor );
                commandData.Monitor = monitor;
            }

            return command;
        }
    }
}
