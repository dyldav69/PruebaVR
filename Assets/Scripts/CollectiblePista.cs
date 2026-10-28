
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MostrarPistaAlAgarrar : MonoBehaviour
{
    public GameObject cartelPistas;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable objetoAgarrable;

    private void Awake()
    {
        objetoAgarrable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (cartelPistas != null)
            cartelPistas.SetActive(false);
    }

    private void OnEnable()
    {
        if (objetoAgarrable != null)
            objetoAgarrable.selectEntered.AddListener(MostrarCartel);
    }

    private void OnDisable()
    {
        if (objetoAgarrable != null)
            objetoAgarrable.selectEntered.RemoveListener(MostrarCartel);
    }

    private void MostrarCartel(SelectEnterEventArgs args)
    {
        if (cartelPistas != null)
            cartelPistas.SetActive(true);
    }
}
