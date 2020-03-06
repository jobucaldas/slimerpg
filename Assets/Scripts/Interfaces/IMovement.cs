using UnityEngine;

namespace GameInterfaces
{
    // Movement interface
    public interface IMovement
    {
        /* Variables */
        // Objects
        IAnimation animate           { get; set; }           // Animation
        // Collision
        Collision collision          { get; set; }           // Collisions
        BoxCollider2D collisionBody  { get; set; }
        
        // Movement settings
        float distanceFromBackground { get; }           // Distance from BG
        Vector3 movePoint            { get; }      // Point where to move to
        float movementSpeed          { get; set; } // Speed | Velocity

        /* Functions */
        // Moves parent object
        bool MoveTo(Vector3 movePoint);            // Returns if moving or not
    }

    // Movements from mouse position
    public class MouseMovement : IMovement
    {
        // Movement
        public float distanceFromBackground { get; }
        public float movementSpeed          { get; set; }
        public Vector3 movePoint            { get; private set; }

        // Objects
        // Transform
        public Transform transform;
        // Animation
        public IAnimation animate           { get; set; }
        // Collision
        public Collision collision          { get; set; }
        public BoxCollider2D collisionBody  { get; set; }
        // Raycast
        private RaycastController raycaster;

        // Initializer
        public MouseMovement(BoxCollider2D collisionBody, LayerMask collisionMask, // Collision 
                             IAnimation animate,                                   // Animation 
                             float distanceFromBackground, float movementSpeed,    // Movement settings
                             float horizontalRaySpacing, float verticalRaySpacing) // Ray settings
        {
            // Set animation
            this.animate                 = animate;                // Not created here so that it can be used from player

            // Set transform
            transform = collisionBody.gameObject.GetComponent<Transform>();

            // Set movement settings
            this.distanceFromBackground  = distanceFromBackground; // Distance from BG
            this.movementSpeed           = movementSpeed;          // Speed | Velocity

            // Start objects
            // Sets raycaster
            raycaster = new RaycastController(collisionBody, collisionMask,              // Collision settings
                                              horizontalRaySpacing, verticalRaySpacing); // Ray settings
            // Sets collision
            collision = new Collision(raycaster, collisionBody);
        }

        // Move to point
        public bool MoveTo(Vector3 movePoint)
        {
            Debug.Log($"Mouse: { movePoint }");
            Vector3 mouseCurrent = movePoint;

            // Sets raycast to current
            raycaster.UpdateRaycastOrigins();
            collision.collisions.Reset();
            
            // Hold move point
            Vector3 moveInto = movePoint;

            // Horizontal collide
            if (moveInto.x != transform.position.x) {
                collision.HorizontalCollisions(ref moveInto); // Changed inside
            }
            // Vertical collide
            if (moveInto.y != transform.position.y) {
                collision.VerticalCollisions(ref moveInto);   // Changed inside
            }

            // XY velocity
            // Almost the same as player.Translate(velocity);
            transform.position = Vector3.MoveTowards(transform.position, moveInto, movementSpeed); // Actual movement
            movePoint          = moveInto; // Move holder gets position
            Debug.Log($"MovePoint: { movePoint }");
            Debug.Log($"MoveInto:  { moveInto }");

            // Z velocity (separate so movement is still 2D)
            moveInto           = new Vector3(transform.position.x, transform.position.y, // Current player XY
                                             movePoint.z - distanceFromBackground);      // Z move point minus distance from BG
            transform.position = Vector3.MoveTowards(transform.position, moveInto, movementSpeed); // Actual movement

            bool moving = ((mouseCurrent - new Vector3(0, 0, distanceFromBackground)) != transform.position);
            if(moving){ Debug.Log($"Moving: { moving }"); }
            animate.Move(moving);

            return moving; // Returns if moving or not
        }
    }
}