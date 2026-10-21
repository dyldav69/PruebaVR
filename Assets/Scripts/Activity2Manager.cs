
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity2Manager : MonoBehaviour
{
    [Header("Jugador VR")]
    public Transform xrOrigin;
    public CharacterController characterController;
    public Transform puntoInicio;
    public Transform camaraJugador;

    [Header("Detección de caídas")]
    public float alturaLimiteCaida = -2f;

    [Header("Respuesta final")]
    public GameObject quadRespuesta;
    public int objetosNecesarios = 3;

    [Header("Sonido opcional")]
    public AudioSource audioSource;
    public AudioClip sonidoRecoleccion;

    [Header("Diagnóstico")]
    [SerializeField] private int objetosRecogidos;
    [SerializeField] private bool actividadCompletada;
    [SerializeField] private bool reiniciando;
    [SerializeField] private float alturaActualPies;

    private readonly HashSet<GameObject> objetosRegistrados =
        new HashSet<GameObject>();

    public bool Reiniciando => reiniciando;
    public bool ActividadCompletada => actividadCompletada;

    private void Start()
    {
        if (xrOrigin == null || puntoInicio == null)
        {
            Debug.LogError(
                "[Actividad 2] Asigna XR Origin y Punto Inicio.", this);
            enabled = false;
            return;
        }

        if (characterController == null)
        {
            characterController =
                xrOrigin.GetComponent<CharacterController>();

            if (characterController == null)
                characterController =
                    xrOrigin.GetComponentInChildren<CharacterController>();
        }

        if (camaraJugador == null)
        {
            Camera camara = xrOrigin.GetComponentInChildren<Camera>();

            if (camara != null)
                camaraJugador = camara.transform;
        }

        if (characterController == null || camaraJugador == null)
        {
            Debug.LogError(
                "[Actividad 2] Falta asignar el CharacterController o la cámara.",
                this);
            enabled = false;
            return;
        }

        objetosNecesarios = Mathf.Max(1, objetosNecesarios);

        if (quadRespuesta != null)
            quadRespuesta.SetActive(false);

        Debug.Log("[Actividad 2] Actividad iniciada.");
    }


    private void Update()
    {
        // Durante el respawn no comprobar nuevas caídas.
        if (reiniciando)
            return;

        if (characterController == null ||
            !characterController.enabled)
            return;

        // La detección de caídas sigue activa incluso después
        // de recoger todos los objetos.
        alturaActualPies = characterController.bounds.min.y;

        if (alturaActualPies < alturaLimiteCaida)
        {
            Debug.LogWarning(
                $"[Actividad 2] Caída detectada. Altura: " +
                $"{alturaActualPies:F2}. Reiniciando.");

            VolverAlInicio();
        }
    }


    public bool EsJugador(Collider otro)
    {
        if (otro == null || characterController == null)
            return false;

        return otro == characterController ||
            otro.GetComponentInParent<CharacterController>()
                == characterController;
    }

    public void RecogerObjeto(GameObject objeto)
    {
        if (reiniciando || actividadCompletada || objeto == null)
            return;

        if (!objetosRegistrados.Add(objeto))
            return;

        objetosRecogidos++;

        if (audioSource != null && sonidoRecoleccion != null)
            audioSource.PlayOneShot(sonidoRecoleccion);

        Debug.Log(
            $"[Actividad 2] Objetos recogidos: " +
            $"{objetosRecogidos}/{objetosNecesarios}");

        if (objetosRecogidos >= objetosNecesarios)
            CompletarActividad();
    }

    private void CompletarActividad()
    {
        actividadCompletada = true;

        if (quadRespuesta != null)
            quadRespuesta.SetActive(true);
        else
            Debug.LogWarning(
                "[Actividad 2] Falta asignar el Quad de respuesta.");

        Debug.Log("[Actividad 2] ¡Actividad completada!");
    }

    public void VolverAlInicio()
    {
        if (reiniciando)
            return;

        StartCoroutine(ReiniciarActividad());
    }

    private IEnumerator ReiniciarActividad()
    {
        reiniciando = true;

        // Ocultar la respuesta, conservando los objetos recogidos.
        if (quadRespuesta != null)
            quadRespuesta.SetActive(false);

        // NO reactivar coleccionables.
        // NO borrar objetosRegistrados.
        // NO poner objetosRecogidos en cero.

        yield return new WaitForFixedUpdate();

        // Recuperar el método de respawn que ya funcionaba.
        Bounds limitesJugador = characterController.bounds;

        Vector3 posicionPies = new Vector3(
            limitesJugador.center.x,
            limitesJugador.min.y,
            limitesJugador.center.z);

        Vector3 desplazamiento =
            puntoInicio.position - posicionPies;

        characterController.enabled = false;

        xrOrigin.position += desplazamiento;

        Vector3 rotacionActual = xrOrigin.eulerAngles;
        Vector3 rotacionInicio = puntoInicio.eulerAngles;

        xrOrigin.rotation = Quaternion.Euler(
            rotacionActual.x,
            rotacionInicio.y,
            rotacionActual.z);

        Physics.SyncTransforms();

        characterController.enabled = true;

        yield return new WaitForFixedUpdate();

        Debug.Log(
            $"[Actividad 2] Punto objetivo: {puntoInicio.position}");

        Debug.Log(
            $"[Actividad 2] Pies después del respawn: " +
            $"{characterController.bounds.min}");

        reiniciando = false;

        Debug.Log("[Actividad 2] Respawn completado.");
    }
}
