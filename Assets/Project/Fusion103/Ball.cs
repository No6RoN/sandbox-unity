using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace No6RoN.Sandbox.Fusion103
{
    public class Ball : NetworkBehaviour
    {
        [Networked] private TickTimer life { get; set; }
        
        public void Init()
        {
            life = TickTimer.CreateFromSeconds(Runner, 5.0f);
        }
        
        public override void FixedUpdateNetwork()
        {
            if(life.Expired(Runner))
                Runner.Despawn(Object);
            transform.position += 5 * transform.forward * Runner.DeltaTime;
        }
    }
}
