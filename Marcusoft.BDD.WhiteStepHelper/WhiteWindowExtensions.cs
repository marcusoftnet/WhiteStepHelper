using White.Core.AutomationElementSearch;
using White.Core.UIItems.WindowItems;

namespace Marcusoft.BDD.WhiteStepHelper
{
    /// <summary>
    /// Extension methods for White Windows
    /// </summary>
    public static class WhiteWindowExtensions
    {
        public static Window GetModalDialogByTitle(this Window instance,string dialogTitle)
        {
            var dialog = instance.ModalWindow(dialogTitle);

            if(dialog == null)
            {
                var message = string.Format("No dialog with title {0} found", dialogTitle);
                throw new AutomationElementSearchException(message);                
            }
            return dialog;
        }
    }
}
