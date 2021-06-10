using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private GameObject _interactableTarget = null;
    [SerializeField] private List<GameObject> _interactableList = null;
    [SerializeField] private LayerMask _layersToRaycast = 0;
    [SerializeField] private bool _isInteractableLocked = false;
    private bool _isElder = false;

    private void Start()
    {
        _isElder = GetComponent<CharacterController>().IsElder(); //not all characters can interact with everything
    }

    private void Update()
    {
        if (!_isInteractableLocked) //if player is currently locked into an interaction, ignore
            _interactableTarget = GetClosestObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable") && other.gameObject.GetComponent<Interactable>().isActiveAndEnabled) //only check active interactables
        {
            if (_isInteractableLocked)
                return;
            if (!other.GetComponent<Interactable>().IsElderlyFriendly() && _isElder) //if player is not compatibale with interactable, ignore
                return;

            _interactableList.Add(other.gameObject);
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (_interactableList.Contains(other.gameObject))
        {
            _interactableList.Remove(other.gameObject);

            if (_interactableTarget == other.gameObject)
            {
                if (!_isInteractableLocked) //release the interactable if it was locked
                {
                    _interactableTarget.GetComponent<Interactable>().Deselect(gameObject);
                    _interactableTarget = null;
                }
            }
            

        }
    }


    private GameObject GetClosestObject()
    {
        
        if (_isInteractableLocked && _interactableTarget) //return target if player is locked
            return _interactableTarget;
   
        if (_interactableList.Count == 0)  //exit if no possible objects are found
            return null;

        GameObject currentTarget = null;
        float minDis = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        Vector3 dirToTarget;


        for (int i = 0; i < _interactableList.Count; i++)
        {
            if (_interactableList[i] == null) continue; 

            Interactable interactable = _interactableList[i].GetComponent<Interactable>();
            if (interactable == null) continue;

            //reset all objects' states (unless selected by other player)
            interactable.Deselect(gameObject);

            dirToTarget = _interactableList[i].transform.position - currentPos;
            float distToTarget = dirToTarget.sqrMagnitude;

            if (distToTarget < minDis) //compare square magnitudes with current selection
            {
                if (!interactable.IsSelected())
                {                   
                    minDis = distToTarget;
                    if(interactable.GetComponent<Interactable>())
                        currentTarget = _interactableList[i];
                }
            }
        }

        if (currentTarget)
            currentTarget.GetComponent<Interactable>().Select(gameObject); //let object know it got selected by a player

        return currentTarget;
    }

    public void LockInteractable(GameObject interactable)
    {
        _isInteractableLocked = true;
        _interactableTarget = interactable;
    }

    public void UnlockInteractable()
    {
        _isInteractableLocked = false;
    }

    public int Interact()
    {
        if (_interactableTarget)
            return _interactableTarget.GetComponent<Interactable>().Interact(gameObject); //return the animation state linked to the interaction
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
}
