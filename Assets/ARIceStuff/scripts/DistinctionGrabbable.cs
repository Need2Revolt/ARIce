using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DistinctionGrabbable : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.tag == "LeftHand")
        {
            //left hand ray will killl the enemy
            GetComponent<EnemyInteractor>().OnPointerClick(null);
        }
        else
        {
            //right hand will grab the object
            base.OnSelectEntering(args);
        }
    }
}
