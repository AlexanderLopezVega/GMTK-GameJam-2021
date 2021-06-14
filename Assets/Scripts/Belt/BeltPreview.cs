using UnityEngine;

public class BeltPreview : MonoBehaviour
{
    private const string PreviewGameObjectName = "Preview Belt";

    [SerializeField] private Material previewMaterial = default;
    [SerializeField] private BeltSpawner beltSpawner = default;
    [SerializeField] private BeltSelector beltSelector = default;
    [Header("Belt SO")]
    [SerializeField] private BeltSO straightBeltSO = default;
    [SerializeField] private BeltSO turnLeftBeltSO = default;
    [SerializeField] private BeltSO turnRightBeltSO = default;
    [SerializeField] private BeltSO upBeltSO = default;
    [SerializeField] private BeltSO downBeltSO = default;
    [Header("Other")]
    [SerializeField] public bool previewEnabled = default;

    public BeltSO CurrentPreviewBelt { get; private set; } = default;
    public GameObject PreviewGameObject { get; private set; } = default;

    public void SetPreviewEnabled(bool value)
    {
        previewEnabled = value;

        Destroy(PreviewGameObject);
        CurrentPreviewBelt = null;
    }

    public void CompareCurrentPreviewStraight() => CompareCurrentPreview(straightBeltSO);
    public void CompareCurrentPreviewTurnLeft() => CompareCurrentPreview(turnLeftBeltSO);
    public void CompareCurrentPreviewTurnRight() => CompareCurrentPreview(turnRightBeltSO);
    public void CompareCurrentPreviewUp() => CompareCurrentPreview(upBeltSO);
    public void CompareCurrentPreviewDown() => CompareCurrentPreview(downBeltSO);

    public void CompareCurrentPreview(BeltSO beltSO)
    {
        if (!previewEnabled || beltSelector.Selected == null)
            return;

        if (beltSO != CurrentPreviewBelt)
        {
            if (PreviewGameObject != null)
                Destroy(PreviewGameObject);

            if (beltSelector.SelectedBeltConnection.nextConnectedBelt != null)
                return;

            CurrentPreviewBelt = beltSO;

            LocRot locRot = beltSelector.SelectedBeltConnection.NextLocRot;

            PreviewGameObject = Instantiate(CurrentPreviewBelt.Prefab, locRot.location, locRot.rotation, null);
            PreviewGameObject.name = PreviewGameObjectName;
            PreviewGameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial = previewMaterial;
        }
        else
        {
            // Spawn current preview belt
            if (beltSpawner.SpawnBelt(CurrentPreviewBelt))
            {
                if (beltSelector.SelectedBeltConnection != null && beltSelector.SelectedBeltConnection.nextConnectedBelt == null)
                {
                    PreviewGameObject.transform.position = beltSelector.SelectedBeltConnection.NextLocRot.location;
                    PreviewGameObject.transform.rotation = beltSelector.SelectedBeltConnection.NextLocRot.rotation;
                }
                else
                {
                    Destroy(PreviewGameObject);
                }
            }
        }
    }

    public void OnSelectionChanged(GameObject newSelected, BeltConnection beltConnection)
    {
        if (beltConnection == null)
        {
            Destroy(PreviewGameObject);

            CurrentPreviewBelt = null;
        }
        else if(beltConnection.nextConnectedBelt != null)
        {
            Destroy(PreviewGameObject);

            CurrentPreviewBelt = null;
        }
        else if(PreviewGameObject != null)
        {
            PreviewGameObject.transform.position = beltConnection.NextLocRot.location;
            PreviewGameObject.transform.rotation = beltConnection.NextLocRot.rotation;
        }
    }

    public void OnEndOfLine(BeltConnection reciever)
    {
        Destroy(PreviewGameObject);
        CurrentPreviewBelt = null;
    }
}
