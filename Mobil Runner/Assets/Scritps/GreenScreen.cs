using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class GreenScreen : MonoBehaviour
{
    GameManager gameManager;
    private void Awake()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Yoyo);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            DOTween.Kill(transform);
            gameManager.GreenScreenCount++;
            Destroy(gameObject);
        }
    }
    private void OnDisable()
    {
        DOTween.Kill(transform);
    }
}

