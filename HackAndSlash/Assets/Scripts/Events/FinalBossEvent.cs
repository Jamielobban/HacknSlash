using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossEvent : EventBaseRounds
{
    private WinInteractable win;
    protected override void Awake()
    {
        base.Awake();
        win = FindObjectOfType<WinInteractable>();
    }
    protected override void CompleteEvent()
    {
        _currentEventState = Enums.EventState.FINISHED;

        LevelManager.Instance.EndEvent();
        AudioManager.Instance.PlayMusic(Enums.Music.MainTheme);
        AudioManager.Instance.PlayFx(Enums.Effects.SuccessEvent);

        win.OnWin();
    }

}
