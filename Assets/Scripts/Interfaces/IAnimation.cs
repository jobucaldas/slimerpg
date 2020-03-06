using UnityEngine;

namespace GameInterfaces
{
    // Interface for all animation
    public interface IAnimation
    {
        // Basic movement
        void Move(bool moving);
        void Stop();

        // Killing animation (runs while animation not finished)
        void Die();
    }

    // Class containing player animation variables
    public class PlayerAnimation : IAnimation
    {
        // Object's animator
        private Animator animate;

        // Initializer
        public PlayerAnimation(Animator animator)
        {
            animate = animator;
        }

        // Moves while character is moving (no math)
        public void Move(bool moving)
        {
            if(moving)
            {
                // animate.SetBool("stop", false);
                animate.SetBool("move", true);
            }
            else
            {
                animate.SetBool("move", false);
            }
        }

        // Hard stops animation
        public void Stop()
        {
            animate.SetBool("move", false);
            // animate.SetBool("stop", true);
        }

        // Death animation (I believe this may be good to have even for props)
        public void Die()
        {
            // animate.SetBool("die", true);
        }
    }
}
