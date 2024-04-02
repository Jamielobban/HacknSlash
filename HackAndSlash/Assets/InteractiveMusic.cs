using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class InteractiveMusic : MonoBehaviour
{
    public AudioMixerSnapshot snapshotEntrada; // El snapshot que se activará al entrar en el trigger
    public AudioMixerSnapshot snapshotSalida; // El snapshot que se activará al salir del trigger

    private bool dentroDelTrigger = false;

    private void Start()
    {
        CambiarSnapshot(snapshotSalida);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que entra al trigger es el jugador
        {
            if (!dentroDelTrigger) // Verifica si ya estaba dentro del trigger
            {
                dentroDelTrigger = true;
                CambiarSnapshot(snapshotEntrada); // Cambia al snapshot de entrada
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que sale del trigger es el jugador
        {
            dentroDelTrigger = false;
            CambiarSnapshot(snapshotSalida); // Cambia al snapshot de salida
        }
    }

    private void CambiarSnapshot(AudioMixerSnapshot snapshot)
    {
        snapshot.TransitionTo(1.0f); // Transición al snapshot indicado durante 1 segundo
    }
}
