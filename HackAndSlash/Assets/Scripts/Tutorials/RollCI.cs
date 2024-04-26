using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollCI : ComboInput
{
    public RollCI(Image sceneImage, Sprite inputSprite) : base(sceneImage, inputSprite) { }
    public override void EndListening()
    {
        _player.OnDashPerformed -= InputDone;
    }

    public override void StartListening(bool firstOfChain)
    {
        _player.OnDashPerformed += InputDone;
    }
}