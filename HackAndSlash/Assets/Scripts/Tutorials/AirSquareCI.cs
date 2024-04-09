using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirSquareCI : ComboInput
{
    public AirSquareCI(Image sceneImage, Sprite inputSprite) : base(sceneImage, inputSprite) { }
    public override void EndListening()
    {
        _player.OnAirSquarePress -= InputDone;
        _player.OnLand -= InputFailed;
    }

    public override void StartListening(bool firstOfChain)
    {
        _player.OnAirSquarePress += InputDone;
        _player.OnLand += InputFailed;
    }
}
