using System;
using System.Collections.Generic;
using System.Windows.Automation;
using White.Core.UIItems.Finders;

namespace Marcusoft.BDD.WhiteStepHelper
{
    /// <summary>
    /// Assumes that the wellknown name of a control is stored in the AutomationProperties.Name property
    /// </summary>
    /// <example>
    /// AutomationProperties.Name="TestTräd"
    /// </example>
    /// <remarks></remarks>
    public class AutomationNameResolver : IIdResolver
    {
        private Dictionary<string, string> _lookedUpIds;
        
        /// <summary>
        /// Constructs a new name resolver that looks for controls by AutomationProperties.Name
        /// </summary>
        /// <remarks></remarks>
        public AutomationNameResolver()
        {
            _lookedUpIds = new Dictionary<string, string>();
        }

        /// <summary>
        /// Returns the id from the already looked up ids
        /// or
        /// empty string if not present
        /// </summary>
        /// <param name="wellKnownName">the wellknown name to look for</param>
        /// <param name="windowTitle">the window title</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string IDFromSavedResult(string wellKnownName, string windowTitle)
        {
            var id = string.Empty;
            var key = KeyForWellKnownName(windowTitle, wellKnownName);

            if ((_lookedUpIds.ContainsKey(key)))
            {
                id = _lookedUpIds[key];
            }

            return id;
        }

        private static string KeyForWellKnownName(string windowTitle, string wellKnownName)
        {
            return windowTitle + "_" + wellKnownName;
        }

        /// <summary>
        /// Returns the ID for an controls well-known name in a certain window
        /// </summary>
        /// <param name="wellKnownName">This is the name that is used in the Gehrkin-specs, typically the name a user is user when referring to the control</param>
        /// <param name="windowToSearch">the window to search in</param>
        /// <returns>the id of the control</returns>
        /// <remarks></remarks>
        public string IdFromWellKnownName(string wellKnownName, White.Core.UIItems.WindowItems.Window windowToSearch)
        {
            var id = IDFromSavedResult(wellKnownName, windowToSearch.Title);

            if (string.IsNullOrEmpty(id))
            {
                id = SearchForIDOnWindow(wellKnownName, windowToSearch);

                var key = KeyForWellKnownName(windowToSearch.Title, wellKnownName);
                _lookedUpIds.Add(key, id);
            }


            return id;
        }

        private string SearchForIDOnWindow(string wellKnownName, White.Core.UIItems.WindowItems.Window windowToSearch)
        {
            var id = string.Empty;

            var crit = SearchCriteria.ByNativeProperty(AutomationElement.NameProperty, wellKnownName);

            try
            {
                id = windowToSearch.Get(crit).PrimaryIdentification;
            }
            catch (Exception ex)
            {
                var message = string.Format("Could not find any control with AutomationProperties.Name=\"{0}\" in Window '{1}'", wellKnownName, windowToSearch.Title);
                throw new ArgumentException(message);
            }
            return id;
        }
    }
}
