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
        _player.controller.OnTrianglePress -= InputFailed;
        _player.controller.OnSquareHold -= InputFailed;
        _player.controller.OnSquarePress -= InputFailed;
        _player.OnAirSquarePress -= InputFailed;
        _player.OnAirTrianglePress -= InputFailed;

    }

    public override void StartListening()
    {
        _player.controller.OnTriangleHold += InputDone;
        _player.controller.OnTrianglePress += InputFailed;
        _player.controller.OnSquareHold += InputFailed;
        _player.controller.OnSquarePress += InputFailed;
        _player.OnAirSquarePress += InputFailed;
        _player.OnAirTrianglePress += InputFailed;
    }
}
