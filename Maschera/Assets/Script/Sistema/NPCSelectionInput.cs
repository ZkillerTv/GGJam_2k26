using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Seleziona l'NPC al click (raycast dalla camera).
/// Assegnare a: Main Camera o Player.
/// - Scena 2D (livello_empatia): Use2D = true, NPC con Collider2D (es. BoxCollider2D).
/// - Scena 3D: Use2D = false, NPC con Collider (es. BoxCollider).
/// - Canvas: se i click sui personaggi non arrivano, sul Panel (o Image a schermo intero) disattiva "Raycast Target" così solo i pulsanti intercettano i click.
/// </summary>
public class NPCSelectionInput : MonoBehaviour
{
    [Header("Camera e modalità")]
    [Tooltip("Se true usa Physics2D (scena 2D con Collider2D). Se false usa Physics 3D.")]
    [SerializeField] bool use2D = true;

    [SerializeField] Camera raycastCamera;
    [SerializeField] LayerMask npcLayers = -1;

    [Header("Debug e UI")]
    [Tooltip("Se true ignora i click quando il puntatore è sopra la UI. Disattiva se i click sui personaggi non arrivano.")]
    [SerializeField] bool skipClickWhenOverUI = false;
    [Tooltip("Attiva per vedere in Console ogni click (colpito / non colpito).")]
    [SerializeField] bool debugClick = true;

    const float RayDistance = 500f;
    const float OverlapRadiusFallback = 1.5f;

    InteractionManager _interactionManager;

    void Start()
    {
        if (raycastCamera == null)
            raycastCamera = Camera.main;
        _interactionManager = FindFirstObjectByType<InteractionManager>();
        if (_interactionManager == null)
            Debug.LogWarning("[NPCSelectionInput] InteractionManager non trovato in scena.");
        else
            Debug.Log("[NPCSelectionInput] Pronto. Use2D=" + use2D + ", Camera=" + (raycastCamera != null ? raycastCamera.name : "null") + ". Clicca su un personaggio (serve Box Collider 2D).");
    }

    void Update()
    {
        if (_interactionManager == null || !Input.GetMouseButtonDown(0)) return;
        // Non selezionare NPC se il click è su UI (pulsanti): così i pulsanti funzionano e il mondo riceve i click solo dove non c’è UI
        if (skipClickWhenOverUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (raycastCamera == null)
        {
            raycastCamera = Camera.main;
            if (raycastCamera == null) return;
        }

        Transform hitTransform = use2D ? GetHitTransform2D() : GetHitTransform3D(raycastCamera.ScreenPointToRay(Input.mousePosition));

        if (hitTransform == null)
        {
            if (debugClick) Debug.Log("[NPCSelectionInput] Click: nessun collider colpito (aggiungi Box Collider 2D agli NPC?).");
            _interactionManager.ClearSelection();
            return;
        }

        Transform npcRoot = GetNpcRoot(hitTransform);
        if (npcRoot != null)
        {
            Debug.Log("[Stazione] Selezionato: " + npcRoot.name + " – ora clicca Ascolta / Offri / Stai.");
            _interactionManager.SetSelectedNpc(npcRoot);
        }
        else
        {
            if (debugClick) Debug.Log("[NPCSelectionInput] Click: collider colpito ma nessuno script NPC (INPCInteractable) su questo oggetto.");
            _interactionManager.ClearSelection();
        }
    }

    /// <summary>2D: posizione mondo del mouse + OverlapPoint/OverlapCircle (più affidabile con Canvas).</summary>
    Transform GetHitTransform2D()
    {
        Vector3 screen = Input.mousePosition;
        screen.z = Mathf.Abs(raycastCamera.transform.position.z);
        Vector2 worldPos = raycastCamera.ScreenToWorldPoint(screen);

        // Se Npc Layers = Nothing (0) in Inspector, usa tutto (-1) altrimenti non colpiamo mai
        int mask = npcLayers.value == 0 ? -1 : npcLayers.value;

        Collider2D hit = Physics2D.OverlapPoint(worldPos, mask);
        if (hit == null)
            hit = Physics2D.OverlapCircle(worldPos, OverlapRadiusFallback, mask);
        return hit != null ? hit.transform : null;
    }

    Transform GetHitTransform3D(Ray ray)
    {
        int mask = npcLayers.value == 0 ? -1 : npcLayers.value;
        if (Physics.Raycast(ray, out RaycastHit hit, RayDistance, mask))
            return hit.transform;
        return null;
    }

    /// <summary>
    /// Risale dalla collider colpita fino a trovare un GameObject con INPCInteractable.
    /// </summary>
    static Transform GetNpcRoot(Transform from)
    {
        Transform current = from;
        while (current != null)
        {
            if (current.GetComponent<INPCInteractable>() != null)
                return current;
            current = current.parent;
        }
        return null;
    }
}
