using UnityEngine;

namespace GameInterfaces
{
    public class  MousePoint
    {
        // Mouse Variables
        private bool clickedOnce  = false;
        private Vector3 mousePos;

        public MousePoint(Vector3 initialPos)
        {
            mousePos = initialPos;
        }

        public Vector3 Get()
        {
            Vector3 mousePoint = mousePos;

            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, Mathf.Infinity);
                mousePoint = hit.point;
                mousePos = hit.point;

                clickedOnce = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                clickedOnce = false;
            }

            /*
             * Vector3 newPos = new Vector3(transform.position.x, transform.position.y, goal.z - distanceFromBackground);
             * Vector2 mousePos2D = new Vector2(goal.x, goal.y);
            */

            return mousePoint;
        }
    }
}