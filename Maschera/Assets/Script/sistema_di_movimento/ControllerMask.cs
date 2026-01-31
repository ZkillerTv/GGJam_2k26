using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMask : MonoBehaviour
{
    [Header("Impostazioni Movimento")]
    public float moveSpeed = 6.0f;
    public float acceleration = 10.0f;
    public float rotationSpeed = 10.0f; // Quanto velocemente la maschera si gira verso la direzione di movimento

    [Header("Impostazioni Volo")]
    public float hoverHeight = 2.0f;
    public float hoverFrequency = 1.5f;
    public float hoverAmplitude = 0.3f;

    [Header("Impostazioni Inclinazione")]
    public float tiltAmount = 20.0f;
    public float tiltSpeed = 5.0f;

    private Vector3 currentVelocity;
    private Transform camTransform; // Riferimento alla telecamera
    private bool isLocked = false;

    void Start()
    {
        // Prende automaticamente la Main Camera
        if (Camera.main != null)
        {
            camTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("Nessuna Main Camera trovata nella scena!");
        }
    }

    void Update()
    {
    if (isLocked) return; // SE BLOCCATO, NON FARE NULLA

    HandleMovement();
    HandleHover();
    }

    public void SetLocked(bool state)
    {  
    isLocked = state;
    // Se ci blocchiamo, fermiamo subito la velocità per evitare scivolamenti
    if (state) currentVelocity = Vector3.zero;
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Calcoliamo la direzione basata sulla telecamera
        Vector3 cameraForward = camTransform.forward;
        Vector3 cameraRight = camTransform.right;

        // Appiattiamo i vettori su Y (non vogliamo volare verso il basso se guardiamo giù)
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Direzione finale desiderata
        Vector3 moveDir = (cameraForward * v + cameraRight * h).normalized;

        // Calcolo velocità fluida
        Vector3 targetVelocity = moveDir * moveSpeed;
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.deltaTime * acceleration);

        // Applica movimento
        transform.Translate(currentVelocity * Time.deltaTime, Space.World);

        // Rotazione del personaggio (La maschera guarda dove cammina)
        if (moveDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            
            // Aggiungiamo il "Tilting" (Inclinazione) alla rotazione base
            // Calcoliamo l'inclinazione locale basata sulla velocità laterale relativa
            float tiltZ = -h * tiltAmount; // Roll
            float tiltX = v * tiltAmount * 0.5f; // Pitch leggero in avanti quando corre

            // Combiniamo la rotazione verso la direzione + l'inclinazione
            Quaternion tiltRotation = Quaternion.Euler(tiltX, 0, tiltZ);
            
            // Interpoliamo la rotazione
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * tiltRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            // Se siamo fermi, raddrizziamo la maschera lentamente (togliamo il tilt)
            Vector3 euler = transform.rotation.eulerAngles;
            Quaternion upright = Quaternion.Euler(0, euler.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, upright, Time.deltaTime * tiltSpeed);
        }
    }

    void HandleHover()
    {
        // Movimento sinusoidale su Y indipendente
        float newY = hoverHeight + Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        Vector3 pos = transform.position;
        pos.y = newY;
        transform.position = pos;
    }

    //Per il teletrasporto
    public void TeleportTo(Vector3 destination)
    {
        // 1. Sposta istantaneamente l'oggetto
        transform.position = destination;

        // 2. Azzera la velocità e l'inerzia
        // Questo evita che la maschera "scivoli" via appena arrivata
        currentVelocity = Vector3.zero; 
        
        // (Opzionale) Se vuoi che si raddrizzi quando arriva:
        // transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}