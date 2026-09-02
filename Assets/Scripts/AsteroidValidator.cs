using UnityEngine;

public class AsteroidValidator : MonoBehaviour
{
    [Header("Configuración Matemática")]
    [Tooltip("Marca esta casilla SOLO si este portal está en un máximo/mínimo real de la función")]
    public bool isCorrectChoice = false;

    [Header("Referencias")]
    [Tooltip("Arrastra aquí el asteroide/plataforma que caerá si la elección es errónea")]
    public Rigidbody platformRigidbody;

    private bool evaluated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (evaluated) return;

        // Verifica si quien pisó el portal es el jugador de VR
        if (other.CompareTag("Player") || other.GetComponentInParent<Unity.XR.CoreUtils.XROrigin>() != null)
        {
            evaluated = true;

            if (isCorrectChoice)
            {
                Debug.Log("<color=green>¡CORRECTO!</color> Es un punto crítico válido. La plataforma se mantiene firme.");
            }
            else
            {
                Debug.Log("<color=red>¡INCORRECTO!</color> No es un máximo/mínimo. La plataforma colapsa.");
                DropPlatform();
            }
        }
    }

    private void DropPlatform()
    {
        if (platformRigidbody != null)
        {
            // Desactiva el modo cinemático y activa la gravedad para que el asteroide caiga
            platformRigidbody.isKinematic = false;
            platformRigidbody.useGravity = true;
        }
    }
}