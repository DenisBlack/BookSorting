using UnityEngine;

public class IPickUpAbleItem : MonoBehaviour
{
    public virtual bool PickUp()
    {
        Debug.Log("Picked Up");
        return true;
    }

    public virtual bool CheckPicked(out Temp type)
    {
        if (PickUp())
        {
            // TO DO 
            type = Temp.Something;
            return true;
        }

        type = Temp.None;
        return false;
    }
}

public enum Temp
{
    None = 0,
    Something = 1
}
