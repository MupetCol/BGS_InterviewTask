using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class ShopHandler : MonoBehaviour
{
    [Header("Main")]

    [SerializeField] private Transform _shopContent;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private ToggleGroup _toggleGroup;
    [SerializeField] private InventoryHandler _playerInventory;
    [SerializeField] private FloatReference _playerCoins;
    [SerializeField] private TMP_Text _coinsText;

    [SerializeField] private List<Item> _items = new List<Item>();

	#region UNITY_METHODS

	private void Awake()
	{
        UpdateItems();
    }

    #endregion

    #region PUBLIC_METHODS

    public void Add(Item item)
	{
        // For when the player sells to the shop
        _items.Add(item);
        UpdateItems();
    }

    public void Remove(Item item)
	{
        // For when something is sold
        _items.Remove(item);
        UpdateItems();
    }
    public void Buy()
    {

        Toggle toggleSelected = _toggleGroup.ActiveToggles().FirstOrDefault();
        ItemInstance itemSelected = toggleSelected.GetComponent<ItemInstance>();


        if (_playerCoins.value >= itemSelected._item.cost)
        {
            Remove(itemSelected._item);
            _playerInventory.Add(itemSelected._item);
            _playerCoins.value -= itemSelected._item.cost;
            _coinsText.text = "X " + _playerCoins.value.ToString();
        }
    }

    #endregion

    #region PRIVATE_METHODS

    public void UpdateItems()
	{
        // Clear all existing items on the hierarchy
		foreach (Transform item in _shopContent)
		{
            Destroy(item.gameObject);
		}

		foreach (Item _it in _items)
		{
            GameObject obj = Instantiate(_itemPrefab, _shopContent);
            obj.GetComponent<Toggle>().group = _toggleGroup;
            obj.GetComponent<ItemInstance>()._item = _it;

            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemCost = obj.transform.Find("ItemCost").GetComponent<TMP_Text>();

            itemIcon.sprite = _it.icon;
            itemCost.text = _it.cost.ToString();
		}
	}

    #endregion

}
