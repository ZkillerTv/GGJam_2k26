using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RockDrag2D : MonoBehaviour
{
    [Header("Impostazioni Movimento")]
    [Tooltip("Velocità di inseguimento del mouse. Più è basso, più è 'Zen'.")]
    public float followSpeed = 20f;
    
    [Tooltip("Distanza della roccia dalla telecamera (di solito 10 per giochi 2D).")]
    public float zOffset = 10f;

    private GameObject selectedRock;
    private Rigidbody2D rb;
    private bool isDragging = false;

    void Update()
    {
        // 1. RILEVAMENTO DEL CLICK (Inizio Trascinamento)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartDragging();
        }

        // 2. LOGICA DI TRASCINAMENTO (Durante la pressione)
        if (isDragging && selectedRock != null)
        {
            UpdatePosition();
        }

        // 3. RILASCIO (Fine Trascinamento)
        if (Mouse.current.leftButton.wasReleasedThisFrame && selectedRock != null)
        {
            StopDragging();
        }
    }

    private void StartDragging()
    {
        // Spariamo un raggio dalla camera verso il mouse
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.CompareTag("Rock"))
        {
            selectedRock = hit.collider.gameObject;
            rb = selectedRock.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Disabilitiamo la fisica per poterla muovere liberamente
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            isDragging = true;
            Debug.Log("Roccia agganciata: " + selectedRock.name);
        }
    }

    private void UpdatePosition()
    {
        // Prendiamo la posizione del mouse in pixel
        Vector3 mousePos = Mouse.current.position.ReadValue();
        
        // Impostiamo la profondità corretta per la conversione in coordinate mondo
        mousePos.z = zOffset; 

        // Convertiamo i pixel in coordinate del mondo di gioco
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePos);
        
        // Manteniamo l'oggetto a Z = 0 (fondamentale per il 2D)
        targetPosition.z = 0f;

        // Muoviamo la roccia verso il mouse in modo fluido
        selectedRock.transform.position = Vector3.Lerp(selectedRock.transform.position, targetPosition, Time.deltaTime * followSpeed);
    }

    private void StopDragging()
    {
        if (rb != null)
        {
            // Riattiviamo la gravità e la fisica
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        Debug.Log("Roccia rilasciata.");
        isDragging = false;
        selectedRock = null;
    }
}
