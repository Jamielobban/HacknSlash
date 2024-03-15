using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleJumpCI : ComboInput
{
    public DoubleJumpCI(Image sceneImage) : base(sceneImage) { }
    public override void EndListening()
    {
        _player.OnDoubleJumpPress -= InputDone;
    }

    public override void StartListening()
    {
        _player.OnDoubleJumpPress += InputDone;
    }
}