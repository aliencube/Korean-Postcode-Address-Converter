using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Enums;
using Aliencube.Utilities.KoreanPostcodeAddressConverter.Services.Events;
using Aliencube.Utilities.KoreanPostcodeAddressUpdater.Services;
using Settings = Aliencube.Utilities.KoreanPostcodeAddressUpdater.Services.Settings;

namespace Aliencube.Utilities.KoreanPostcodeAddressConverter.Console
{
    /// <summary>
    /// This represents the main entry point of the program entity.
    /// </summary>
    public class Program
    {
        private static readonly ILog _log = LogManager.GetLogger("KPAC Console");

        #region Methods
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        public static void Main(string[] args)
        {
            ShowSplash();
            try
            {
                ProcessRequests(args);
                System.Console.WriteLine();
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("ERROR:");
                    sb.AppendLine();
                    sb.AppendLine(ex.Message);
                    sb.AppendLine();
                    sb.AppendLine(ex.StackTrace);

                    _log.Error(sb.ToString());
                }

                if (ex.GetType() != typeof(ArgumentException))
                    return;

                System.Console.WriteLine();
                ShowUsage();
            }
        }

        /// <summary>
        /// Shows the splash message.
        /// </summary>
        private static void ShowSplash()
        {
            var fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            var sb = new StringBuilder();
            sb.AppendLine(String.Format("{0} v{1}", fvi.ProductName, fvi.FileVersion));
            sb.AppendLine("------------------------------");

            System.Console.WriteLine(sb.ToString());
        }

        /// <summary>
        /// Shows the usage of the app message.
        /// </summary>
        private static void ShowUsage()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Usage:");
            sb.AppendLine("  KoreanPostcodeAddressUpdaterConsole.exe [Lot|Street]");
            sb.AppendLine();
            sb.AppendLine("Parameter:");
            sb.AppendLine("  Lot:    Run LOT-based address converter service.");
            sb.AppendLine("  Street: Run Street-based address converter service.");
            sb.AppendLine();

            System.Console.WriteLine(sb.ToString());
        }

        /// <summary>
        /// Gets the converter service type.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        /// <returns>Returns the converter service type.</returns>
        private static ConverterServiceType GetServiceType(IList<string> args)
        {
            var serviceType = ConverterServiceType.Undefined;

            ConverterServiceType result;
            if (Enum.TryParse(args[0], true, out result))
                serviceType = result;

            return serviceType;
        }

        /// <summary>
        /// Processes the requests based on the argument provided.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        private static void ProcessRequests(IList<string> args)
        {
            if (args == null || !args.Any())
                throw new ArgumentException("No argument found");

            var serviceType = GetServiceType(args);
            if (serviceType == ConverterServiceType.Undefined)
                throw new ArgumentException("Invalid arguments");

            var settings = Settings.Instance;

            if (_log.IsInfoEnabled)
                _log.Info(String.Format("Starting - {0}", Convert.ToString(serviceType)));

            var factory = new UpdaterServiceFactory(settings);
            factory.StatusChanged += Factory_StatusChanged;
            factory.ExceptionThrown += Factory_ExceptionThrown;
            factory.ProcessRequests(serviceType);

            if (_log.IsInfoEnabled)
                _log.Info(String.Format("Stopped - {0}", Convert.ToString(serviceType)));
        }

        /// <summary>
        /// Occurs when status changed event is raised.
        /// </summary>
        /// <param name="sender">Object that triggers the event.</param>
        /// <param name="e">Provides data for the status changed event.</param>
        static void Factory_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (e.StatusMessage == " .")
                System.Console.Write(e.StatusMessage);
            else if (_log.IsInfoEnabled)
                _log.Info(e.StatusMessage);
        }

        /// <summary>
        /// Occurs when exception thrown event is raised.
        /// </summary>
        /// <param name="sender">Object that triggers the event.</param>
        /// <param name="e">Provides data for the exception thrown event.</param>
        static void Factory_ExceptionThrown(object sender, ExceptionThrownEventArgs e)
        {
            if (_log.IsWarnEnabled)
                _log.Warn(e.Exception.Message);
        }
        #endregion
    }
}
