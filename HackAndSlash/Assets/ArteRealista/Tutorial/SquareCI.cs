using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareCI : ComboInput
{
    public SquareCI(Image sceneImage) : base(sceneImage) { }
    public override void EndListening()
    {
        _player.controller.OnSquarePress -= InputDone;
    }

    public override void StartListening()
    {
        _player.controller.OnSquarePress += InputDone;
    }
}
