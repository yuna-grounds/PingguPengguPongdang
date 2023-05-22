using Oculus.Interaction;
using Oculus.Interaction.Surfaces;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(ColliderSurface))]
[RequireComponent(typeof(RayInteractable))]
[RequireComponent(typeof(InteractableUnityEventWrapper))]
public class AddInteractable : MonoBehaviour
{
    BoxCollider collider;
    ColliderSurface surface;
    RayInteractable interactable;
    InteractableUnityEventWrapper eventWrapper;

  
    private void Awake()
    {
        
        collider = GetComponent<BoxCollider>();
        surface = GetComponent<ColliderSurface>();
        interactable = GetComponent<RayInteractable>();
        eventWrapper = GetComponent<InteractableUnityEventWrapper>();
        collider.enabled = true;
        surface.enabled = true;
        interactable.enabled = true;
        eventWrapper.enabled = true;

        surface.InjectCollider(collider);
        interactable.InjectSurface(surface);
        eventWrapper.InjectInteractableView(interactable);
    }
    

}
