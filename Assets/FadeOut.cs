using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    

    void Start()
    {
        LeanTween.delayedCall(10f, () =>
        {
            LeanTween.alpha(gameObject, 0f, 3f).setOnComplete(() =>
            {
                Destroy(gameObject);
            });
        });
    }

}
