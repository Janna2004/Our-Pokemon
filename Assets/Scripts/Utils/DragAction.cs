using System.Collections;
using UnityEngine;

public abstract class DragAction : MonoBehaviour
{
    public abstract void OnStartDrag();

    public abstract void OnEndDrag();

    public abstract void OnDrag();

    public virtual bool CanDrag
    {
        get
        {
            return true;
        }
    }

    protected abstract bool DragSuccess();
}