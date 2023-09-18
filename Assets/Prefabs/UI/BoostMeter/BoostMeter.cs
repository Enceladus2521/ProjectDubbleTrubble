using UnityEngine;

public class BoostMeter : MonoBehaviour
{
    

    private UnityEngine.UI.Image _image;


    private void Awake()
    {
        _image = GetComponent<UnityEngine.UI.Image>();
    }


    public void SetBoost(float boost) {
        _image.fillAmount = boost;
    }
}
