using UnityEngine;

/// <summary>
/// Il tempo avanza SOLO quando il giocatore compie un'azione (Ascolta/Offri/Stai).
/// Assegnare a: Sistema/TimeController.
/// </summary>
public class TimeController : MonoBehaviour
{
    [Header("Tempo a turni")]
    [Tooltip("Tempo in secondi per cui il tempo avanza dopo ogni azione (0 = turni fissi)")]
    [SerializeField] float tickDuration = 2f;

    [Tooltip("TimeScale quando nessuna azione (tempo fermo)")]
    [SerializeField] float timeScaleIdle = 0f;

    [Tooltip("TimeScale durante un tick dopo un'azione")]
    [SerializeField] float timeScaleActive = 1f;

    float _tickRemaining;

    void Start()
    {
        Time.timeScale = timeScaleIdle;
    }

    void Update()
    {
        if (_tickRemaining > 0f)
        {
            _tickRemaining -= Time.unscaledDeltaTime;
            if (_tickRemaining <= 0f)
                Time.timeScale = timeScaleIdle;
        }
    }

    /// <summary>
    /// Chiamare da InteractionManager quando il player sceglie Ascolta/Offri/Stai su un NPC.
    /// </summary>
    public void OnPlayerAction()
    {
        Time.timeScale = timeScaleActive;
        _tickRemaining = tickDuration;
    }
}
