using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    public LayerMask collisionMask;

    public const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    //[HideInInspector]
    protected float horizontalRaySpacing;
    //[HideInInspector]
    protected float verticalRaySpacing;

    //[HideInInspector]
    protected BoxCollider2D playerCollider;

    public RaycastOrigins raycastOrigins;

    public virtual void Awake()
    {
        playerCollider = gameObject.GetComponent<BoxCollider2D>();
        //transform = playerCollider.GetComponent<Transform>();
        CalculateRaySpacing();
    }

    public struct RaycastOrigins {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
    
    public void UpdateRaycastOrigins()
    {
        Bounds bounds = playerCollider.bounds;
        bounds.Expand(skinWidth * -2);
        
        raycastOrigins.bottomLeft = new Vector2(transform.position.x - bounds.extents.x,
        transform.position.y - bounds.extents.y);
        raycastOrigins.bottomRight = new Vector2(transform.position.x + bounds.extents.x,
        transform.position.y - bounds.extents.y);
        raycastOrigins.topLeft = new Vector2(transform.position.x - bounds.extents.x,
        transform.position.y + bounds.extents.y);
        raycastOrigins.topRight = new Vector2(transform.position.x + bounds.extents.x,
        transform.position.y + bounds.extents.y);
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = playerCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
}
