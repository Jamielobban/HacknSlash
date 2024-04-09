using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirTriangleCI : ComboInput
{
    public AirTriangleCI(Image sceneImage, Sprite inputSprite) : base(sceneImage, inputSprite) { }
    public override void EndListening()
    {
        _player.OnAirTrianglePress -= InputDone;
        _player.OnLand -= InputFailed;
    }

    public override void StartListening(bool firstOfChain)
    {
        _player.OnAirTrianglePress += InputDone;
        _player.OnLand += InputFailed;
    }
}
