using System.Windows.Automation;
using White.Core.AutomationElementSearch;
using White.Core.UIItems;
using White.Core.UIItems.Finders;
using White.Core.UIItems.WindowItems;
using White.Core.UIItems.WPFUIItems;

namespace Marcusoft.BDD.WhiteStepHelper
{
    /// <summary>
    /// Methods to resolve wellknown name 
    /// using AutomationProperties.Name
    /// </summary>
    public static class AutomationPropertiesNameWellKnowNameResolverExtensions
    {
        /// <summary>
        /// Returns the control with the given well known name in AutomationProperties.Name
        /// </summary>
        /// <typeparam name="T">the type of the control</typeparam>
        /// <param name="instance">the window to search</param>
        /// <param name="wellKnownName">the well known name to search for</param>
        /// <returns></returns>
        public static T GetControlByWellKnownName<T>(this Window instance, string wellKnownName) where T : UIItem
        {
            var c = instance.Get<T>(CreateAutomationNameSearchCriteria(wellKnownName));

            if (c == null)
            {
                var mess = string.Format("Could not find a control of type '{2}' with AutomationProperties.Name='{0}' in Window {1}", wellKnownName, instance.Title, typeof(T).Name);
                throw new AutomationElementSearchException(mess);
            }

            return c;
        }

        /// <summary>
        /// Returns the control with the given well known name in AutomationProperties.Name
        /// </summary>
        /// <param name="instance">the window to search</param>
        /// <param name="wellKnownName">the well known name to search for</param>
        /// <returns></returns>
        public static IUIItem GetControlByWellKnownName(this Window instance, string wellKnownName) 
        {
            var c = instance.Get(CreateAutomationNameSearchCriteria(wellKnownName));

            if (c == null)
            {
                var mess = string.Format("Could not find a control with AutomationProperties.Name='{0}' in Window {1}", wellKnownName, instance.Title);
                throw new AutomationElementSearchException(mess);
            }

            return c;
        }

        /// <summary>
        /// Returns the control with the given well known name in AutomationProperties.Name
        /// </summary>
        /// <typeparam name="T">the type of the control</typeparam>
        /// <param name="instance">the ui-item to search</param>
        /// <param name="wellKnownName">the well known name to search for</param>
        /// <returns></returns>
        public static T GetControlByWellKnownName<T>(this UIItem instance, string wellKnownName) where T : UIItem
        {
            var c = instance.Get<T>(CreateAutomationNameSearchCriteria(wellKnownName));

            if (c == null)
            {
                var mess = string.Format("Could not find a control of type '{2}' with AutomationProperties.Name='{0}' in UIItem {1}", wellKnownName, instance.Name, typeof(T).Name);
                throw new AutomationElementSearchException(mess);
            }

            return c;
        }

        /// <summary>
        /// Creates a critera to search for AutomationProperties.Name
        /// </summary>
        /// <param name="wellKnownName">the wellknown name to create a critera for</param>
        /// <returns></returns>
        private static SearchCriteria CreateAutomationNameSearchCriteria(string wellKnownName)
        {
            return SearchCriteria.ByNativeProperty(AutomationElement.NameProperty, wellKnownName);
        }
    }
}
