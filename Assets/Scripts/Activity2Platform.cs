
using System.Collections;
using UnityEngine;

public class Activity2Platform : MonoBehaviour
{
    [Header("Referencias")]
    public Activity2Manager administrador;
    public CharacterController jugador;
    public Collider colliderPlataforma;

    [Header("Detección de aterrizaje")]
    [Tooltip("Margen bajo la superficie para tolerar pequeños solapamientos.")]
    public float toleranciaInferior = 0.25f;

    [Tooltip("Altura máxima sobre la superficie para detectar al jugador.")]
    public float toleranciaSuperior = 0.50f;

    [Header("Diagnóstico")]
    [SerializeField] private float alturaPies;
    [SerializeField] private float alturaSuperficie;
    [SerializeField] private bool jugadorSobrePlataforma;

    private bool yaActivada;

    private void Start()
    {
        if (administrador == null)
        {
            administrador =
                FindFirstObjectByType<Activity2Manager>();
        }

        if (colliderPlataforma == null)
        {
            colliderPlataforma = GetComponent<Collider>();
        }

        if (jugador == null && administrador != null)
        {
            jugador = administrador.characterController;
        }

        if (administrador == null ||
            jugador == null ||
            colliderPlataforma == null)
        {
            Debug.LogError(
                $"[Actividad 2] Referencias incompletas en {name}.",
                this
            );

            enabled = false;
        }
    }

    private void Update()
    {
        if (yaActivada ||
            administrador == null ||
            administrador.Reiniciando ||
            administrador.ActividadCompletada ||
            jugador == null ||
            !jugador.enabled ||
            colliderPlataforma == null)
        {
            return;
        }

        Bounds plataforma = colliderPlataforma.bounds;
        Bounds cuerpo = jugador.bounds;

        // Comprobar que el volumen del jugador coincide
        // horizontalmente con el área de la plataforma.
        bool coincideX =
            cuerpo.min.x <= plataforma.max.x &&
            cuerpo.max.x >= plataforma.min.x;

        bool coincideZ =
            cuerpo.min.z <= plataforma.max.z &&
            cuerpo.max.z >= plataforma.min.z;

        // Comparar los pies del jugador con la superficie.
        alturaPies = cuerpo.min.y;
        alturaSuperficie = plataforma.max.y;

        bool alturaValida =
            alturaPies >= alturaSuperficie - toleranciaInferior &&
            alturaPies <= alturaSuperficie + toleranciaSuperior;

        jugadorSobrePlataforma =
            coincideX && coincideZ && alturaValida;

        if (jugadorSobrePlataforma)
        {
            yaActivada = true;

            Debug.Log(
                $"[Actividad 2] Plataforma incorrecta activada: {name}"
            );

            administrador.VolverAlInicio();

            StartCoroutine(RestaurarDeteccion());
        }
    }

    private IEnumerator RestaurarDeteccion()
    {
        // Evitar una nueva activación durante el reinicio.
        yield return new WaitUntil(
            () => administrador == null ||
                  !administrador.Reiniciando
        );

        // Dar tiempo a que el jugador abandone la plataforma.
        yield return new WaitForSeconds(0.5f);

        yaActivada = false;
    }
}
