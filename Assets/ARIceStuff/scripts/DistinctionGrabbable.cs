using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DistinctionGrabbable : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if(args.interactorObject.transform.tag == "Hand")
        {
            GetComponent<EnemyInteractor>().OnPointerClick(null);
        }
    }
}
