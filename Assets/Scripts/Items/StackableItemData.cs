using UnityEngine;
using UnityEngine.UI;

public class StackableItemData : MonoBehaviour
{
    public int StackSize;
    public int StackLimit;

    public bool LimitedStackSize = true;

    public Text StackNumber;

    private void Start ( )
    {
        if(!LimitedStackSize)
        {
            StackLimit = int.MaxValue;
        }
    }

    public void UpdateStack()
    {
        if (LimitedStackSize)
        {
            StackNumber.text = StackSize.ToString () + "/" + StackLimit.ToString ();
        }
        else
        {
            StackNumber.text = StackSize.ToString ();
        }
    }   
}
