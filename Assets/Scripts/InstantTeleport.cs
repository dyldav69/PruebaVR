using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InstantTeleport : MonoBehaviour
{
    [Tooltip("Arrastra aquí tu objeto XR Origin o Jugador")]
    public Transform playerTransform;
    [Tooltip("Punto exacto donde aparecerá el jugador sobre esta plataforma")]
    public Vector3 spawnOffset = new Vector3(0, 1.5f, 0);

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable simpleInteractable;

    private void Awake()
    {
        // Intenta obtener o añadir el componente de interacción
        if (!TryGetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>(out simpleInteractable))
        {
            simpleInteractable = gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        }

        // Se suscribe al evento de selección (clic/gatillo del mando)
        simpleInteractable.selectEntered.AddListener(OnSelected);
    }

    private void OnDestroy()
    {
        if (simpleInteractable != null)
            simpleInteractable.selectEntered.RemoveListener(OnSelected);
    }

    // Se corrigió la referencia directa a SelectEnterEventArgs
    private void OnSelected(SelectEnterEventArgs args)
    {
        if (playerTransform != null)
        {
            // Mueve la posición del jugador sobre el cubo
            playerTransform.position = transform.position + spawnOffset;
            Debug.Log($"[Teleport] Jugador movido a {transform.name}");
        }
    }
}