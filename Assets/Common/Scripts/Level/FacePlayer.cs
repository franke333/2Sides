using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class FacePlayer : MonoBehaviour
{
    private void Start()
    {
        //DOTween to oscilate up and down
        transform.DOLocalMoveY(0.2f, 2.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }
    // Update is called once per frame
    void Update()
    {
        //rotate to face the Camera.main
        Vector3 target = Camera.main.transform.position;
        target.y = transform.position.y;
        transform.LookAt(target);
    }
}
