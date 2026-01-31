using UnityEngine;

/// <summary>
/// Puzzle Vecchietta: "Che ore sono?".
/// Se risposta = orario → loop. Se azione = Stai (vicino a Punto_Seduta) → dopo X secondi sblocca e risolto.
/// Assegnare a: NPC/NPC_Vecchietta.
/// </summary>
public class NPC_VecchiettaBehaviour : MonoBehaviour, INPCInteractable
{
    [Header("Puzzle")]
    [SerializeField] Transform puntoSeduta;
    [SerializeField] float stayDuration = 3f;
    [SerializeField] float maxDistanceToSeat = 2f;

    bool _solved;
    float _stayTimer;
    bool _isStaying;

    public bool IsSolved() => _solved;

    public void OnListen()
    {
        if (_solved) return;
        Debug.Log("[Vecchietta] Ascolti: 'Che ore sono?' (non rispondere con l'orario)");
    }

    public void OnOffer()
    {
        if (_solved) return;
        Debug.Log("[Vecchietta] Offri qualcosa (non necessario per questo puzzle)");
    }

    public void OnStay()
    {
        if (_solved) return;
        _isStaying = true;
        _stayTimer = 0f;
        Debug.Log("[Vecchietta] Stai con lei... resta vicino alla panchina.");
    }

    void Update()
    {
        if (!_isStaying || _solved) return;

        if (puntoSeduta != null && Vector3.Distance(transform.position, puntoSeduta.position) > maxDistanceToSeat)
        {
            _isStaying = false;
            return;
        }

        _stayTimer += Time.deltaTime;
        if (_stayTimer >= stayDuration)
        {
            _solved = true;
            _isStaying = false;
            Debug.Log("[Vecchietta] 'Mio marito lavorava qui...' Puzzle risolto!");
            TryNotifyWin();
        }
    }

    void TryNotifyWin()
    {
        var win = FindFirstObjectByType<WinCondition>();
        win?.NotifyNPCSolved();
    }
}
