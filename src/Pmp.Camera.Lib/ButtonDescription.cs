using Reinforced.Typings.Attributes;
using System.Linq;

namespace Pmp.Camera.Lib
{
    public class ButtonType
    {
        public static string UcButton = nameof(UcButton);
    }

    [TsClass]
    public class ButtonDescription
    {
        public string Type = ButtonType.UcButton;
        public string[] PossibleValues { get; set; }
        public bool IsEnum => PossibleValues != null && PossibleValues.Any();

        public string SetCommand { get; set; }
        public string GetCommand { get; set; }
        public string Name { get; internal set; }
    }

    [TsClass]
    public class ButtonWithParametersDescription
    {
        public string Name { get; set; }
        public string ParameterDescription { get; set; }
    }

    [TsClass]
    public class ParameterDescription
    {
        public string Name { get; set; }
    }
}
