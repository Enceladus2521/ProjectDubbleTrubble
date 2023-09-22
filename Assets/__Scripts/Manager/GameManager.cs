using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [Header("Phase Settings")]
    [SerializeField] private PhaseSetup _phaseSetup;

    [Header("UI Settings")]
    [SerializeField] private GameObject _gameOverScreen;



    public static GameManager instance = null;
    public static event Action<Phase> OnPhaseChange;
    private float _startTime;
    private float _currentTimeSince;
    private List<Phase> _phases;
    private Phase _currentPhase;
    private bool _phaseRunning = false;


    private void Awake()
    {
        instance = this;
        _startTime = Time.time;
        _phases = _phaseSetup._phases;
        if (_phases.Count > 0) {
            _currentPhase = _phases[0];
        }
        else {
            Debug.LogError("No phases in phase setup");
        }
    }


    private void Update() {
        _currentTimeSince = Time.time - _startTime;

        if (_currentTimeSince > _currentPhase._timeBefore && _currentTimeSince < _currentPhase._timeBefore + _currentPhase._duration) {
            if (!_phaseRunning) {
                OnPhaseChange?.Invoke(_currentPhase);
            }
        }
        else if (_currentTimeSince > _currentPhase._timeBefore + _currentPhase._duration + _currentPhase._timeAfter) {
            _startTime = Time.time;
            if (_phases.IndexOf(_currentPhase) < _phases.Count - 1) {
                _currentPhase = _phases[_phases.IndexOf(_currentPhase) + 1];
            }
            else {
                Debug.Log("No more phases");
            }
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
