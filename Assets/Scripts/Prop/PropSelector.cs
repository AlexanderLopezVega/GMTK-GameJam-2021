using UnityEngine;

public class PropSelector : MonoBehaviour
{
    private const float DefaultMaxSelectionDistance = 10f;

    [SerializeField] private Camera activeCamera = default;
    [SerializeField] private float maxSelectionDistance = DefaultMaxSelectionDistance;
    [SerializeField] private LayerMask selectionMask = default;
    [SerializeField] private Material selectionMaterial = default;

    private GameObject m_Selected = default;
    private MeshRenderer m_SelectedRenderer = default;
    private PropConnection m_SelectedPropConnection = default;
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

            m_Selected = value;

            // On selected changed post
            {
                m_SelectedRenderer = value.GetComponentInChildren<MeshRenderer>();

                if (m_SelectedRenderer != null)
                {
                    m_PreviousMaterial = m_SelectedRenderer.sharedMaterial;
                    m_SelectedRenderer.sharedMaterial = selectionMaterial;
                }

                m_SelectedPropConnection = value.GetComponent<PropConnection>();
            }
        }
    }

    public PropConnection SelectedPropConnection
    {
        get => m_SelectedPropConnection;
        set
        {
            m_SelectedPropConnection = value;
            Selected = m_SelectedPropConnection.gameObject;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = activeCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, maxSelectionDistance, selectionMask))
            {
                Selected = hitInfo.transform.gameObject;
            }
        }
    }
}
