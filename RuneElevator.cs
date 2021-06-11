using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneElevator : MonoBehaviour
{
    #region Editor fields
    [SerializeField] private List<Transform> _levelList = new List<Transform>(); //different levels for the lift to be at
    [SerializeField] private float _lerpSpeed = 3;
    [SerializeField] private string _soundName = "WoodenLift";
    #endregion

    #region Fields
    private AudioManager _audioManager = null;
    private int _currentLevel=0;
    #endregion

    #region Methods
    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    public void ElevatorInteraction(bool succes)
    {
        if (succes)
        {
            _currentLevel++; //increase lift level
        }
        else
        {
           _currentLevel=0; //drop lift to ground
        }

        _audioManager.Play(_soundName);
    }


    void FixedUpdate()
    {
        if (( _levelList[_currentLevel].position- transform.position ).sqrMagnitude > 0.5f) 
        {
            //move lift towards current level
            transform.position = Vector3.Slerp(transform.position, _levelList[_currentLevel].position, _lerpSpeed * Time.deltaTime);
        }
        else
        {
                _audioManager.StopPlaying(_soundName);
        }
    }
    #endregion
}
