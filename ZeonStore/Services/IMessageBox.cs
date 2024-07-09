using System;
using System.Threading.Tasks;

namespace ZeonStore.Services
{
    public interface IMessageBox
    {
        Task ShowInstallationError(Exception exception);
        Task ShowUnknownInstallationError();
    }
}
