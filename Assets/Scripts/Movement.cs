using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : RaycastController
{
    // Variables
    // I don't know what this does
    public CollisionInfo collisions;

    // Variables used all over the code
    private Animate animate;
    private Vector3 goal;
    private float distanceFromBackground;
    private float movementSpeed;

    public Movement(ref Animate animate, float distanceFromBackground = 20F, float movementSpeed = 0.1F)
    {
        this.animate                 = animate;
        this.distanceFromBackground  = distanceFromBackground;
        this.movementSpeed           = movementSpeed;

        base.Awake();
    }

    private bool MoveTransform(Vector3 goal)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        
        Vector3 movePoint = goal;

        if (movePoint.x != transform.position.x) {
            HorizontalCollisions(ref movePoint, ref goal);
        }

        if (movePoint.y != transform.position.y) {
            VerticalCollisions(ref movePoint, ref goal);
        }

        // XY velocity
        // Almost the same as player.Translate(velocity);
        transform.position = Vector3.MoveTowards(transform.position, movePoint, movementSpeed);
        goal = movePoint;

        // Z velocity
        movePoint = new Vector3(transform.position.x, transform.position.y, goal.z - distanceFromBackground);
        transform.position = Vector3.MoveTowards(transform.position, movePoint, movementSpeed);

        return (transform.position != movePoint);
    }

    public void HorizontalCollisions(ref Vector3 movePoint, ref Vector3 goal) 
    {
        float distance = goal.x - transform.position.x;
        float directionX = Mathf.Sign(distance);
		float rayLength = Mathf.Abs(distance) + skinWidth;

		for (int i = 0; i < horizontalRayCount; i ++) {
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft :
            raycastOrigins.bottomRight;
			
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX,
            rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			if (hit) {
                Debug.Log("Horizontal hit");
                rayLength = hit.distance;

                collisions.left = (directionX == -1);
                collisions.right = (directionX == 1);

                Bounds bounds = playerCollider.bounds;
                float hitPoint = hit.point.x - (skinWidth + bounds.extents.x) * directionX;

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
		float rayLength = Mathf.Abs(distanceY) + skinWidth;

		for (int i = 0; i < verticalRayCount; i ++) {
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : 
                raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY,
                rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.white);

			if (hit) {
                Debug.Log("Vertical hit");
                rayLength = hit.distance;

                collisions.below = (directionY == -1);
                collisions.above = (directionY == 1);

                Bounds bounds = playerCollider.bounds;
                float hitPoint = hit.point.y - (skinWidth + bounds.extents.y) * directionY;

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
