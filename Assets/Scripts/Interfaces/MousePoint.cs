using UnityEngine;

namespace GameInterfaces
{
    public class  MousePoint
    {
        // Mouse Variables
        private Vector3 mousePos;
        private LayerMask clickable;

        public MousePoint(Vector3 initialPos, LayerMask layerToClick)
        {
            // Set clickable layer
            clickable = layerToClick;

            // Start at asked position (to not move on startup)
            mousePos = new Vector3(initialPos.x, initialPos.y, 0);
        }

        public Vector3 Get()
        {
            if (Input.GetMouseButton(0))
            {
                // Ray setting
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Raycast itself
                Physics.Raycast(ray, out hit, Mathf.Infinity);


                Debug.Log($"Tag: {LayerMask.LayerToName(clickable)}"); // To se if clickable is actually bg
                Debug.Log($"Hit: {hit.collider.gameObject.layer}");
                // So that only the background is clickable
                if(hit.collider.gameObject.layer == clickable.value)   // (hit.collider.tag == "Background")
                {
                    mousePos = hit.point;
                }
            }

            return mousePos;
        }
    }
}