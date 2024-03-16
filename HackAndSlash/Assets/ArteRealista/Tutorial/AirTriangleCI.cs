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
    }

    public override void StartListening()
    {
        _player.OnAirTrianglePress += InputDone;
    }
}
