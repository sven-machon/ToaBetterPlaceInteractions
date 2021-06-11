# ToaBetterPlaceInteractions
A short view into the interaction system I made, used in To a Better Place

The Interactable is the class used for every interactable object in the world. When selected by a player it can use the Unity Event system to invoke the linked behaviour for
the object.
The Activatables are all objects controlled by the Interactables, the player has no direct control over these objects.

In this case Interact() will invoke the TryActivate() in the RuneInteractable. All the Runess are linked to a RuneHandler, this RuneHandler handles the order in which 
the Runes need to be activated in order for it to work. The RuneHandler will send a message to the Activatable which will invoke the ElevatorInteraction(bool)
on the RuneElevator, resulting in the desired behaviour by the Elevator object.
