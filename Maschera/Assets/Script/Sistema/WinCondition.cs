using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controlla se tutti gli NPC sono risolti; quando sì → vittoria (carica scena o messaggio).
/// Assegnare a: Sistema/WinCondition.
/// </summary>
public class WinCondition : MonoBehaviour
{
    [Header("NPC da risolvere (3 obbligatori)")]
    [SerializeField] MonoBehaviour[] npcBehaviours;

    [Header("Vittoria")]
    [Tooltip("Nome scena da caricare alla vittoria (vuoto = solo log)")]
    [SerializeField] string nextSceneName = "";

    void Start()
    {
        if (npcBehaviours == null || npcBehaviours.Length == 0)
        {
            var v = FindFirstObjectByType<NPC_VecchiettaBehaviour>();
            var m = FindFirstObjectByType<NPC_ManagerBehaviour>();
            var t = FindFirstObjectByType<NPC_TerzoBehaviour>();
            int count = (v != null ? 1 : 0) + (m != null ? 1 : 0) + (t != null ? 1 : 0);
            if (count > 0)
            {
                var list = new System.Collections.Generic.List<MonoBehaviour>();
                if (v != null) list.Add(v);
                if (m != null) list.Add(m);
                if (t != null) list.Add(t);
                npcBehaviours = list.ToArray();
            }
        }
    }

    /// <summary>
    /// Chiamare da ogni NPC quando passa a stato "risolto".
    /// </summary>
    public void NotifyNPCSolved()
    {
        if (AllSolved())
            TriggerWin();
    }

    bool AllSolved()
    {
        if (npcBehaviours == null || npcBehaviours.Length == 0)
            return false;
        foreach (var mb in npcBehaviours)
        {
            if (mb is INPCInteractable npc && !npc.IsSolved())
                return false;
        }
        return true;
    }

    void TriggerWin()
    {
        Debug.Log("[WinCondition] Livello completato!");
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }
}
