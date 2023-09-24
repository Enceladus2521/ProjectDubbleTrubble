using UnityEngine;

public class EyeAnimator : MonoBehaviour
{
    
    private GameObject _focusedPlayer;
    private float _angle;



    private void Awake() {
        GazerController.OnChangeFocusedPlayer += (GameObject player) => _focusedPlayer = player;
        _angle = GetComponent<Gizmo>()._angle;
    }


    private void Update() {

        if (_focusedPlayer == null) return;
        
        Vector3 direction = transform.position - _focusedPlayer.transform.position;
        //look at player direction plus the given angle in degrees
        //dont lerp, just set rotation
        transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, _angle, 0);
    }
}
