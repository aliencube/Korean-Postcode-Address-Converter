namespace Aliencube.Utilities.KoreanPostcodeAddressUpdater.Services
{
    /// <summary>
    /// This represents the configuration settings entity.
    /// </summary>
    public class Settings: Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Settings
    {
        #region Constructors
        /// <summary>
        ///	Initialises a new instance of the Settings object as private.
        /// </summary>
        private Settings()
        {
        }
        #endregion

        #region Properties
        private static Settings _instance;
        /// <summary>
        /// Gets the instance of the settings object.
        /// </summary>
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Settings();
                return _instance;
            }
        }
        #endregion
    }
}
