using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JumpCI : ComboInput
{
    public JumpCI(Image sceneImage, Sprite inputSprite) : base(sceneImage, inputSprite) { }

    public override void EndListening()
    {
        _player.controller.OnJumpPerformed -= InputDone;
    }

    public override void StartListening()
    {
        _player.controller.OnJumpPerformed += InputDone;
    }
}
