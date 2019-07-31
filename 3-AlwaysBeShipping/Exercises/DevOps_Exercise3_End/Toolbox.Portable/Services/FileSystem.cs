
using Toolbox.Portable.Infrastructure;

namespace Toolbox.Portable.Services
{
	public static class FileSystem
	{
		private static IFileSystemService registeredService;

		public static IFileSystemService CurrentService
		{
			get
			{
				if (registeredService == null)
				{
					registeredService = ServiceContainer.Resolve<IFileSystemService>();
				}
				return registeredService;
			}
		}

		public static void RegisterService(IFileSystemService service)
		{
			ServiceContainer.Register<IFileSystemService>(service);
			registeredService = service;
		}

		public static IFileStorageService DocumentStorage
		{
			get { return CurrentService.DocumentStorage; }
		}

		public static IFileStorageService SettingsStorage
		{
			get { return CurrentService.SettingsStorage; }
		}

		public static IFileStorageService TempStorage
		{
			get { return CurrentService.TempStorage; }
		}
	}
}
