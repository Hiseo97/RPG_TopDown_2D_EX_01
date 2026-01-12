using UnityEngine;
using UnityEngine.Tilemaps;
public class TilemapYSort : MonoBehaviour
{
    public int stOffset = 0;
    TilemapRenderer tr;

    void Awake()
    {
        tr = GetComponent<TilemapRenderer>();
        tr.sortingOrder = stOffset + Mathf.RoundToInt(transform.position.y * -100f);
    }

}