using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollCI : ComboInput
{
    public RollCI(Image sceneImage) : base(sceneImage) { }
    public override void EndListening()
    {
        _player.controller.OnDashPerformed -= InputDone;
    }

    public override void StartListening()
    {
        _player.controller.OnDashPerformed += InputDone;
    }
}