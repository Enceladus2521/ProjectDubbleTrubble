using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [Header("Phase Settings")]
    [SerializeField] private float _timeBeforePhase1 = 1f;
    [SerializeField] private float _phase1Duration = 1f;
    [SerializeField] private float _gazerSizePhase1 = 1f;
    [SerializeField] private float _timeBeforePhase2 = 1f;
    [SerializeField] private float _phase2Duration = 1f;
    [SerializeField] private float _gazerSizePhase2 = 1f;
    [SerializeField] private GameObject _gazerPrefab;

    [Header("UI Settings")]
    [SerializeField] private GameObject _gameOverScreen;



    public static GameManager instance = null;
    private float _startTime;
    private bool _spawnedGazer1 = false;
    private bool _spawnedGazer2 = false;
    private bool _gameOver = false;


    private void Awake()
    {
        instance = this;
        _startTime = Time.time;
    }


    private void Update() {
        if (Time.time - _startTime > _timeBeforePhase1 && Time.time - _startTime < _timeBeforePhase1 + _phase1Duration && !_spawnedGazer1) {
            //phase 1
            Debug.Log("Start phase 1");
            GameObject obj = Instantiate(_gazerPrefab, new Vector3(0, 0.001f, 0), Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.transform.localScale = new Vector3(_gazerSizePhase1, 0, _gazerSizePhase1);
            _spawnedGazer1 = true;
        }
        else if (Time.time - _startTime > _timeBeforePhase1 + _phase1Duration + _timeBeforePhase2 && Time.time - _startTime < _timeBeforePhase1 + _phase1Duration + _timeBeforePhase2 + _phase2Duration && !_spawnedGazer2) {
            //phase 2
            Debug.Log("Start phase 2");
            GameObject obj = Instantiate(_gazerPrefab, new Vector3(0, 0.001f, 0), Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.transform.localScale = new Vector3(_gazerSizePhase2, 0, _gazerSizePhase2);
            _spawnedGazer2 = true;
        }
        else if (Time.time - _startTime > _timeBeforePhase1 + _phase1Duration + _timeBeforePhase2 + _phase2Duration && !_gameOver) {
            //game over
            _gameOver = true;
            //GameOver();
        }
    }

    public void GameOver() {
        Time.timeScale = 0;
        _gameOverScreen.SetActive(true);
    }


    public void Restart() {
        Time.timeScale = 1;
        _gameOverScreen.SetActive(false);
        //get active scene
        int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
    }

}
