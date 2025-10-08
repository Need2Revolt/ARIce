using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class NearFarDistinctionInteractor : NearFarInteractor
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            Debug.Log("N2RG: NearFarInteractor OnSelectEntered");
            base.OnSelectEntered(args);
            Debug.Log("N2RG: args" + args.ToString());
            if (m_SelectedTargetCastSource == Region.Far)
            {
                Debug.Log("N2RG: Far Grabbed");
                args.interactableObject.transform.GetComponent<EnemyInteractor>().OnPointerClick(null);
                //m_NormalRelativeToInteractable = firstInteractableSelected.GetAttachTransform(this).InverseTransformDirection(m_RayEndNormal);
            }

            if (m_SelectedTargetCastSource == Region.Near)
            {
                Debug.Log("N2RG: Near Grabbed");
            }
        }
}
