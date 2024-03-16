using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldTriangleCI : ComboInput
{
    public HoldTriangleCI(Image sceneImage, Sprite inputSprite) : base(sceneImage, inputSprite) { }
    public override void EndListening()
    {
        _player.controller.OnTriangleHold -= InputDone;
    }

    public override void StartListening()
    {
        _player.controller.OnTriangleHold += InputDone;
    }
}
