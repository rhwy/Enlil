using System;
using System.Collections;
using System.Collections.Generic;
using Enlil.Domain;

namespace Enlil
{
    public static class EnlilHelper
    {
        public static BuildContext BuildAssemblyForPath => BuildContext.Default;
        public static GetTypeFilterForAttributeName TypesForAttribute = f => (context) =>
            AssemblyHelper.GetTypesByAttributeName(context.ResultingAssembly, f);
    }

    public delegate TypeFilter GetTypeFilterForAttributeName(string name);
    public delegate IEnumerable<Type> TypeFilter(BuildContext context);
}