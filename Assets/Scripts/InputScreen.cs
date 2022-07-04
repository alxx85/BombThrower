using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputScreen : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _sensitivity;

    private Vector2 _touchPosition;
    
    public event UnityAction<float, float> ChangedPosition;
    public event UnityAction<bool> ChangedTouch;

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 newPosition = eventData.position;
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 deltaPosition = (newPosition - _touchPosition) / screenSize * _sensitivity;
        ChangedPosition?.Invoke(deltaPosition.x, deltaPosition.y);
        _touchPosition = newPosition;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        _touchPosition = eventData.position;
        ChangedTouch?.Invoke(true);
        OnDrag(eventData);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        ChangedTouch?.Invoke(false);
        _touchPosition = Vector2.zero;
    }
}
