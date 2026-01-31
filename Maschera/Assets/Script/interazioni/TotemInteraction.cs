using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemInteraction : MonoBehaviour
{
    [Header("UI Minigioco")]
    public GameObject minigameUIPanel; // Trascina qui il pannello del minigioco (Canvas)
    
    [Header("Messaggio Interazione")]
    public GameObject interactMessage; // Opzionale: un testo "Premi F" che appare quando sei vicino

    private bool isPlayerNearby = false;
    private ControllerMask playerScript;
    private OrbitCamera cameraScript;

    void Start()
    {
        // Assicuriamoci che l'UI del minigioco sia spenta all'inizio
        if(minigameUIPanel != null) minigameUIPanel.SetActive(false);
        if(interactMessage != null) interactMessage.SetActive(false);

        cameraScript = Camera.main.GetComponent<OrbitCamera>();
    }

    void Update()
    {
        // Se il giocatore è vicino e preme F
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            OpenMinigame();
        }
    }

    void OpenMinigame()
    {
        Debug.Log("Avvio Minigioco...");

        // 1. Attiva il pannello UI
        if (minigameUIPanel != null) minigameUIPanel.SetActive(true);
        if (interactMessage != null) interactMessage.SetActive(false); // Nascondi la scritta "Premi F"

        // 2. Blocca il movimento del giocatore
        if (playerScript != null) playerScript.SetLocked(true);

        // 3. Blocca la rotazione della camera
        if (cameraScript != null) cameraScript.SetLocked(true);

        // 4. Sblocca il cursore del mouse (per poter cliccare nel minigioco)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Funzione da collegare al bottone "Chiudi" o "Vittoria" del minigioco
    public void CloseMinigame()
    {
        // 1. Chiudi UI
        if (minigameUIPanel != null) minigameUIPanel.SetActive(false);

        // 2. Sblocca giocatore e camera
        if (playerScript != null) playerScript.SetLocked(false);
        if (cameraScript != null) cameraScript.SetLocked(false);

        // 3. Blocca di nuovo il cursore per il gioco 3D
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Rilevamento collisione (Trigger)
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ControllerMask>())
        {
            isPlayerNearby = true;
            playerScript = other.GetComponent<ControllerMask>();
            
            // Mostra messaggio "Premi F"
            if (interactMessage != null) interactMessage.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ControllerMask>())
        {
            isPlayerNearby = false;
            playerScript = null;

            // Nascondi messaggio
            if (interactMessage != null) interactMessage.SetActive(false);
        }
    }
}