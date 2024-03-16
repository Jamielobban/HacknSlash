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
        _player.controller.OnTrianglePress -= InputFailed;
        _player.controller.OnSquarePress -= InputFailed;
        _player.controller.OnTriangleHold -= InputFailed;
        _player.OnAirSquarePress -= InputFailed;
        _player.OnAirTrianglePress -= InputFailed;
    }

    public override void StartListening()
    {
        _player.controller.OnSquareHold += InputDone;
        _player.controller.OnTrianglePress += InputFailed;
        _player.controller.OnSquarePress += InputFailed;
        _player.controller.OnTriangleHold += InputFailed;
        _player.OnAirSquarePress += InputFailed;
        _player.OnAirTrianglePress += InputFailed;
    }
}
