
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    [Header("Configuración")]
    public Activity2Manager administrador;

    [Header("Diagnóstico")]
    [SerializeField] private bool recogido = false;

    private void Start()
    {
        if (administrador == null)
            administrador = FindFirstObjectByType<Activity2Manager>();

        if (administrador == null)
        {
            Debug.LogError(
                $"[Coleccionable] No se encontró Activity2Manager en {name}.",
                this);

            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (recogido || administrador == null)
            return;

        if (!administrador.EsJugador(other))
            return;

        recogido = true;

        administrador.RecogerObjeto(gameObject);

        gameObject.SetActive(false);

        Debug.Log(
            $"[Coleccionable] {name} recogido permanentemente.");
    }
}
