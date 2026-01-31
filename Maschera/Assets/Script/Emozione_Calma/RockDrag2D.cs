using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RockDrag2D : MonoBehaviour
{
    [Header("Impostazioni Movimento")]
    public float followSpeed = 15f;
    public float zOffset = 10f;
    
    [Header("Impostazioni Lancio")]
    [Tooltip("Forza impressa al rilascio.")]
    public float throwForceMultiplier = 0.5f;
    [Tooltip("Velocità massima che la roccia può raggiungere al lancio.")]
    public float maxThrowSpeed = 10f;

    [Header("Impostazioni Rotazione")]
    public float rotationSpeed = 150f;

    private GameObject selectedRock;
    private Rigidbody2D rb;
    private bool isDragging = false;

    private Vector2 lastPosition;
    private Vector2 currentVelocity;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartDragging();
        }

        if (isDragging && selectedRock != null)
        {
            UpdatePositionAndVelocity();
            HandleRotation();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && selectedRock != null)
        {
            StopDragging();
        }
    }

    private void StartDragging()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.CompareTag("Rock"))
        {
            selectedRock = hit.collider.gameObject;
            rb = selectedRock.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            isDragging = true;
            lastPosition = selectedRock.transform.position;
        }
    }

    private void UpdatePositionAndVelocity()
    {
        Vector3 mouseInput = Mouse.current.position.ReadValue();
        mouseInput.z = zOffset;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mouseInput);
        targetPos.z = 0f;

        // Calcolo velocità basato sullo spostamento effettivo
        // Usiamo un piccolo smorzamento (0.1f) invece di deltaTime puro per evitare picchi
        Vector2 frameVelocity = ((Vector2)targetPos - lastPosition) / Time.deltaTime;
        
        // Applichiamo un filtro per rendere la velocità meno nervosa
        currentVelocity = Vector2.Lerp(currentVelocity, frameVelocity, 20f * Time.deltaTime);
        lastPosition = selectedRock.transform.position;

        selectedRock.transform.position = Vector3.Lerp(selectedRock.transform.position, targetPos, Time.deltaTime * followSpeed);
    }

    private void HandleRotation()
    {
        if (Keyboard.current.aKey.isPressed)
            selectedRock.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        if (Keyboard.current.dKey.isPressed)
            selectedRock.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }

    private void StopDragging()
    {
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            
            // CALCOLO DEL LANCIO CON LIMITATORE
            Vector2 throwVelocity = currentVelocity * throwForceMultiplier;
            
            // Limitiamo la velocità massima per evitare che schizzi via
            rb.velocity = Vector2.ClampMagnitude(throwVelocity, maxThrowSpeed);
        }

        isDragging = false;
        selectedRock = null;
    }
    public bool GetIsDragging()
    {
        return isDragging;
    }
}
