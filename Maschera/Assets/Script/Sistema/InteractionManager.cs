using UnityEngine;

/// <summary>
/// Gestisce le 3 interazioni (Ascolta, Offri, Stai) e quale NPC è selezionato.
/// Assegnare a: Sistema/InteractionManager.
/// UI: 3 pulsanti che chiamano OnListen / OnOffer / OnStay.
/// </summary>
public class InteractionManager : MonoBehaviour
{
    public enum ActionType { Listen, Offer, Stay }

    [Header("Selezione NPC")]
    [Tooltip("La selezione avviene tramite click (NPCSelectionInput sulla Camera). Raggio/layer qui riservati per uso futuro (es. selezione per distanza).")]
    [SerializeField] float selectionRadius = 3f;
    [SerializeField] LayerMask npcLayer;

    Transform _selectedNpc;
    TimeController _timeController;

    void Start()
    {
        _timeController = FindFirstObjectByType<TimeController>();
    }

    /// <summary>
    /// Imposta l'NPC attualmente selezionato (chiamare da click/raycast o trigger).
    /// </summary>
    public void SetSelectedNpc(Transform npc)
    {
        _selectedNpc = npc;
    }

    /// <summary>
    /// Deseleziona l'NPC.
    /// </summary>
    public void ClearSelection()
    {
        _selectedNpc = null;
    }

    public Transform GetSelectedNpc() => _selectedNpc;

    /// <summary>
    /// Chiamare dal pulsante "Ascolta".
    /// </summary>
    public void OnListen()
    {
        if (_selectedNpc == null)
        {
            Debug.Log("[InteractionManager] Ascolta: nessun NPC selezionato. Clicca prima su un personaggio.");
            return;
        }
        _timeController?.OnPlayerAction();
        var interactable = _selectedNpc.GetComponent<INPCInteractable>();
        interactable?.OnListen();
        Debug.Log("[InteractionManager] Ascolta su " + _selectedNpc.name);
    }

    /// <summary>
    /// Chiamare dal pulsante "Offri".
    /// </summary>
    public void OnOffer()
    {
        if (_selectedNpc == null)
        {
            Debug.Log("[InteractionManager] Offri: nessun NPC selezionato. Clicca prima su un personaggio.");
            return;
        }
        _timeController?.OnPlayerAction();
        var interactable = _selectedNpc.GetComponent<INPCInteractable>();
        interactable?.OnOffer();
        Debug.Log("[InteractionManager] Offri su " + _selectedNpc.name);
    }

    /// <summary>
    /// Chiamare dal pulsante "Stai".
    /// </summary>
    public void OnStay()
    {
        if (_selectedNpc == null)
        {
            Debug.Log("[InteractionManager] Stai: nessun NPC selezionato. Clicca prima su un personaggio.");
            return;
        }
        _timeController?.OnPlayerAction();
        var interactable = _selectedNpc.GetComponent<INPCInteractable>();
        interactable?.OnStay();
        Debug.Log("[InteractionManager] Stai su " + _selectedNpc.name);
    }
}
