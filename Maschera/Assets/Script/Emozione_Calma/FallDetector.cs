using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetector : MonoBehaviour
{
   [Header("Configurazione Respawn Random")]
    [Tooltip("Valore minimo di X (sinistra).")]
    public float minX = -2f;
    [Tooltip("Valore massimo di X (destra).")]
    public float maxX = 2f;
    [Tooltip("Altezza fissa di spawn (asse Y).")]
    public float spawnY = 6f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rock"))
        {
            RespawnRock(other.gameObject);
        }
    }

    private void RespawnRock(GameObject rock)
    {
        // 1. Calcolo della posizione X randomica
        float randomX = Random.Range(minX, maxX);
        Vector3 newSpawnPos = new Vector3(randomX, spawnY, 0);

        // 2. Reset della posizione
        rock.transform.position = newSpawnPos;

        // 3. Reset della fisica (per evitare che mantenga la velocità di caduta)
        Rigidbody2D rb = rock.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        Debug.Log($"Sasso recuperato! Spawnato a X: {randomX}");
    }

    // Disegna un'anteprima visiva nell'editor per aiutarti a regolare i valori
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 start = new Vector3(minX, spawnY, 0);
        Vector3 end = new Vector3(maxX, spawnY, 0);
        Gizmos.DrawLine(start, end);
        Gizmos.DrawSphere(start, 0.1f);
        Gizmos.DrawSphere(end, 0.1f);
    }
}
