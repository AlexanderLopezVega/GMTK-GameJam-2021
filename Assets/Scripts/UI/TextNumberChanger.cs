using TMPro;
using UnityEngine;

public class TextNumberChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text = default;

    public void SetNumber(int number)
    {
        text.text = number.ToString();
    }

    public void DecreaseNumber(int amount)
    {
        IncreaseNumber(-amount);
    }

    public void IncreaseNumber(int amount)
    {
        int num = int.Parse(text.text);
        text.text = (num + amount).ToString();
    }
}
