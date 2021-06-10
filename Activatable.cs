using UnityEngine.Events;
using UnityEngine;

public class Activatable : MonoBehaviour
{
    //this class is meant for object that get controlled by the objects the player interacts with, not by the player directly
    protected bool _isActivated;
    [SerializeField] private UnityEvent<bool> _activate = null; //event invoked when called

    public void Activate(bool isActivated)
    {
        _isActivated = isActivated;

        _activate.Invoke(_isActivated);

    }
}
