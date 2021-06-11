using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    #region Fields
    private RuneHandler _runeSystem=null;
    private bool _isActive=false;
    private MeshRenderer _renderer;
    #endregion

    #region Methods
    private void Awake()
    {
        //if there is no renderer attached to the rune, check children (this is for the broken rune that first needs to be fixed)
        if (!TryGetComponent<MeshRenderer>(out _renderer)) 
        {
            _renderer = GetComponentInChildren<MeshRenderer>();
        }

        _renderer.material.color = Color.red;
    }


    public void TryActivate() //this function gets called by the interactable invoke
    {
        _runeSystem.Activate(this,_isActive);
    }

    public void Activate() //the rune handler will call this to change the rune state
    {
        if(_renderer)
            _renderer.material.color = Color.blue;
        _isActive = true;

    }


    public void Deactivate() //the rune handler will call this to change the rune state
    {
        if (_renderer)
            _renderer.material.color = Color.red;
        _isActive = false;
    }

    public void SetHandler(RuneHandler handler)
    {
        _runeSystem = handler;
    }
    #endregion
}
