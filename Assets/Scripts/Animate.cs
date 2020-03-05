using UnityEngine;

public class Animate : MonoBehaviour {
    private Animator animate;

    public Animate(ref Animator animator)
    {
        animate = animator;
    }

    public void Move(bool moving)
    {
        if(moving)
        {
            animate.SetBool("stop", false);
            animate.SetBool("move", true);
        }
        else
        {
            animate.SetBool("move", false);
        }
    }

    public void Stop()
    {
        animate.SetBool("stop", true);
    }

    public void Die()
    {
        animate.SetBool("die", true);
    }
}