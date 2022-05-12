using UnityEngine;

namespace Code.Player
{
    public class GunAimView : MonoBehaviour
    {
        public LayerMask Mask;

        public void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 30, Mask))
            {
                hit.transform.GetComponent<IAimable>()?.Aim();
            }
        }
    }
}