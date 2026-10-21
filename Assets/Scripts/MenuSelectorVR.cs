
using UnityEngine;

public class MenuSelectorVR : MonoBehaviour
{
    [Header("Jugador")]
    public Transform xrOrigin;

    [Header("Puntos de inicio")]
    public Transform puntoActividad1;
    public Transform puntoActividad2;
    public Transform puntoActividad3;

    [Header("Menú")]
    public GameObject menuPrincipal;

    public void IniciarActividad1()
    {
        IrAActividad(puntoActividad1);
    }

    public void IniciarActividad2()
    {
        IrAActividad(puntoActividad2);
    }

    public void IniciarActividad3()
    {
        IrAActividad(puntoActividad3);
    }

    private void IrAActividad(Transform puntoInicio)
    {
        if (xrOrigin == null)
        {
            Debug.LogError(
                "[Menu VR] Falta asignar XR Origin.");
            return;
        }

        if (puntoInicio == null)
        {
            Debug.LogError(
                "[Menu VR] Falta asignar el punto de inicio.");
            return;
        }

        // Trasladar al jugador al punto seleccionado.
        xrOrigin.SetPositionAndRotation(
            puntoInicio.position,
            puntoInicio.rotation
        );

        // Ocultar el menú después de elegir.
        if (menuPrincipal != null)
        {
            menuPrincipal.SetActive(false);
        }

        Debug.Log(
            "[Menu VR] Actividad seleccionada. " +
            "Jugador trasladado a: " + puntoInicio.name
        );
    }
}
