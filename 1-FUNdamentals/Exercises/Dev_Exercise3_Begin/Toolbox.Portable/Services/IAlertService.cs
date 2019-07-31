using System;
using System.Threading.Tasks;

namespace Toolbox.Portable.Services
{
    public interface IAlertService
    {
		Task DisplayAsync(string title, string message = null, string cancelButton = null);
        Task<string> DisplayInputEntryAsync(string title, string message = null, string actionButton = null, string cancelButton = null, string hint = null, Func<string, bool> validator = null);
    }
}