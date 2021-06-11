using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneHandler : MonoBehaviour
{
    #region Editor fields
    [SerializeField] private List<Rune> _runesLinked = null; //the runes that control the object
    [SerializeField] private Activatable _linkedObject = null; //object to control
    [SerializeField] private string _correctSound = "CorrectRune";
    [SerializeField] private string _wrongSound = "BadRune";
    #endregion

    #region Fields
    private AudioManager _audioManager = null;
    private int _currentRuneIdx=0;
    #endregion

    #region Methods
    private void Start()
    {
        foreach (Rune currRune in _runesLinked)
        {
            currRune.SetHandler(this);
            currRune.Deactivate();
        }

        _audioManager = FindObjectOfType<AudioManager>();
        if (!_audioManager) Debug.LogError("NO AUDIO MANAGER FOUND");
    }

    public void Activate(Rune rune,bool activated)
    {
        if (activated) //if the rune was already activated, break elevator and reset all
        {
            foreach(Rune currRune in _runesLinked) //deactivate all runes
            {
                currRune.Deactivate();
            }
            _currentRuneIdx = 0;
            _audioManager.Play(_wrongSound);

            // let linked object know Runes broke
            if (_linkedObject)
                _linkedObject.Activate(false);
        }
        else
        {
            if (_runesLinked.IndexOf(rune) == _currentRuneIdx) // if rune is next in list, succesful activation
            {
                _audioManager.Play(_correctSound);
                rune.Activate();
                _currentRuneIdx++;

                // let linked object know succesful activation
                if (_linkedObject)
                    _linkedObject.Activate(true);

            }
            else
            {
                rune.Activate();
                _audioManager.Play(_wrongSound);
                _currentRuneIdx++;
                StartCoroutine(Deactivate()); //break the elevator after a short delay
            }
        }

    }

  IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (Rune currRune in _runesLinked) //deactivate all runes
        {
            currRune.Deactivate();
        }
        _currentRuneIdx = 0;

        // let linked object know Runes broke
        if (_linkedObject)
            _linkedObject.Activate(false);
    }



    private void OnEnable()
    {
        foreach (Rune currRune in _runesLinked)
        {
            currRune.GetComponent<Interactable>().enabled = true;
        }
    }
    #endregion
}
