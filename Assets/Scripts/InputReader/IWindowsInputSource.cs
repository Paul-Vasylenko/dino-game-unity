using System;

namespace InputReader
{
    public interface IWindowsInputSource
    {
        public event Action InventoryRequested;
        public event Action StatsRequested;
    }
}