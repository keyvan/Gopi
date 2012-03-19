using System.Configuration.Provider;

namespace Gopi.Provider
{
    /// <summary>
    /// A collection of defined providers in the configuration
    /// </summary>
    public class DataProviderCollection : ProviderCollection
    {
        /// <summary>
        /// A DataProvider in the providers lists.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        /// <returns>The DataProvider instance.</returns>
        new public DataProvider this[string name]
        {
            get { return (DataProvider)base[name]; }
        }
    }
}
