namespace Toolbox.Portable.Services
{
    public interface IFileSystemService
    {
        IFileStorageService DocumentStorage { get; }

		IFileStorageService SettingsStorage { get; }

		IFileStorageService TempStorage { get; }

        string GetTempFileName();
    }
}