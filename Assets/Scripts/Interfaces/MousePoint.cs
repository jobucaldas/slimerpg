using UnityEngine;

namespace GameInterfaces
{
    public class  MousePoint
    {
        // Mouse Variables
        private Vector3 mousePos;

        public MousePoint(Vector3 initialPos)
        {
            mousePos = initialPos;
        }

        public Vector3 Get()
        {
            Vector3 mousePosition;
            string colliderTag;

            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, Mathf.Infinity);
                mousePosition = hit.point;

                // So that only the background is clickable
                if(hit.collider.tag.Equals("Background"))
                {
                    mousePos = mousePosition;
                }
            }

            return mousePos;
        }
    }
}