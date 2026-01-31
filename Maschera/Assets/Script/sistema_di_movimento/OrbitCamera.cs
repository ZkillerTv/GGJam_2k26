using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;            // Trascina qui la tua Maschera
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Sposta il punto di fuoco un po' sopra il centro della maschera

    [Header("Impostazioni Orbit")]
    public float distance = 6.0f;       // Distanza dalla maschera
    public float sensitivityX = 4.0f;   // Velocità rotazione orizzontale
    public float sensitivityY = 2.0f;   // Velocità rotazione verticale
    
    [Header("Limiti")]
    public float minY = -40f;           // Limite guardare sotto
    public float maxY = 80f;            // Limite guardare sopra

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private bool isLocked = false;

    void Start()
    {
        // Blocca il cursore al centro dello schermo e lo nasconde (come negli FPS/TPS)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null || isLocked) return; // SE BLOCCATO, FERMA LA CAMERA
        if (target == null) return;

        // 1. Leggi Input Mouse
        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

        // 2. Limita l'angolo verticale (per non ribaltarsi)
        currentY = Mathf.Clamp(currentY, minY, maxY);

        // 3. Calcola la rotazione e la posizione
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        
        // La posizione è: Posizione Target + Offset - (Direzione Telecamera * Distanza)
        Vector3 position = (target.position + offset) - (rotation * Vector3.forward * distance);

        // 4. Applica trasformazioni
        transform.rotation = rotation;
        transform.position = position;
    }

    public void SetLocked(bool state)
    {
    isLocked = state;
    }
}