using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Enlil.Domain;

namespace Enlil
{
    public class AssemblyHelper
    {
        public static WorkingContext CreatWorkingContext(BuildContext buildContext)
        {
            return new WorkingContext(buildContext);
        }

        public static IEnumerable<Type> GetTypesByAttributeName(Assembly asm, string name)
        {
            if (string.IsNullOrEmpty(name)) return Enumerable.Empty<Type>();
            
            name = name.EndsWith("Attribute") ? name : name + "Attribute";

            return asm
                .GetExportedTypes()
                .Where(t => t.CustomAttributes.Any(a => a.AttributeType.Name == name));
        }
        public static IEnumerable<MethodInfo> GetTypesByAttributeNameOnMethod(Assembly asm, string name)
        {
            name = name.EndsWith("Attribute") ? name : name + "Attribute";

            return asm
                .GetExportedTypes()
                .SelectMany(
                    type => type.GetMethods())
                .Where(
                    method => method.CustomAttributes.Any(
                        attrib => attrib.AttributeType.Name == name)); 
        }
        
    }

    public struct WorkingContext
    {
        public BuildContext BuildContext { get; }

        public WorkingContext(BuildContext buildContext)
        {
            BuildContext = buildContext;
        }
    }
}