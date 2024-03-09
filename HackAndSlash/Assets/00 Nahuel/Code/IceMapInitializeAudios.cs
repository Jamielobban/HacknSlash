using UnityEngine;

public class IceMapInitializeAudios : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayMusic(Enums.Music.TutorialMusic);
        AudioManager.Instance.PlayMusicEffect(Enums.MusicEffects.Atmosfera);
        AudioManager.Instance.PlayMusicEffect(Enums.MusicEffects.Wind);
    }

}
