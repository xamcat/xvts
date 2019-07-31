using System;
using System.IO;
using Toolbox.Portable.Services;

namespace Toolbox.iOS.Services
{
	public class FileStorageService : IFileStorageService
	{
		private readonly DirectoryInfo baseDirectory;

		public FileStorageService(string baseDirectoryPath, bool create = true)
		{
			baseDirectory = new DirectoryInfo(baseDirectoryPath);

			if (create && !baseDirectory.Exists)
				baseDirectory.Create();
		}

		public FileStorageService(DirectoryInfo baseDirectory, bool create = true)
		{
			this.baseDirectory = baseDirectory;

			if (create && !this.baseDirectory.Exists)
				this.baseDirectory.Create();
		}

		public bool FileExists(string path)
		{
			return File.Exists(GetPath(path));
		}

		public bool DeleteFile(string path)
		{
			File.Delete(GetPath(path));
			return true;
		}

		public Stream CreateOutputStream(string path, OutputStreamMode mode = OutputStreamMode.OverwriteOrCreate)
		{
			FileMode fileMode = FileMode.OpenOrCreate;

			switch (mode)
			{
				case OutputStreamMode.Append:
					fileMode = FileMode.Append;
					break;
				case OutputStreamMode.OverwriteOrCreate:
					fileMode = FileMode.Create;
					break;
				case OutputStreamMode.OpenOrCreate:
					fileMode = FileMode.OpenOrCreate;
					break;
			}
			return new FileStream(GetPath(path), fileMode, FileAccess.Write);
		}

		public Stream CreateInputStream(string path)
		{
			return new FileStream(GetPath(path), FileMode.Open, FileAccess.Read);
		}

		public long FileSize(string path)
		{
			path = GetPath(path);
			if (File.Exists(path))
			{
				var file = new FileInfo(path);
				return file.Length;
			}

			return 0;
		}

		public string BasePath => baseDirectory.FullName;

		private string GetPath(string path)
		{
			if (!path.StartsWith(BasePath, StringComparison.Ordinal))
				return Path.Combine(BasePath, path);

			return path;
		}
	}
}
