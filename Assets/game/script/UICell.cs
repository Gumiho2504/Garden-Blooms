using UnityEngine;
using UnityEngine.UI;

public class UICell : MonoBehaviour
{
    public Flower flower; // Reference to the flower object
    private int row, column;

    // Reference to the button component
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick); // Add listener to the button click event
    }

    public void SetCoordinates(int r, int c)
    {
        row = r;
        column = c;
    }

    public bool IsEmpty()
    {
        return flower == null;
    }

    public void PlantFlower(Flower newFlower)
    {
        if (IsEmpty())
        {
            flower = newFlower;
            // Change button image to represent the flower
           // GetComponent<Image>().sprite = newFlower.GetComponent<Image>().sprite;
            flower.transform.SetParent(transform,false); // Set cell as parent for the flower
        }
    }

    public void RemoveFlower()
    {
        if (flower != null)
        {
            Destroy(flower.gameObject);
            flower = null;
            // Reset button image to empty state
            //GetComponent<Image>().sprite = null;
        }
    }

    private void OnClick()
    {
        GardenManager.Instance.OnCellClicked(this); // Notify GardenManager on click
    }

    public int GetRow() { return row; }
    public int GetColumn() { return column; }
}
