namespace Marcusoft.BDD.WhiteStepHelper
{
    public interface IIdResolver
    {
        /// <summary>
        /// Returns the ID for an controls well-known name in a certain window
        /// </summary>
        /// <param name="wellKnownName">This is the name that is used in the Gehrkin-specs, typically the name a user is user when referring to the control</param>
        /// <param name="windowToSearch">the window to search in</param>
        /// <returns>the id of the control</returns>
        /// <remarks></remarks>
        string IdFromWellKnownName(string wellKnownName, White.Core.UIItems.WindowItems.Window windowToSearch);
    }
}
