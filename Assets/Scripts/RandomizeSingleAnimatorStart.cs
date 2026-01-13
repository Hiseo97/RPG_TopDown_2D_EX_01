using UnityEngine;

public class RandomizeSingleAnimatorStart : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string stateName = "Idle";
    [SerializeField, Range(0f, 1f)] private float randomRange = 1f;
    [SerializeField] private Vector2 speedRange = new Vector2(0.9f, 1.1f);

    private void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!animator) return;

        float t = Random.value * randomRange;
        float speed = Random.Range(speedRange.x, speedRange.y);

        animator.Update(0f);
        animator.speed = speed;
        animator.Play(stateName, 0, t);
        animator.Update(0f);
    }
}
