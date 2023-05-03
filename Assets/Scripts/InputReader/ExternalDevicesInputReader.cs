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
        public bool Kick { get; private set; }
        public bool Bite { get; private set; }

        public void ResetOneTimeActions()
        {
            Jump = false;
            Kick = false;
            Bite = false;
        }

        private void OnUpdate()
        {
            if (Input.GetButtonDown("Jump")) Jump = true;
            if (Input.GetButtonDown("Fire1")) Kick = true;
            if (Input.GetButtonDown("Fire2")) Bite = true;
        }
    }
}