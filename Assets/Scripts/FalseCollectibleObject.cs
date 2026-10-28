
using UnityEngine;

public class FalseCollectibleObject : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject cartelPistas;

    [Header("Diagnóstico")]
    [SerializeField] private bool activado = false;

    private void Start()
    {
        if (cartelPistas == null)
        {
            Debug.LogError(
                $"[Pista] No se asignó el cartel en {name}.",
                this
            );
            enabled = false;
            return;
        }

        cartelPistas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activado)
            return;

        // Detectar al jugador mediante el sistema existente.
        Activity2Manager administrador =
            FindFirstObjectByType<Activity2Manager>();

        if (administrador == null ||
            !administrador.EsJugador(other))
            return;

        activado = true;
        cartelPistas.SetActive(true);

        Debug.Log($"[Pista] Cartel activado por {name}.");
    }
}
