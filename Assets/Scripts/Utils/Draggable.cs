using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public bool showPointer = true;
    private bool isDragging = false;
    private Vector3 offset = Vector3.zero;
    private float zCoord;
    private DragAction dragAction;

    private void Awake()
    {
        dragAction = GetComponent<DragAction>();
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDragging)
        {
            gameObject.transform.position = GetMouseWorldPos() + offset;
            dragAction.OnDrag();
        }
    }

    private void OnMouseDown()
    {
        if (dragAction.CanDrag)
        {
            isDragging = true;
            // ��¼�����z������
            zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            // ���������λ�����������ĵ�ƫ��
            if (showPointer)
            {
                offset = gameObject.transform.position - GetMouseWorldPos();
            }
            else
            {
                offset = Vector3.zero;
            }
            dragAction.OnStartDrag();
        }
    }

    private void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            dragAction.OnEndDrag();
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}