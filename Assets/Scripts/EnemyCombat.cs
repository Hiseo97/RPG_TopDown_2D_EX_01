using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public int damage = 2;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHealth health))
        {
            health.ChangeHealth(-damage);
        }
    }

}
