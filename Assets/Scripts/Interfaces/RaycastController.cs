using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameInterfaces
{
    public class RaycastController : MonoBehaviour
    {
        private LayerMask collisionMask;

        public const float skinWidth = .015f;
        public int horizontalRayCount = 4;
        public int verticalRayCount = 4;

        //[HideInInspector]
        private float horizontalRaySpacing;
        //[HideInInspector]
        private float verticalRaySpacing;

        //[HideInInspector]
        private BoxCollider2D playerCollider;

        public RaycastOrigins raycastOrigins;

        public float GetSkinWidth()
        {
            return skinWidth;
        }

        public int GetHorizontalRayCount()
        {
            return horizontalRayCount;
        }

        public int GetVerticalRayCount()
        {
            return verticalRayCount;
        }

        public RaycastOrigins GetRaycastOrigins()
        {
            return raycastOrigins;
        }

        public RaycastController(ref LayerMask collisionMask, ref BoxCollider2D playerCollider, float horizontalRaySpacing, float verticalRaySpacing)
        {
            this.playerCollider       = playerCollider; //gameObject.GetComponent<BoxCollider2D>();
            this.collisionMask        = collisionMask;
            this.horizontalRaySpacing = horizontalRaySpacing;
            this.verticalRaySpacing   = verticalRaySpacing;
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
}
