using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [Header("Configuración de Caída")]
    [Tooltip("Tiempo en segundos antes de caer al pisarla")]
    public float delayBeforeFall = 0.2f;

    [Tooltip("Distancia horizontal máxima (en metros) para detectar al jugador")]
    public float maxDistanceXZ = 3.0f;

    [Header("Diagnóstico en Tiempo Real")]
    [SerializeField] private float currentDistanceXZ;
    [SerializeField] private bool isPlayerOnTop;

    private Rigidbody rb;
    private Transform cameraTransform;
    private bool isFalling = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // En RV, Camera.main SIEMPRE representa la posición real de los ojos del jugador
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("[FallingPlatform] No se encontró 'Camera.main'. Asegúrate de que la cámara de RV tenga el Tag 'MainCamera'.");
        }
    }

    private void Update()
    {
        if (isFalling || cameraTransform == null) return;

        // 1. Convertir posiciones a 2D (Ejes X y Z) para ignorar diferencias de altura
        Vector2 playerXZ = new Vector2(cameraTransform.position.x, cameraTransform.position.z);
        Vector2 platformXZ = new Vector2(transform.position.x, transform.position.z);

        // 2. Calcular la distancia horizontal real
        currentDistanceXZ = Vector2.Distance(playerXZ, platformXZ);

        // 3. Verificar si el jugador está horizontalmente sobre la plataforma y por encima de ella en Y
        bool isAbove = cameraTransform.position.y >= (transform.position.y - 0.5f);
        isPlayerOnTop = (currentDistanceXZ <= maxDistanceXZ) && isAbove;

        if (isPlayerOnTop)
        {
            StartCoroutine(StartFallingSequence());
        }
    }

    private IEnumerator StartFallingSequence()
    {
        isFalling = true;
        Debug.Log($"<color=orange>[Plataforma]</color> ¡Jugador detectado sobre {gameObject.name}! Distancia XZ: {currentDistanceXZ:F2}m");

        yield return new WaitForSeconds(delayBeforeFall);

        if (rb != null)
        {
            // Liberar las restricciones de posición (X, Y, Z) para permitir la caída libre
            rb.constraints = RigidbodyConstraints.None;
            rb.isKinematic = false;
            rb.useGravity = true;

            Debug.Log($"<color=red>[Plataforma]</color> ¡{gameObject.name} COLAPSÓ!");
        }
        else
        {
            Debug.LogError($"[FallingPlatform ERROR] No se encontró Rigidbody en {gameObject.name}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja un cilindro visual en la escena para ver la zona de caída
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistanceXZ);
    }
}