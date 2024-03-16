using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleJumpCI : ComboInput
{
    public DoubleJumpCI(Image sceneImage, Sprite inputSprite) : base(sceneImage, inputSprite) { }
    public override void EndListening()
    {
        _player.OnDoubleJumpPress -= InputDone;
        _player.OnLand -= InputFailed;
    }

    public override void StartListening()
    {
        _player.OnDoubleJumpPress += InputDone;
        _player.OnLand += InputFailed;
    }
}