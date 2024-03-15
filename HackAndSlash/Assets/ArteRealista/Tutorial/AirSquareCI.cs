using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirSquareCI : ComboInput
{
    public AirSquareCI(Image sceneImage) : base(sceneImage) { }
    public override void EndListening()
    {
        _player.OnAirSquarePress -= InputDone;
    }

    public override void StartListening()
    {
        _player.OnAirSquarePress += InputDone;
    }
}
