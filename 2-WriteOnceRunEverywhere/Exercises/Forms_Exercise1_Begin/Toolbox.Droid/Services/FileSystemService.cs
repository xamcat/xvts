using System.IO;
using Toolbox.Portable.Services;

namespace Toolbox.Droid.Services
{
    public class FileSystemService : IFileSystemService
    {
        private readonly IFileStorageService documentStorage;
        private readonly IFileStorageService settingsStorage;
        private readonly IFileStorageService tempStorage;

        public FileSystemService()
        {
            var applicationSupportPath = Platform.ApplicationSupportPath;
            var documentDirectoryPath = Path.Combine(applicationSupportPath, "Documents");

            // Note: When running in debug mode (most likely in the Simulator) we print out the
            // path to the documents directory, since it can change based on the simulator chosen.
            // Apple uses cryptic GUID's in the paths, thus this helps to find the directories 
            // quicker.
#if DEBUG
            //Logger.Debug($"Documents Directory Path: {documentDirectoryPath}");
#endif

            documentStorage = new FileStorageService(documentDirectoryPath);
            settingsStorage = new FileStorageService(Path.Combine(applicationSupportPath, "Settings"));
            tempStorage = new FileStorageService(Path.Combine(applicationSupportPath, "Temp"));
        }

        public IFileStorageService DocumentStorage => documentStorage;

        public IFileStorageService SettingsStorage => settingsStorage;

        public IFileStorageService TempStorage => tempStorage;

        public string GetTempFileName()
        {
            return Path.GetTempFileName();
        }
    }
}
