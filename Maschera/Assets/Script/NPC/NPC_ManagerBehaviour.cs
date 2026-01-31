using UnityEngine;

/// <summary>
/// Puzzle Manager: urla al telefono. Se Ascolta + risposta "C'è un altro treno fra 5 minuti" → risolto.
/// Assegnare a: NPC/NPC_Manager.
/// </summary>
public class NPC_ManagerBehaviour : MonoBehaviour, INPCInteractable
{
    bool _solved;

    public bool IsSolved() => _solved;

    public void OnListen()
    {
        if (_solved) return;
        _solved = true;
        Debug.Log("[Manager] Ascolti e dici: 'C'è un altro treno fra 5 minuti'. Chiude il telefono. Puzzle risolto!");
        TryNotifyWin();
    }

    public void OnOffer()
    {
        if (_solved) return;
        Debug.Log("[Manager] Offri qualcosa (può dare info orario alternativo)");
    }

    public void OnStay()
    {
        if (_solved) return;
        Debug.Log("[Manager] Stai (non risolve: serve Ascolta con frase giusta)");
    }

    void TryNotifyWin()
    {
        var win = FindFirstObjectByType<WinCondition>();
        win?.NotifyNPCSolved();
    }
}
