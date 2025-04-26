using System;

namespace NewCore.Services.UI
{
    public interface IPanel : IDisposable
    {
        void Open();
        void Close();
    }
}