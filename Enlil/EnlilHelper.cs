using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Enlil.Domain;

namespace Enlil
{
    public static class EnlilHelper
    {
        public static BuildContext BuildAssemblyForPath => BuildContext.Default;
        public static GetTypeFilterForAttributeName TypesForAttribute = name => (context) =>
            AssemblyHelper.GetTypesByAttributeName(context.ResultingAssembly, name);

        public static GetMethodFilterForAttributeName MethodTypesForAttribute = name => (context) =>
            AssemblyHelper.GetTypesByAttributeNameOnMethod(context.ResultingAssembly, name);
    }

    public delegate TypeFilter GetTypeFilterForAttributeName(string name);

    public delegate MethodFilter GetMethodFilterForAttributeName(string name);
    public delegate IEnumerable<Type> TypeFilter(BuildContext context);

    public delegate IEnumerable<MethodOnType> MethodFilter(BuildContext context);
}