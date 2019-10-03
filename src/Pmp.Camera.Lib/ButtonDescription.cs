using System.Linq;

namespace Pmp.Camera.Lib
{
    public class ButtonDescription
    {
        public string[] PossibleValues { get; set; }
        public bool IsEnum => PossibleValues != null && PossibleValues.Any();

        public string SetCommand { get; set; }
        public string GetCommand { get; set; }
        public string Name { get; internal set; }
    }
}
