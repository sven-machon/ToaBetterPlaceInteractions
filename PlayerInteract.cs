using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    #region Editor fields
    [SerializeField] private LayerMask _layersToRaycast = 0;
    #endregion

    #region Fields
    private List<GameObject> _interactableList = new List<GameObject>();
    private GameObject _interactableTargetObject = null;
    private Interactable _interactableTarget = null;
    private bool _isElder = false;
    private bool _isInteractableLocked = false;
    #endregion

    #region Methods
    private void Start()
    {
        _isElder = GetComponent<CharacterController>().IsElder(); //not all characters can interact with everything
    }

    private void Update()
    {
        if (!_isInteractableLocked) //if player is currently locked into an interaction, ignore
        {
           GetClosestObject();
        }           
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable interact = other.gameObject.GetComponent<Interactable>();
        if (other.gameObject.CompareTag("Interactable") && interact.isActiveAndEnabled) //only check active interactables
        {
            if (_interactableList.Contains(other.gameObject))
                return;
            if (_isInteractableLocked)
                return;
            if (!interact.IsElderlyFriendly() && _isElder) //if player is not compatibale with interactable, ignore
                return;

            _interactableList.Add(other.gameObject);
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (_interactableList.Contains(other.gameObject))
        {
            _interactableList.Remove(other.gameObject);

            if (!_isInteractableLocked) //release the interactable if it was locked
            {
                if (_interactableTargetObject == other.gameObject)
                {
                    _interactableTarget.Deselect(gameObject);
                    _interactableTarget = null;
                    _interactableTargetObject = null;
                }
            }
        }
    }


    private void GetClosestObject()
    {
        
        if (_isInteractableLocked && _interactableTargetObject) //return target if player is locked
            return;
   
        if (_interactableList.Count == 0)  //exit if no possible objects are found
            return;

        GameObject currentTarget = null;
        float minDis = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        Vector3 dirToTarget;

        for (int i = 0; i < _interactableList.Count; i++)
        {
            if (_interactableList[i] == null) continue; 

            Interactable interactable = _interactableList[i].GetComponent<Interactable>();
            if (interactable == null) continue;

            dirToTarget = _interactableList[i].transform.position - currentPos;
            float distToTarget = dirToTarget.sqrMagnitude;

            if (distToTarget < minDis) //compare square magnitudes with current selection
            {
                minDis = distToTarget;
                currentTarget = _interactableList[i];              
            }
        }

        if (currentTarget != _interactableTargetObject)
        {
            if(_interactableTargetObject!=null)
                _interactableTarget.Deselect(gameObject);
        _interactableTargetObject = currentTarget;
        _interactableTarget = _interactableTargetObject.GetComponent<Interactable>(); 
        _interactableTarget.Select(gameObject); //let object know it got selected by a player
        }
        
    }

    public void LockInteractable(GameObject interactable)
    {
        _isInteractableLocked = true;
        _interactableTargetObject = interactable;
    }

    public void UnlockInteractable()
    {
        _isInteractableLocked = false;
    }

    public int Interact()
    {
        if (_interactableTarget)
            return _interactableTarget.Interact(gameObject); //return the animation state linked to the interaction
        return 0; //idle
    }

    public void RemoveObjectFromSight(GameObject interactable)
    {
        _interactableList.Remove(interactable);
    }

    public bool IsInteractableLocked()
    {
        return _isInteractableLocked;
    }
    #endregion
}
