using UnityEngine;

public class SandCastleController : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private UnityEngine.VFX.VisualEffect _deathEffect;







    private void OnCollisionEnter(Collision col) {
        if (col.gameObject.CompareTag("Gazer")) {
            _health--;
            if (_health <= 0) {
                LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(() => {
                    // _deathEffect.Play();
                    Destroy(gameObject);
                });
            }
        }
    }
}
