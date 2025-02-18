using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        Invoke("JouerAnimation", 2f);
    }

    void JouerAnimation()
    {
        animator.Play("Take 001");
        Destroy(gameObject,3f);
    }
}