using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMapInitializeAudios : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayMusic(Enums.Music.MainTheme);
        AudioManager.Instance.PlayMusicEffect(Enums.MusicEffects.Atmosfera);
        AudioManager.Instance.PlayMusicEffect(Enums.MusicEffects.IceWind);
    }

}
