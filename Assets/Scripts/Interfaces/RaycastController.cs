using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameInterfaces
{
    public class RaycastController : MonoBehaviour
    {
        // Collision
        private BoxCollider2D objectCollider;
        private LayerMask collisionMask; // Mask used only inside
        public float skinWidth           { get; private set; } = .015f;

        // Raycast
        public int horizontalRayCount    { get; private set; } = 4;
        public int verticalRayCount      { get; private set; } = 4;
        private float horizontalRaySpacing;
        private float verticalRaySpacing;

        // Raycast settings
        public struct RaycastOrigins {
            public Vector2 topLeft, topRight;
            public Vector2 bottomLeft, bottomRight;
        }
        public RaycastOrigins raycastOrigins;

        // Transform
        Transform transform;

        public RaycastController(BoxCollider2D objectCollider, LayerMask collisionMask, float horizontalRaySpacing, float verticalRaySpacing)
        {
            this.objectCollider       = objectCollider; // gameObject.GetComponent<BoxCollider2D>();
            this.collisionMask        = collisionMask;
            this.horizontalRaySpacing = horizontalRaySpacing;
            this.verticalRaySpacing   = verticalRaySpacing;
            transform = objectCollider.GetComponent<Transform>();
            CalculateRaySpacing();
        }
        
        public void UpdateRaycastOrigins()
        {
            Bounds bounds = objectCollider.bounds;
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
            Bounds bounds = objectCollider.bounds;
            bounds.Expand(skinWidth * -2);

            horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
            verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }
    }
}
