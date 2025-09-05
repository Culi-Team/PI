using EQX.Core.Process;
using EQX.Core.Sequence;
using PIFilmAutoDetachCleanMC.Defines;
using System.Windows;
using System.Windows.Controls;

namespace PIFilmAutoDetachCleanMC.RecipeNameSelectors
{
    public class ProcessNameTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate TransferFixtureProcTemplate { get; set; }
        public DataTemplate DetachProcTemplate { get; set; }
        public DataTemplate WetCleanLeftProcTemplate { get; set; }
        public DataTemplate WetCleanRightProcTemplate { get; set; }
        public DataTemplate AFCleanLeftProcTemplate { get; set; }
        public DataTemplate AFCleanRightProcTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IProcess<ESequence> process)
            {
                switch (process.Name)
                {
                    case "TransferFixtureProc":
                        return TransferFixtureProcTemplate;
                    case "DetachProc":
                        return DetachProcTemplate;
                    case "WetCleanLeftProc":
                        return WetCleanLeftProcTemplate;
                    case "WetCleanRightProc":
                        return WetCleanRightProcTemplate;
                    case "FCleanLeftProc":
                        return AFCleanLeftProcTemplate;
                    case "AFCleanRightProc":
                        return AFCleanRightProcTemplate;
                }
            }
            return DefaultTemplate;
        }
    }
}
