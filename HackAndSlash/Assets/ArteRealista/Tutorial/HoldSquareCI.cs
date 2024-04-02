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
        _player.OnComboTimeExpired -= InputFailed;
        _player.CancelComboTimeCountdown();
    }

    public override void StartListening(bool firstOfChain)
    {
        _player.controller.OnSquareHold += InputDone;
        _player.controller.OnTrianglePress += InputFailed;
        _player.controller.OnSquarePress += InputFailed;
        _player.controller.OnTriangleHold += InputFailed;
        _player.OnAirSquarePress += InputFailed;
        _player.OnAirTrianglePress += InputFailed;
        if (!firstOfChain)
        {
            _player.OnComboTimeExpired += InputFailed;
            _player.StartComboTimeCountdown(0.8f);
        }
    }
}
