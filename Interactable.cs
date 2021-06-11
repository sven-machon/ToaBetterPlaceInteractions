using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    enum ParentAnimation
    {
        nothing = 0,
        pushButton = 1,
        carryObject = 2
    }

    #region Editor fields
    [SerializeField] private int _requiredPlayers = 1;
    [SerializeField] private UnityEvent<GameObject> _interactEvent = null; //event that will handle the behaviour of the interactable
    [SerializeField] private ParentAnimation _animState = ParentAnimation.nothing;
    [SerializeField] private bool _isElderFriendly = true;
    #endregion

    #region Fields
    protected bool _isSelected;
    private bool _isLocked;
    private List<GameObject> _playersSelectedByList = new List<GameObject>();
    private MeshRenderer _meshRenderer = new MeshRenderer();
    #endregion

    #region properties
    public bool IsSelected() { return _isSelected; }
    public bool IsElderlyFriendly() { return _isElderFriendly; }
    public bool IsLocked() { return _isLocked; }

    public void SetEvent(UnityEvent<GameObject> newEvent) {
        _interactEvent = newEvent;
        _animState = ParentAnimation.pushButton;
    }
    #endregion //properties

    #region Methods
    protected void Start()
    {
        TryGetComponent<MeshRenderer> (out _meshRenderer);
    }

    public int Interact(GameObject player)
    {
        _isLocked = !_isLocked;
        _interactEvent.Invoke(player);

        return (int)_animState; // let the player know what animation state is required
    }

    public void Select(GameObject player)
    {
        if (_isSelected && _requiredPlayers == _playersSelectedByList.Count) //can't be selected if it has been selected by max players
            return;

        if (!_playersSelectedByList.Contains(player))
        {
            _playersSelectedByList.Add(player);
            if (_playersSelectedByList.Count == _requiredPlayers)
                _isSelected = true;
        }
    }

    public void Deselect(GameObject player)
    {
        if (!_playersSelectedByList.Contains(player)) //cant be deselected by just any player
            return;

        _playersSelectedByList.Remove(player);
        _isSelected = false;
    }
    #endregion
}
