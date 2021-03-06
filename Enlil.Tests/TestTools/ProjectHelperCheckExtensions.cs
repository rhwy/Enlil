using System;
using Enlil.Domain;
using NFluent;
using NFluent.Extensibility;

namespace Enlil.Tests.TestTools
{
    public static class ProjectHelperCheckExtensions
    {
        public static ICheckLink<ICheck<BuildContext>> HasNoErrors(this ICheck<BuildContext> check)
        { 
            var actual = ExtensibilityHelper.ExtractChecker(check).Value;
            
            ExtensibilityHelper.BeginCheck(check)
                .FailWhen(sut => sut.HasError, $"Current context has errors whereas it should not : {actual.Error?.Message}")
                .OnNegate("hum, I don't see any errors whereas it's supposed to have...")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class ExperimentAttribute : Attribute
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}