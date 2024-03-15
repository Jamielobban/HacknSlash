using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriangleCI : ComboInput
{
    public TriangleCI(Image sceneImage) : base(sceneImage) { }
    public override void EndListening()
    {
        _player.controller.OnTrianglePress -= InputDone;
    }

    public override void StartListening()
    {
        _player.controller.OnTrianglePress += InputDone;
    }
}
