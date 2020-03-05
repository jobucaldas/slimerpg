using UnityEngine;

namespace GameInterfaces
{
    // Movement interface
    public interface IMovement<T>
    {
        bool MoveTo(Vector3 movePoint);
    }

    public class MouseMovement : MonoBehaviour, IMovement<MouseMovement>
    {
        // I don't know what this does
        public CollisionInfo collisions;

        // Variables used all over the code
        private PlayerAnimation animate;
        private Vector3 movePoint;
        private RaycastController raycaster;
        private float distanceFromBackground;
        private float movementSpeed;

        private LayerMask collisionMask;
        //[HideInInspector]
        private float horizontalRaySpacing;
        //[HideInInspector]
        private float verticalRaySpacing;

        private BoxCollider2D collisionBody;

        public MouseMovement(ref BoxCollider2D collisionBody, ref PlayerAnimation animate, float distanceFromBackground, float movementSpeed, float horizontalRaySpacing, float verticalRaySpacing)
        {
            this.animate                 = animate;
            this.distanceFromBackground  = distanceFromBackground;
            this.movementSpeed           = movementSpeed;
            this.collisionBody           = collisionBody;

            raycaster = new RaycastController(ref collisionMask, ref collisionBody, horizontalRaySpacing, verticalRaySpacing);
        }

        public bool MoveTo(Vector3 movePoint)
        {
            raycaster.UpdateRaycastOrigins();
            collisions.Reset();
            
            Vector3 moveInto = movePoint;

            if (moveInto.x != transform.position.x) {
                HorizontalCollisions(ref moveInto, ref movePoint);
            }

            if (moveInto.y != transform.position.y) {
                VerticalCollisions(ref moveInto, ref movePoint);
            }

            // XY velocity
            // Almost the same as player.Translate(velocity);
            transform.position = Vector3.MoveTowards(transform.position, moveInto, movementSpeed);
            movePoint = moveInto;

            // Z velocity
            moveInto = new Vector3(transform.position.x, transform.position.y, movePoint.z - distanceFromBackground);
            transform.position = Vector3.MoveTowards(transform.position, moveInto, movementSpeed);

            return (transform.position != moveInto);
        }

        public void HorizontalCollisions(ref Vector3 moveInto, ref Vector3 movePoint) 
        {
            float distance = movePoint.x - transform.position.x;
            float directionX = Mathf.Sign(distance);
            float rayLength = Mathf.Abs(distance) + raycaster.GetSkinWidth();

            for (int i = 0; i < raycaster.GetHorizontalRayCount(); i ++) {
                Vector2 rayOrigin = (directionX == -1) ? raycaster.GetRaycastOrigins().bottomLeft :
                raycaster.GetRaycastOrigins().bottomRight;
                
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
                    float hitPoint = hit.point.x - (raycaster.GetSkinWidth() + bounds.extents.x) * directionX;

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

        public void VerticalCollisions(ref Vector3 movePoint, ref Vector3 goal)
        {
            float distanceX = goal.x - transform.position.x;
            float directionX = Mathf.Sign(distanceX);
            float distanceY = goal.y - transform.position.y;
            float directionY = Mathf.Sign(distanceY);
            float rayLength = Mathf.Abs(distanceY) + raycaster.GetSkinWidth();

            for (int i = 0; i < raycaster.GetVerticalRayCount(); i ++) {
                Vector2 rayOrigin = (directionY == -1) ? raycaster.GetRaycastOrigins().bottomLeft : 
                    raycaster.GetRaycastOrigins().topLeft;
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
                    float hitPoint = hit.point.y - (raycaster.GetSkinWidth() + bounds.extents.y) * directionY;

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

        public struct CollisionInfo
        {
            public bool above, below;
            public bool left, right;

            public void Reset() {
                above = below = false;
                left = right = false;
            }
        }
    }
}