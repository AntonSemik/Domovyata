using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
    [SerializeField] Sprite[] _sprites;

    [SerializeField] SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer.sprite = RandomSprite();
    }

    private void OnDisable()
    {
        _renderer.sprite = RandomSprite();
    }

    Sprite RandomSprite()
    {
        return _sprites[Random.Range(0, _sprites.Length)];
    }
}
