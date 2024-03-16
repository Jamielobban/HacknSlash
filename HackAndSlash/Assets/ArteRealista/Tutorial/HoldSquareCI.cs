using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldSquareCI : ComboInput
{
    public HoldSquareCI(Image sceneImage, Sprite inputSprite) : base(sceneImage, inputSprite) { }
    public override void EndListening()
    {
        _player.controller.OnSquareHold -= InputDone;
    }

    public override void StartListening()
    {
        _player.controller.OnSquareHold += InputDone;
    }
}
