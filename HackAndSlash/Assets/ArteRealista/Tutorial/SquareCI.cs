using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareCI : ComboInput
{
    public SquareCI(Image sceneImage, Sprite inputSprite) : base(sceneImage, inputSprite) { }
    public override void EndListening()
    {
        _player.controller.OnSquarePress -= InputDone;
        _player.controller.OnTrianglePress -= InputFailed;
        _player.controller.OnSquareHold -= InputFailed;
        _player.controller.OnTriangleHold -= InputFailed;
        _player.OnAirSquarePress -= InputFailed;
        _player.OnAirTrianglePress -= InputFailed;
    }

    public override void StartListening()
    {
        _player.controller.OnSquarePress += InputDone;
        _player.controller.OnTrianglePress += InputFailed;
        _player.controller.OnSquareHold += InputFailed;
        _player.controller.OnTriangleHold += InputFailed;
        _player.OnAirSquarePress += InputFailed;
        _player.OnAirTrianglePress += InputFailed;
    }
}
