using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnclarCamara : MonoBehaviour
{
    public GameObject camera;
    Vector3 pos;
    PlayerControl player;
    bool reachPlayer;
    private float elapsedTime = 0.0f; // Tiempo transcurrido desde el inicio del movimiento

    bool anclado;
    float duration;

    Quaternion SaveRot;
    Quaternion SaveRot2;

    float rot;
    public float rot2; 

    void Start()
    {
        anclado = false;
        reachPlayer = false;
        player = GameObject.FindObjectOfType<PlayerControl>();
    }

    void Update()
    {
        if(reachPlayer)
        {
            elapsedTime += Time.deltaTime;

            // Calcular la fracción de tiempo transcurrido en relación con la duración total del movimiento
            float t = Mathf.Clamp01(elapsedTime / 0.25f);

            // Interpolar entre la posición inicial y final usando Lerp
            transform.position = Vector3.Lerp(pos, player.transform.position, t);

            // Si el tiempo transcurrido supera la duración del movimiento, detener el movimiento
            if (elapsedTime >= 0.25f)
            {
                player.blockCameraRotation = false;

                reachPlayer = false; // Desactivar este script para evitar actualizaciones adicionales
                camera.transform.localPosition = Vector3.zero;
            }
        }

        if (anclado)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < 0.25f)
            {
                float t = (elapsedTime / 0.15f);
                Quaternion targetRotation = Quaternion.Euler(player.transform.GetChild(0).eulerAngles + new Vector3(0, rot, 0));
                camera.transform.GetChild(0).GetChild(0).rotation = Quaternion.Lerp(SaveRot, targetRotation, t);
                camera.transform.GetChild(0).GetChild(0).GetChild(0).localRotation = Quaternion.Lerp(SaveRot2, Quaternion.Euler(0, rot2, 0), t);
            }
        }


    }
    public void AnclarCamera(float duration)
    {
        elapsedTime = 0;
        this.duration = duration;
        // Cambiar el padre del objeto
        camera.transform.parent = GameObject.FindWithTag("Slashes").transform;
        anclado = true;
        // Restaurar la posición y rotación local del objeto para mantener su posición relativa al nuevo padre
        player.blockCameraRotation = true;
        SaveRot = camera.transform.GetChild(0).GetChild(0).transform.rotation;
        SaveRot2 = camera.transform.GetChild(0).GetChild(0).GetChild(0).transform.localRotation;
        Invoke("Desanclar", duration);
    }

    public void SetCameraRotation(float rotation)
    {
        rot = rotation;
    }

    public void Desanclar()
    {
        anclado = false;

        pos = camera.transform.position;
        camera.transform.parent = player.transform;
        elapsedTime = 0;
        reachPlayer = true;

        //camera.GetComponent<Rigidbody>().DOMove(player.transform.position, 0.25f, false);
    }

}
