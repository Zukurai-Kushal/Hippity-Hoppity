using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIScript : MonoBehaviour
{
    public Transform [] shopItems;
    [SerializeField] private ItemManager itemManager;
    public UnityEngine.UI.Text MelonCounterText;
    private void Awake()
    {
        foreach(Transform shopItem in shopItems)
        {
            shopItem.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        showItems();
    }

    public void showItems()
    {
        foreach (int itemIndex in itemManager.unlockedItems)
        {
            shopItems[itemIndex].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MelonCounterText.text = ": "+itemManager.melonCounter.ToString();
    }

    // private void CreateItemButton(Sprite itemSprite, string itemDescription, int itemCost, int positionIndex)
    // {
    //     Transform shopItemTransform = Instantiate(shopItemTemplate, shopContainer);
    //     RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();
        
    //     shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemGap * positionIndex);
    //     shopItemTemplate.Find("itemImage").GetComponent<UnityEngine.UI.Image>().sprite = itemSprite;
    //     shopItemTransform.Find("itemDescription").GetComponent<UnityEngine.UI.Text>().text = itemDescription;
    //     shopItemTransform.Find("itemCost").GetComponent<UnityEngine.UI.Text>().text = itemCost.ToString();
    // }
}
