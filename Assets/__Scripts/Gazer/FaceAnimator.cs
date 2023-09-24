using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAnimator : MonoBehaviour
{
    

    [SerializeField] private FaceAnim _angryFace;
    [SerializeField] private FaceAnim _obsessedFace;
    [SerializeField] private float _timeBetweenFrames = 0.5f;


    private SpriteRenderer _spriteRenderer;
    public FaceAnim _currentFaceAnim;
    private List<GameObject> _faceAnims = new List<GameObject>();


    private void Start() {
        _currentFaceAnim = _obsessedFace;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        //loop through all children of current parent
        //add children to _faceAnims list
        foreach (Transform child in transform.parent) {
            _faceAnims.Add(child.gameObject);
        }

        StartCoroutine(changeFace());
    }


    private IEnumerator changeFace() {
        while (true) {
            for (int i = 0; i < _currentFaceAnim.faces.Count; i++) {
                _spriteRenderer.sprite = _currentFaceAnim.faces[i];
                foreach (GameObject faceAnim in _faceAnims) {
                    if (faceAnim.name != _currentFaceAnim.eyeObject.name && faceAnim.name != "FaceLooks") {
                        faceAnim.SetActive(false);
                    }
                    else {
                        faceAnim.SetActive(true);
                    }
                }
                yield return new WaitForSeconds(_timeBetweenFrames);
            }
            yield return null;
        }
    }
}