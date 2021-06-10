using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneElevator : MonoBehaviour
{
    [SerializeField] private List<Transform> _levelList = new List<Transform>();
    private int _currentLevel=0;
    [SerializeField] private float _lerpSpeed = 3;
    [SerializeField] private string _soundName = "WoodenLift";
    private AudioManager _audioManager = null;


    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }



    public void ElevatorUp(bool isActivated)
    {
        if (isActivated)
        {
            _currentLevel++;

        }
        else
        {
           _currentLevel=0;
        }

        _audioManager.Play(_soundName);
    }


    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _levelList[_currentLevel].position) > 0.5f) 
        {
            //move lift towards current level
            transform.position = Vector3.Slerp(transform.position, _levelList[_currentLevel].position, _lerpSpeed * Time.deltaTime);
        }
        else
        {
                _audioManager.StopPlaying(_soundName);
        }
    }


}
