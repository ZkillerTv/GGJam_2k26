using UnityEngine;

/// <summary>
/// Puzzle Terzo: chiede caricabatterie. Se Offri telefono O Stai + accompagni a Punto_Accompagno → risolto.
/// Assegnare a: NPC/NPC_Terzo.
/// </summary>
public class NPC_TerzoBehaviour : MonoBehaviour, INPCInteractable
{
    [Header("Puzzle")]
    [SerializeField] Transform puntoAccompagno;
    [SerializeField] float maxDistanceToGoal = 2f;

    bool _solved;

    public bool IsSolved() => _solved;

    public void OnListen()
    {
        if (_solved) return;
        Debug.Log("[Terzo] Ascolti: 'Mi serve solo una chiamata, batteria al 3%'");
    }

    public void OnOffer()
    {
        if (_solved) return;
        _solved = true;
        Debug.Log("[Terzo] Offri il telefono. 'Grazie... davvero.' Puzzle risolto!");
        TryNotifyWin();
    }

    public void OnStay()
    {
        if (_solved) return;
        if (puntoAccompagno != null && Vector3.Distance(transform.position, puntoAccompagno.position) <= maxDistanceToGoal)
        {
            _solved = true;
            Debug.Log("[Terzo] Lo accompagni alla biglietteria. 'Grazie... davvero.' Puzzle risolto!");
            TryNotifyWin();
        }
        else
            Debug.Log("[Terzo] Stai con lui (accompagnalo fino a Biglietteria per risolvere)");
    }

    void TryNotifyWin()
    {
        var win = FindFirstObjectByType<WinCondition>();
        win?.NotifyNPCSolved();
    }
}
