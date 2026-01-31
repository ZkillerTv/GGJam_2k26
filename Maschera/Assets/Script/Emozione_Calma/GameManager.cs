using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Configurazione Vittoria")]
    public int pietreRichieste = 6;
    public float tempoStabilita = 3.0f;
    public float sogliaVelocita = 0.1f;

    [Header("Controlli Impilamento")]
    [Tooltip("Quanto possono essere distanti le pietre dall'asse centrale della base.")]
    public float tolleranzaOrizzontale = 1.0f; 
    [Tooltip("L'altezza minima che la cima della torre deve raggiungere.")]
    public float altezzaMinimaVittoria = 3.0f;
    public Transform baseDellaTorre; // Trascina qui il tuo Cubo/Base

    [Header("Riferimenti")]
    public RockDrag2D dragScript;
    public GameObject pannelloVittoria;

    private float timerStabile = 0f;
    private bool haVinto = false;

    void Update()
    {
        if (haVinto) return;

        GameObject[] pietre = GameObject.FindGameObjectsWithTag("Rock");

        // 1. Condizione base: numero di pietre e nessun trascinamento in corso
        if (pietre.Length >= pietreRichieste && !dragScript.GetIsDragging())
        {
            // 2. Controllo se sono allineate, abbastanza alte e ferme
            if (CheckAllineamentoEAltezza(pietre) && TuttePietreFerme(pietre))
            {
                timerStabile += Time.deltaTime;
                if (timerStabile >= tempoStabilita)
                {
                    Vittoria();
                }
            }
            else
            {
                timerStabile = 0f;
            }
        }
        else
        {
            timerStabile = 0f;
        }
    }

    bool CheckAllineamentoEAltezza(GameObject[] pietre)
    {
        float altezzaMassima = -Mathf.Infinity;
        float xBase = baseDellaTorre.position.x;

        foreach (GameObject pietra in pietre)
        {
            // Controllo se la pietra è troppo lontana dal centro della base (asse X)
            if (Mathf.Abs(pietra.transform.position.x - xBase) > tolleranzaOrizzontale)
            {
                return false; // Una pietra è caduta fuori dalla base
            }

            // Troviamo il punto più alto della torre
            if (pietra.transform.position.y > altezzaMassima)
            {
                altezzaMassima = pietra.transform.position.y;
            }
        }

        // Controllo se la torre è abbastanza alta rispetto alla base
        float altezzaEffettiva = altezzaMassima - baseDellaTorre.position.y;
        return altezzaEffettiva >= altezzaMinimaVittoria;
    }

    bool TuttePietreFerme(GameObject[] pietre)
    {
        foreach (GameObject pietra in pietre)
        {
            Rigidbody2D rb = pietra.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (rb.velocity.magnitude > sogliaVelocita || Mathf.Abs(rb.angularVelocity) > sogliaVelocita)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void Vittoria()
    {
        haVinto = true;
        Debug.Log("HAI VINTO! Torre stabile e alta.");
        if (pannelloVittoria != null) pannelloVittoria.SetActive(true);
    }
}
