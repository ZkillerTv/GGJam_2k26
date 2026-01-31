using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEditor.Events;

/// <summary>
/// Crea la gerarchia di GameObjects per la scena "Stazione" (livello empatia).
/// Menu: Tools > Stazione > Crea struttura scena stazione
///
/// DESIGN:
/// - Spazio unico: una stazione, una sola schermata.
/// - Panchina centrale, tabellone treni (cambia raramente), 3-4 NPC fissi.
/// - Tempo scorre solo quando il giocatore agisce.
/// - 3 interazioni: Ascolta, Offri, Stai.
/// </summary>
public static class StazioneSceneSetup
{
    const string MenuPath = "Tools/Stazione/Crea struttura scena stazione";
    const string AssignPath = "Tools/Stazione/Assegna script e collider (click sui personaggi)";
    const string CreateUIPath = "Tools/Stazione/Crea pulsanti Ascolta Offri Stai";

    [MenuItem(AssignPath)]
    public static void AssignScriptsAndColliders()
    {
        var root = GameObject.Find("_Stazione")?.transform;
        if (root == null)
        {
            Debug.LogWarning("[Stazione] _Stazione non trovata. Esegui prima: Tools > Stazione > Crea struttura scena stazione");
            return;
        }

        int count = 0;

        // Sistema
        var sistema = root.Find("Sistema");
        if (sistema != null)
        {
            Add<InteractionManager>(sistema.Find("InteractionManager")?.gameObject, ref count);
            Add<TimeController>(sistema.Find("TimeController")?.gameObject, ref count);
            Add<WinCondition>(sistema.Find("WinCondition")?.gameObject, ref count);
        }

        // Main Camera (cerca in tutta la scena)
        var cam = Object.FindFirstObjectByType<Camera>();
        if (cam != null)
        {
            Add<NPCSelectionInput>(cam.gameObject, ref count);
        }
        else
        {
            Debug.LogWarning("[Stazione] Nessuna Camera in scena: aggiungi una Camera e riesegui.");
        }

        // NPC: script sul root + Collider2D per il click
        var npcRoot = root.Find("NPC");
        if (npcRoot != null)
        {
            AssignNpc(npcRoot.Find("NPC_Vecchietta")?.gameObject, typeof(NPC_VecchiettaBehaviour), ref count);
            AssignNpc(npcRoot.Find("NPC_Manager")?.gameObject, typeof(NPC_ManagerBehaviour), ref count);
            AssignNpc(npcRoot.Find("NPC_Terzo")?.gameObject, typeof(NPC_TerzoBehaviour), ref count);
        }

        Undo.RegisterCompleteObjectUndo(root.gameObject, "Stazione assign scripts");
        Debug.Log($"[Stazione] Assegnati script e collider ({count} componenti). In Play: clicca su un personaggio poi sui pulsanti Ascolta/Offri/Stai. Se non hai ancora i pulsanti: Tools > Stazione > Crea pulsanti Ascolta Offri Stai.");
    }

    [MenuItem(CreateUIPath)]
    public static void CreateInteractionButtons()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            canvas = CreateCanvasAndEventSystem();
            if (canvas == null) return;
        }

        var imGo = GameObject.Find("InteractionManager");
        if (imGo == null)
        {
            var stazione = GameObject.Find("_Stazione");
            if (stazione != null)
                imGo = stazione.transform.Find("Sistema/InteractionManager")?.gameObject;
        }
        if (imGo == null)
            imGo = Object.FindFirstObjectByType<InteractionManager>()?.gameObject;
        if (imGo == null)
        {
            Debug.LogWarning("[Stazione] InteractionManager non trovato. Esegui prima: Tools > Stazione > Assegna script e collider.");
            return;
        }
        var im = imGo.GetComponent<InteractionManager>();
        if (im == null)
        {
            Debug.LogWarning("[Stazione] Il GameObject InteractionManager non ha il componente InteractionManager.");
            return;
        }

        Transform parent = canvas.transform;

        // Pulsanti in basso al centro, grandi e ben distanziati
        float width = 220f;
        float height = 50f;
        float spacing = 60f;
        float yFromBottom = 120f;

        var b1 = CreateButton(parent, "Btn_Ascolta", "Ascolta", new Vector2(0, yFromBottom), new Vector2(width, height), im.OnListen);
        var b2 = CreateButton(parent, "Btn_Offri", "Offri", new Vector2(0, yFromBottom - spacing), new Vector2(width, height), im.OnOffer);
        var b3 = CreateButton(parent, "Btn_Stai", "Stai", new Vector2(0, yFromBottom - spacing * 2), new Vector2(width, height), im.OnStay);

        Undo.RegisterCreatedObjectUndo(b1, "Stazione UI buttons");
        Undo.RegisterCreatedObjectUndo(b2, "Stazione UI buttons");
        Undo.RegisterCreatedObjectUndo(b3, "Stazione UI buttons");
        Selection.activeGameObject = b1;
        Debug.Log("[Stazione] Creati 3 pulsanti (Ascolta, Offri, Stai) sotto il Canvas. Se non li vedi in Game, controlla in Hierarchy: Canvas > Btn_Ascolta, Btn_Offri, Btn_Stai.");
    }

    static Canvas CreateCanvasAndEventSystem()
    {
        if (Object.FindFirstObjectByType<EventSystem>() == null)
        {
            var esGo = new GameObject("EventSystem");
            esGo.AddComponent<EventSystem>();
            esGo.AddComponent<StandaloneInputModule>();
            Undo.RegisterCreatedObjectUndo(esGo, "EventSystem");
        }

        var canvasGo = new GameObject("Canvas");
        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasGo.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
        canvasGo.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.5f;
        canvasGo.AddComponent<GraphicRaycaster>();
        var rt = canvasGo.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        Undo.RegisterCreatedObjectUndo(canvasGo, "Canvas");
        return canvas;
    }

    static GameObject CreateButton(Transform parent, string name, string label, Vector2 anchoredPos, Vector2 sizeDelta, UnityEngine.Events.UnityAction onClick)
    {
        var btn = new GameObject(name);
        btn.transform.SetParent(parent, false);

        var rt = btn.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0f);
        rt.anchorMax = new Vector2(0.5f, 0f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = sizeDelta;
        rt.anchoredPosition = anchoredPos;

        btn.AddComponent<CanvasRenderer>();
        var img = btn.AddComponent<Image>();
        img.color = new Color(0.25f, 0.25f, 0.35f, 0.95f);

        var button = btn.AddComponent<Button>();
        UnityEventTools.AddPersistentListener(button.onClick, onClick);

        var textGo = new GameObject("Text");
        textGo.transform.SetParent(btn.transform, false);
        var textRt = textGo.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = new Vector2(4, 2);
        textRt.offsetMax = new Vector2(-4, -2);
        var text = textGo.AddComponent<Text>();
        text.text = label;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 18;
        return btn;
    }

    static void AssignNpc(GameObject npc, System.Type behaviourType, ref int count)
    {
        if (npc == null) return;
        if (npc.GetComponent(behaviourType) == null)
        {
            npc.AddComponent(behaviourType);
            count++;
        }
        EnsureCollider2D(npc, ref count);
    }

    static void EnsureCollider2D(GameObject npc, ref int count)
    {
        // Cerca un figlio con SpriteRenderer (Quad placeholder) o il root
        Transform target = npc.transform;
        foreach (Transform child in npc.transform)
        {
            if (child.GetComponent<SpriteRenderer>() != null || child.GetComponent<Renderer>() != null)
            {
                target = child;
                break;
            }
        }
        if (target.GetComponent<Collider2D>() == null)
        {
            target.gameObject.AddComponent<BoxCollider2D>();
            count++;
        }
    }

    static void Add<T>(GameObject go, ref int count) where T : Component
    {
        if (go == null) return;
        if (go.GetComponent<T>() == null)
        {
            go.AddComponent<T>();
            count++;
        }
    }

    [MenuItem(MenuPath)]
    public static void CreateStazioneHierarchy()
    {
        // Root
        Transform root = CreateOrGet("_Stazione", null, "Root della scena stazione");

        // === AMBIENTE ===
        Transform ambiente = CreateOrGet("Ambiente", root, "Decor e oggetti di scena");
        CreateOrGet("Panchina_Centrale", ambiente, "Panchina centrale dove si siede la vecchietta");
        CreateOrGet("Tabellone_Treni", ambiente, "Tabellone partenze/arrivi (cambia raramente)");
        CreateOrGet("Binari_Pensiline", ambiente, "Opzionale: binari / pensiline");
        CreateOrGet("Biglietteria_InfoPoint", ambiente, "Per puzzle Terzo: accompagnarlo qui");

        // === NPC (3-4 fissi) ===
        Transform npcRoot = CreateOrGet("NPC", root, "Tutti gli NPC fissi della stazione");

        // 1. La vecchietta – puzzle della presenza (Stai > parlare)
        Transform vecchietta = CreateOrGet("NPC_Vecchietta", npcRoot, "Chiede 'Che ore sono?' – soluzione: sedersi/stare, non rispondere all'orario");
        CreateOrGet("Orologio_Visibile", vecchietta, "Ha un orologio ben visibile (indizio)");
        CreateOrGet("Punto_Seduta", vecchietta, "Posizione sulla panchina per la soluzione 'stai'");

        // 2. Il manager – puzzle del fraintendimento (osservazione + orario alternativo)
        Transform manager = CreateOrGet("NPC_Manager", npcRoot, "Urla al telefono, guarda tabellone – soluzione: 'C'è un altro treno fra 5 minuti'");
        CreateOrGet("Telefono", manager, "Urla al telefono (licenziamenti, ritardi)");
        CreateOrGet("Punto_Guardia_Tabellone", manager, "Guarda ossessivamente il tabellone");

        // 3. Il terzo – puzzle dell'urgenza nascosta (offrire telefono / accompagnare)
        Transform terzo = CreateOrGet("NPC_Terzo", npcRoot, "Chiede caricabatterie – soluzione: offrire il TUO telefono o accompagnarlo a biglietteria");
        CreateOrGet("Telefono_Batteria3", terzo, "Batteria al 3%, minimizza 'È solo una chiamata'");
        CreateOrGet("Punto_Accompagno", terzo, "Destinazione accompagnamento (biglietteria/info)");

        // 4. Opzionale: quarto NPC (placeholder)
        CreateOrGet("NPC_Quarto", npcRoot, "Opzionale: quarto NPC fisso");

        // === SISTEMA ===
        Transform sistema = CreateOrGet("Sistema", root, "Logica di gioco");
        CreateOrGet("TimeController", sistema, "Il tempo scorre SOLO quando il giocatore agisce");
        CreateOrGet("InteractionManager", sistema, "Gestisce le 3 interazioni: Ascolta, Offri, Stai");
        CreateOrGet("WinCondition", sistema, "Vittoria: NPC se ne vanno (saliti su treno), stazione vuota");

        // === PLAYER / UI (placeholder) ===
        Transform player = CreateOrGet("Player", root, "Giocatore (camera / avatar)");
        CreateOrGet("UI_Interazioni", root, "UI per le 3 scelte: Ascolta, Offri, Stai");

        Undo.RegisterCreatedObjectUndo(root.gameObject, "Stazione scene structure");
        Selection.activeGameObject = root.gameObject;
        Debug.Log("[Stazione] Struttura creata. Seleziona _Stazione nella Hierarchy.");
    }

    static Transform CreateOrGet(string name, Transform parent, string comment)
    {
        Transform existing = parent != null ? parent.Find(name) : GameObject.Find(name)?.transform;
        if (existing != null)
            return existing;

        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        if (!string.IsNullOrEmpty(comment))
            AddComment(go, comment);
        return go.transform;
    }

    static void AddComment(GameObject go, string comment)
    {
        var hint = go.AddComponent<StazioneSceneHint>();
        if (hint != null) hint.DesignNote = comment;
    }
}
