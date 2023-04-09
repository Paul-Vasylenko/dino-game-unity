using System;
using Core.Services.Updater;
using UnityEngine;

namespace InputReader
{
    public class ExternalDevicesInputReader : IEntityInputSource, IDisposable
    {
        public ExternalDevicesInputReader()
        {
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        }

        public void Dispose()
        {
            ProjectUpdater.Instance.UpdateCalled -= OnUpdate;
        }

        public float HorizontalDirection => Input.GetAxisRaw("Horizontal");
        public bool Jump { get; private set; }

        public void ResetOneTimeActions()
        {
            Jump = false;
        }

        private void OnUpdate()
        {
            if (Input.GetButtonDown("Jump")) Jump = true;
        }
    }
}