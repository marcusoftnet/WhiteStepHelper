namespace Marcusoft.BDD.WhiteStepHelper
{
    /// <summary>
    /// This is a real simple convention based id resolver
    /// The convention is that the ID of the controll is the same as the wellknown name
    /// transformed to lowercase and no spaces
    /// </summary>
    /// <remarks></remarks>
    public class SimpleIdResolver : IIdResolver
    {

        public string IdFromWellKnownName(string wellKnownName, White.Core.UIItems.WindowItems.Window windowToSearch)
        {
            return wellKnownName.Replace(" ", string.Empty).ToLower();
        }
    }
}
