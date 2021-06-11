using UnityEngine.Events;
using UnityEngine;

public class Activatable : MonoBehaviour
{
    //this class is meant for object that get controlled by the objects the player interacts with, not by the player directly
    #region Editor fields
    [SerializeField] private UnityEvent<bool> _activate = null; //event invoked when called
    #endregion

    #region Fields
    protected bool _isActivated;
    #endregion

    #region Methods
    public void Activate(bool isActivated)
    {
        _isActivated = isActivated;
        _activate.Invoke(_isActivated);
    }
    #endregion
}
