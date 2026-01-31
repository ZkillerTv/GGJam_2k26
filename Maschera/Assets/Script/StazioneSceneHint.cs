using UnityEngine;

/// <summary>
/// Mostra nel Inspector una nota di design per un GameObject della scena Stazione.
/// Usato dall'editor "Crea struttura scena stazione" per documentare ogni nodo.
/// </summary>
public class StazioneSceneHint : MonoBehaviour
{
    [TextArea(2, 6)]
    public string DesignNote = "";
}
