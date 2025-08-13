using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    public string itemName = "Box";

    public void OnInteract()
    {
        Debug.Log(itemName + " È¹µæ!");
        Destroy(gameObject);
    }
}
