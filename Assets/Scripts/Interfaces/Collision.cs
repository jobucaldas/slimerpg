using UnityEngine;

namespace GameInterfaces
{
    public class Collision
    {
        public struct CollisionInfo
        {
            public bool above, below;
            public bool left, right;

            public void Reset() {
                above = below = false;
                left = right = false;
            }
        }

        // Transform
        Transform transform;
        // Collisions
        public CollisionInfo collisions;
        private BoxCollider2D collisionBody;
        private LayerMask collisionMask;
        // Raycast
        private RaycastController raycaster;
        private float horizontalRaySpacing;
        private float verticalRaySpacing;

        public Collision(RaycastController raycaster, BoxCollider2D collisionBody)
        {
            // Sets collider
            this.collisionBody = collisionBody;
            this.raycaster     = raycaster;

            // Sets transform 
            transform = collisionBody.gameObject.GetComponent<Transform>();
        }

        public void HorizontalCollisions(ref Vector3 movePoint) 
        {
            float distance = movePoint.x - transform.position.x;
            float directionX = Mathf.Sign(distance);
            float rayLength = Mathf.Abs(distance) + raycaster.skinWidth;

            for (int i = 0; i < raycaster.horizontalRayCount; i ++) {
                Vector2 rayOrigin = (directionX == -1) ? raycaster.raycastOrigins.bottomLeft :
                raycaster.raycastOrigins.bottomRight;
                
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX,
                rayLength, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

                if (hit) {
                    Debug.Log("Horizontal hit");
                    rayLength = hit.distance;

                    collisions.left = (directionX == -1);
                    collisions.right = (directionX == 1);

                    Bounds bounds = collisionBody.bounds;
                    float hitPoint = hit.point.x - (raycaster.skinWidth + bounds.extents.x) * directionX;

                    if (directionX == 1)
                    {
                        if (hitPoint < movePoint.x)
                        {
                            movePoint.x =  hitPoint;
                        }
                    }
                    else if (directionX == -1)
                    {
                        if (hitPoint > movePoint.x)
                        {
                            movePoint.x = hitPoint;
                        }
                    }
                }
            }
        }

        public void VerticalCollisions(ref Vector3 movePoint)
        {
            float distanceX = movePoint.x - transform.position.x;
            float directionX = Mathf.Sign(distanceX);
            float distanceY = movePoint.y - transform.position.y;
            float directionY = Mathf.Sign(distanceY);
            float rayLength = Mathf.Abs(distanceY) + raycaster.skinWidth;

            for (int i = 0; i < raycaster.verticalRayCount; i ++) {
                Vector2 rayOrigin = (directionY == -1) ? raycaster.raycastOrigins.bottomLeft : 
                    raycaster.raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY,
                    rayLength, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.white);

                if (hit) {
                    Debug.Log("Vertical hit");
                    rayLength = hit.distance;

                    collisions.below = (directionY == -1);
                    collisions.above = (directionY == 1);

                    Bounds bounds = collisionBody.bounds;
                    float hitPoint = hit.point.y - (raycaster.skinWidth + bounds.extents.y) * directionY;

                    if (directionY == 1)
                    {
                        if (hitPoint < movePoint.y)
                        {
                            movePoint.y =  hitPoint;
                        }
                    }
                    else if (directionY == -1)
                    {
                        if (hitPoint > movePoint.y)
                        {
                            movePoint.y =  hitPoint;
                        }
                    }
                }
            }
        }
    }
}