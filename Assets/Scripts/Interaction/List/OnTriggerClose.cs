using UnityEngine;


namespace Interaction
{

    public class OnTriggerClose : MonoBehaviour
    {
        string checkItemName = "ף�������ص��ż�";
        private void OnCollisionEnter(Collision collision)
        {
            if(Package.PackageSimple.Instance.CheckItemByName(checkItemName))
            {
                GameObject.Destroy(GetComponent<Collider>());
            }
        }

    }
}