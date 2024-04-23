using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsSFX : MonoBehaviour
{
    public AudioSource footstepSound;  // Asigna el AudioClip al AudioSource desde el Inspector.
    public void PlayFootstepSound()
    {
        footstepSound.Play();
    }
}
