using Enlil.Domain;

namespace Enlil
{
    public struct WorkingContext
    {
        public BuildContext BuildContext { get; }

        public WorkingContext(BuildContext buildContext)
        {
            BuildContext = buildContext;
        }
    }
}