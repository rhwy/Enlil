using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Enlil.Domain;

namespace Enlil
{
    public static class EnlilHelper
    {
        public static BuildContext BuildAssemblyForPath => BuildContext.Default;
        public static GetTypeFilterForAttributeName TypesForAttribute = name => (context) =>
            new ReflectionAction<IEnumerable<Type>,Type>(
                AssemblyHelper.GetTypesByAttributeName(context.ResultingAssembly, name));

        public static GetMethodFilterForAttributeName MethodTypesForAttribute = name => (context) =>
            new ReflectionAction<IEnumerable<MethodOnType>,MethodOnType>(
                AssemblyHelper.GetTypesByAttributeNameOnMethod(context.ResultingAssembly, name));
        
        public static RunAction<T> Each<T>(Action<T> a) => (T e) => a(e);

    }
}