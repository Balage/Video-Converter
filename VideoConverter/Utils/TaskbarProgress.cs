using System.Runtime.InteropServices;

namespace VideoConverter.Utils
{
    public class TaskbarProgress
    {
        public enum TaskbarStates
        {
            NoProgress = 0,
            Indeterminate = 0x1,
            Normal = 0x2,
            Error = 0x4,
            Paused = 0x8
        }

        [ComImport()]
        [Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface ITaskbarList3
        {
            // ITaskbarList
            [PreserveSig]
            void HrInit();
            [PreserveSig]
            void AddTab(IntPtr hwnd);
            [PreserveSig]
            void DeleteTab(IntPtr hwnd);
            [PreserveSig]
            void ActivateTab(IntPtr hwnd);
            [PreserveSig]
            void SetActiveAlt(IntPtr hwnd);

            // ITaskbarList2
            [PreserveSig]
            void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

            // ITaskbarList3
            [PreserveSig]
            void SetProgressValue(IntPtr hwnd, UInt64 ullCompleted, UInt64 ullTotal);
            [PreserveSig]
            void SetProgressState(IntPtr hwnd, TaskbarStates state);
        }

        [ComImport()]
        [Guid("56fdf344-fd6d-11d0-958a-006097c9a090")]
        [ClassInterface(ClassInterfaceType.None)]
        private class TaskbarInstance
        {
            //
        }

        // State
        private readonly IntPtr _windowHandle;
        private readonly ITaskbarList3 _taskbarInstance = (ITaskbarList3)new TaskbarInstance();
        private readonly bool _taskbarSupported = Environment.OSVersion.Version >= new Version(6, 1);

        public TaskbarProgress(IntPtr windowHandle)
        {
            _windowHandle = windowHandle;
        }

        public void SetState(TaskbarStates taskbarState)
        {
            if (_taskbarSupported)
            {
                _taskbarInstance.SetProgressState(_windowHandle, taskbarState);
            }
        }

        public void SetValue(double progressValue, double progressMax)
        {
            if (_taskbarSupported)
            {
                _taskbarInstance.SetProgressValue(_windowHandle, (ulong)(progressValue * 1000.0), (ulong)(progressMax * 1000.0));
            }
        }
    }
}
