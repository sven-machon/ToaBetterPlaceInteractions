using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneHandler : MonoBehaviour
{
    [SerializeField] private List<InteractableRune> _runesLinked = null; //the runes that control the object
    [SerializeField] private int _currentLevel=0;
    [SerializeField] private Activatable _linkedObject = null; //object to control
    private AudioManager _audioManager = null;
    [SerializeField] private string _correctSound = "CorrectRune";
    [SerializeField] private string _wrongSound = "BadRune";


    private void Start()
    {
        foreach (InteractableRune currRune in _runesLinked)
        {
            currRune.SetHandler(this);
            currRune.Deactivate();
        }


        _audioManager = FindObjectOfType<AudioManager>();
    }

    public void Activate(InteractableRune rune,bool activated)
    {
        if (activated) //if the rune was already activated, break elevator and reset all
        {
            foreach(InteractableRune currRune in _runesLinked) //deactivate all runes
            {
                currRune.Deactivate();
            }
            _currentLevel = 0;
            _audioManager.Play(_wrongSound);

            // let linked object know Runes broke
            if (_linkedObject)
                _linkedObject.Activate(false);
        }
        else
        {
            if (_runesLinked.IndexOf(rune) == _currentLevel) // if rune is next in list, succesful activation
            {
                _audioManager.Play(_correctSound);
                rune.Activate();
                _currentLevel++;

                // let linked object know succesful activation
                if (_linkedObject)
                    _linkedObject.Activate(true);

            }
            else
            {
                rune.Activate();
                _audioManager.Play(_wrongSound);
                _currentLevel++;
                StartCoroutine(Deactivate()); //break the elevator after a short delay
            }
        }

    }

  IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (InteractableRune currRune in _runesLinked) //deactivate all runes
        {
            currRune.Deactivate();
        }
        _currentLevel = 0;

        // let linked object know Runes broke
        if (_linkedObject)
            _linkedObject.Activate(false);
    }



    private void OnEnable()
    {
        foreach (InteractableRune currRune in _runesLinked)
        {
            currRune.GetComponent<Interactable>().enabled = true;
        }
    }
}
