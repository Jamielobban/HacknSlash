using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunCI : ComboInput
{
    public RunCI(Image sceneImage) : base(sceneImage) { }
    public override void EndListening()
    {
        _player.OnRunPress -= InputDone;
    }

    public override void StartListening()
    {
        _player.OnRunPress += InputDone;
    }
}