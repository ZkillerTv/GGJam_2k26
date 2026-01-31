/// <summary>
/// Interfaccia per gli NPC che rispondono a Ascolta / Offri / Stai.
/// </summary>
public interface INPCInteractable
{
    void OnListen();
    void OnOffer();
    void OnStay();
    bool IsSolved();
}
