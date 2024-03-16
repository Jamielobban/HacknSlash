using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriangleCI : ComboInput
{
    public TriangleCI(Image sceneImage, Sprite inputSprite) : base(sceneImage, inputSprite) { }
    public override void EndListening()
    {
        _player.controller.OnTrianglePress -= InputDone;
        _player.controller.OnSquarePress -= InputFailed;
        _player.controller.OnSquareHold -= InputFailed;
        _player.controller.OnTriangleHold -= InputFailed;
        _player.OnAirSquarePress -= InputFailed;
        _player.OnAirTrianglePress -= InputFailed;
    }

    public override void StartListening()
    {
        _player.controller.OnTrianglePress += InputDone;
        _player.controller.OnSquarePress += InputFailed;
        _player.controller.OnSquareHold += InputFailed;
        _player.controller.OnTriangleHold += InputFailed;
        _player.OnAirSquarePress += InputFailed;
        _player.OnAirTrianglePress += InputFailed;
    }
}
