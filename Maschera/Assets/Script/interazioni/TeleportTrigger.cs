using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [Header("Coordinate Destinazione")]
    public Vector3 targetCoordinates;

    void OnTriggerEnter(Collider other)
    {
        // 1. Logga il nome di qualsiasi cosa entri nel trigger
        Debug.Log("Qualcosa è entrato nel trigger: " + other.gameObject.name);

        // 2. Prova a recuperare lo script
        ControllerMask mask = other.GetComponent<ControllerMask>();

        if (mask != null)
        {
            Debug.Log("<color=green>MASCHERA RILEVATA!</color> Teletrasporto a: " + targetCoordinates);
            mask.TeleportTo(targetCoordinates);
        }
        else
        {
            // 3. Logga se l'oggetto non è quello giusto
            Debug.LogWarning("Oggetto rilevato, ma non ha lo script FlyingMaskController.");
        }
    }
}