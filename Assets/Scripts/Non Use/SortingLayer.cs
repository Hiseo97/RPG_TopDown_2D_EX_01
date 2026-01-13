using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class SortingLayer : MonoBehaviour
{
    public SpriteRenderer sr;
    public int st_offset;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = st_offset + Mathf.RoundToInt(transform.position.y*-100);
    }

    void Update()
    {
        // sr.sortingOrder = st_offset + Mathf.RoundToInt(transform.position.y*-100);
    }
}
