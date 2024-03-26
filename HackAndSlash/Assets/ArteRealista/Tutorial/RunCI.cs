using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunCI : ComboInput
{
    public RunCI(Image sceneImage, Sprite inputSprite) : base(sceneImage, inputSprite) { }
    public override void EndListening()
    {
        _player.OnRunPress -= InputDone;
    }

    public override void StartListening(bool firstOfChain)
    {
        _player.OnRunPress += InputDone;
    }
}