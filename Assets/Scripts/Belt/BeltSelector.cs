using System;
using UnityEngine;
using UnityEngine.Events;

public class BeltSelector : MonoBehaviour
{
    private const float DefaultMaxSelectionDistance = 10f;

    [SerializeField] private Camera activeCamera = default;
    [SerializeField] private float maxSelectionDistance = DefaultMaxSelectionDistance;
    [SerializeField] private LayerMask selectionMask = default;
    [SerializeField] private Material selectionMaterial = default;
    [SerializeField] private UnityEvent<GameObject, BeltConnection> onSelectedChanged = default;
    [SerializeField] private bool selectionEnabled = default;

    private GameObject m_Selected = default;
    private MeshRenderer m_SelectedRenderer = default;
    private BeltConnection m_SelectedBeltConnection = default;
    private Material m_PreviousMaterial = default;

    public GameObject Selected
    {
        get => m_Selected;
        set
        {
            // On selected changed pre
            {
                if (m_SelectedRenderer != null)
                {
                    m_SelectedRenderer.sharedMaterial = m_PreviousMaterial;
                }
            }

            if (!selectionEnabled)
                return;

            m_Selected = value;

            // On selected changed post
            {
                m_SelectedRenderer = (value == null) ? null : value.GetComponentInChildren<MeshRenderer>();

                if (m_SelectedRenderer != null)
                {
                    m_PreviousMaterial = m_SelectedRenderer.sharedMaterial;
                    m_SelectedRenderer.sharedMaterial = selectionMaterial;
                }

                m_SelectedBeltConnection = (value == null) ? null : value.GetComponent<BeltConnection>();

                onSelectedChanged.Invoke(m_Selected, SelectedBeltConnection);
            }
        }
    }

    public BeltConnection SelectedBeltConnection
    {
        get => m_SelectedBeltConnection;
        set
        {
            m_SelectedBeltConnection = value;
            Selected = m_SelectedBeltConnection.gameObject;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = activeCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, maxSelectionDistance, selectionMask))
            {
                if (hitInfo.transform.gameObject.GetComponent<BeltConnection>().Selectable)
                {
                    Selected = hitInfo.transform.gameObject;
                }
            }
        }
    }

    public void SetSelectedNext(BeltConnection newConnection)
    {
        SelectedBeltConnection.nextConnectedBelt = newConnection;
        newConnection.previousConnectedBelt = SelectedBeltConnection;
    }

    public void SetSelectionEnabled(bool value)
    {
        selectionEnabled = value;

        if (!value)
            Selected = null;
    }

    public void OnEndOfLine(BeltConnection reciever)
    {
        SetSelectedNext(reciever);

        Selected = null;
    }
}
