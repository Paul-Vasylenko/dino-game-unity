using System;
using Core.Services.Updater;
using UnityEngine;

namespace InputReader
{
    public class ExternalDevicesInputReader : IEntityInputSource, IWindowsInputSource, IDisposable
    {
        public float HorizontalDirection => Input.GetAxisRaw("Horizontal");
        public bool Jump { get; private set; }
        public bool Kick { get; private set; }
        public bool Bite { get; private set; }

        public event Action InventoryRequested;
        public event Action StatsRequested;

        public ExternalDevicesInputReader()
        {
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        }

        public void Dispose()
        {
            ProjectUpdater.Instance.UpdateCalled -= OnUpdate;
        }

        public void ResetOneTimeActions()
        {
            Jump = false;
            Kick = false;
            Bite = false;
        }

        private void OnUpdate()
        {
            if (Input.GetButtonDown("Jump")) Jump = true;
            if (Input.GetKeyDown(KeyCode.X)) Kick = true;
            if (Input.GetKeyDown(KeyCode.C)) Bite = true; 
            
            if (Input.GetKeyDown(KeyCode.I)) InventoryRequested?.Invoke();
            if (Input.GetKeyDown(KeyCode.O)) StatsRequested?.Invoke();
        }
    }
}