using System;
using System.Configuration;
using System.Web.Configuration;

namespace Gopi.Provider
{
    /// <summary>
    /// The management class for DataProvider.
    /// </summary>
    public class DataProviderManager
    {
        private static DataProvider defaultProvider;
        private static DataProviderCollection providers;

        /// <summary>
        /// Public static constructor.
        /// </summary>
        static DataProviderManager()
        {
            Initialize();
        }

        /// <summary>
        /// Initialization.
        /// </summary>
        private static void Initialize()
        {
            try
            {
                DataProviderConfiguration configuration =
                    (DataProviderConfiguration)ConfigurationManager.GetSection("GopiProvider");

                if (configuration == null)
                    throw new ConfigurationErrorsException("Configuration exception for Gopi.");

                providers = new DataProviderCollection();

                ProvidersHelper.InstantiateProviders(configuration.Providers, providers, typeof(DataProvider));

                providers.SetReadOnly();

                defaultProvider = providers[configuration.Default];

                if (defaultProvider == null)
                    throw new Exception("defaultProvider");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns the DataProvider instance.
        /// </summary>
        public static DataProvider Provider
        {
            get
            {
                return defaultProvider;
            }
        }

        /// <summary>
        /// Returns the DataProviderCollection instance.
        /// </summary>
        public static DataProviderCollection Providers
        {
            get
            {
                return providers;
            }
        }
    }
}
