namespace Radical.Reflection
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Radical.Validation;

    /// <summary>
    /// Defines static methods to manipulates MemberInfo types.
    /// All methods are also defined as .NET extension methods.
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Determines whether an attribute is defined on the specified type.
        /// </summary>
        /// <typeparam name="T">The type (System.Type) of the attribute to search for.</typeparam>
        /// <param name="memberInfo">The MemberInfo to invastigate.</param>
        /// <returns>
        ///     <c>true</c> if the attribute is defined; otherwise, <c>false</c>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static Boolean IsAttributeDefined<T>(this MemberInfo memberInfo) where T : Attribute
        {
            Ensure.That(memberInfo).Named("memberInfo").IsNotNull();

            return memberInfo.IsDefined(typeof(T));
        }

        /// <summary>
        /// Tries to extracts the first attribute applied to the specified <see cref="MemberInfo"/>.
        /// </summary>
        /// <typeparam name="T">The attribute to search for</typeparam>
        /// <param name="memberInfo">The MemberInfo to search on.</param>
        /// <param name="attribute">An instance of the found attribute, if one, otherwise null.</param>
        /// <returns>
        /// <c>True</c> if an attribute of the given type can be found; otherwise <c>false</c>.
        /// </returns>
        public static Boolean TryGetAttribute<T>(this MemberInfo memberInfo, out T attribute) where T : Attribute
        {
            Ensure.That(memberInfo).Named("memberInfo").IsNotNull();

            if(memberInfo.IsAttributeDefined<T>())
            {
                attribute = memberInfo.GetCustomAttribute<T>();
                return true;
            }

            attribute = null;
            return false;
        }
    }
}
