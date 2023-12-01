using Fusion;
using UnityEngine;

namespace No6RoN.Sandbox.Fusion103
{
    public struct NetworkInputData : INetworkInput
    {
        public const byte MOUSEBUTTON1 = 0x01;

        public byte buttons;
        public Vector3 direction;
    }
}
