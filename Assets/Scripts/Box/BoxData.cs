using UnityEngine;

public class BoxData : MonoBehaviour
{
    [System.Serializable]
    public enum BoxColor
    {
        Red,
        Blue,
        Green,
        Purple,
        Orange,
        Yellow
    }

    [SerializeField] public BoxColor Color = default;
}
