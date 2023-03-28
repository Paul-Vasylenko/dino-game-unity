using UnityEngine;

namespace InputReader
{
    public class ExternalDevicesInputReader : IEntityInputSource
    {
        public float HorizontalDirection => Input.GetAxisRaw("Horizontal");
        public bool Jump { get; private set;  }
        public void OnUpdate()
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump = true;
            }
        }

        public void ResetOneTimeActions()
        {
            Jump = false;
        }

    }
}
