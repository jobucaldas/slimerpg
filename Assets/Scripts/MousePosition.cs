using UnityEngine;

public class  MousePosition : MonoBehaviour
{
    // Mouse Variables
    private Vector3 goal;
    private bool clickedOnce = false;

    private Vector3 Get()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            goal = hit.point;

            clickedOnce = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            clickedOnce = false;
        }

        // Vector3 newPos = new Vector3(transform.position.x, transform.position.y, goal.z - distanceFromBackground);

        // Vector2 mousePos2D = new Vector2(goal.x, goal.y);

        return goal;
    }
}