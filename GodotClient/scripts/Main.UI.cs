using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.Combat;
using Game.Core.Progression;
using Godot;

namespace GameGodot;

public partial class Main
{
	private void RefreshHubView()
	{
		var roster = _progress.HeroRoster ?? Array.Empty<HeroProgressInfo>();
		var activeCount = roster.Count(hero => hero.IsActive);
		_hubStatsLabel.Text = $"Gold: {_progress.Gold} | {activeCount} active hero{(activeCount == 1 ? string.Empty : "es")}";
		RefreshHeroSelectionGrid(roster);
		RefreshShopLists();
	}

	private void RefreshHeroSelectionGrid(HeroProgressInfo[] roster)
	{
		ClearChildren(_heroSelectionGrid);
		for (var i = 0; i < roster.Length; i++)
		{
			var hero = roster[i];
			var card = CreateHeroCard(hero, i);
			_heroSelectionGrid.AddChild(card);
		}
	}

	private Control CreateHeroCard(HeroProgressInfo hero, int index)
	{
		var card = (Control)_heroCardPacked.Instantiate();
		var portrait = card.GetNode<TextureRect>("Content/Portrait");
		if (_heroPortraits.TryGetValue(hero.Specialization, out var portraitTexture))
		{
			portrait.Texture = portraitTexture;
		}

		card.GetNode<Label>("Content/Details/NameLabel").Text = hero.Name;
		card.GetNode<Label>("Content/Details/RoleLabel").Text = hero.Specialization.ToString();
		card.GetNode<Label>("Content/Details/LevelLabel").Text = $"Level {hero.Level}";
		card.GetNode<ProgressBar>("Content/Details/HpBar").Value = Math.Min(100, hero.Level * 10);
		card.GetNode<ProgressBar>("Content/Details/ManaBar").Value = Math.Min(100, 50 + hero.Level * 5);

		var activeToggle = card.GetNode<CheckBox>("Content/Details/ActiveCheck");
		activeToggle.SetPressedNoSignal(hero.IsActive);
		activeToggle.Toggled += value => SetHeroActive(index, value);

		return card;
	}

	private void SetHeroActive(int index, bool isActive)
	{
		var roster = (_progress.HeroRoster ?? Array.Empty<HeroProgressInfo>()).ToArray();
		if (index < 0 || index >= roster.Length)
		{
			return;
		}

		roster[index] = roster[index] with { IsActive = isActive };
		_progress = _progress with { HeroRoster = roster };
		_storage.Save(_progress);
		RefreshHubView();
	}

	private void RefreshShopLists()
	{
		EnsureShopStocked();
		PopulateItemColumn(_inventoryList, _progress.Inventory ?? Array.Empty<InventoryItemSnapshot>());
		PopulateItemColumn(_shopList, _progress.Shop ?? Array.Empty<InventoryItemSnapshot>());
	}

	private void EnsureShopStocked()
	{
		if (_progress.Shop is { Length: > 0 })
		{
			return;
		}

		var stock = new[]
		{
			new InventoryItemSnapshot
			{
				Name = "Lesser Healing Potion",
				Effects = new Dictionary<Characteristic, int> { [Characteristic.Health] = 15 }
			},
			new InventoryItemSnapshot
			{
				Name = "Mana Draught",
				Effects = new Dictionary<Characteristic, int> { [Characteristic.Mana] = 12 }
			},
			new InventoryItemSnapshot
			{
				Name = "Sharpening Stone",
				Effects = new Dictionary<Characteristic, int> { [Characteristic.PhysicalDamage] = 3 },
				BuffName = "Might"
			}
		};

		var updatedGold = Math.Max(_progress.Gold, 25);
		_progress = _progress with { Shop = stock, Gold = updatedGold };
		_storage.Save(_progress);
	}

	private static int CalculatePrice(InventoryItemSnapshot item)
	{
		var effectsScore = item.Effects?.Values.Sum(Math.Abs) ?? 0;
		var buffScore = string.IsNullOrWhiteSpace(item.BuffName) ? 0 : 10;
		return Math.Max(5, effectsScore + buffScore);
	}

	private void BuyBestAffordableItem()
	{
		var shop = _progress.Shop ?? Array.Empty<InventoryItemSnapshot>();
		if (shop.Length == 0)
		{
			PushLog("Shop is empty.");
			return;
		}

		var priced = shop.Select((item, index) => (item, index, price: CalculatePrice(item)))
			.OrderByDescending(entry => entry.price)
			.ToList();

		var pick = priced.First();
		if (_progress.Gold < pick.price)
		{
			PushLog($"Not enough gold. Need {pick.price}, have {_progress.Gold}.");
			return;
		}

		var inventory = (_progress.Inventory ?? Array.Empty<InventoryItemSnapshot>()).ToList();
		inventory.Add(pick.item);

		var shopList = shop.ToList();
		shopList.RemoveAt(pick.index);

		_progress = _progress with
		{
			Gold = _progress.Gold - pick.price,
			Inventory = inventory.ToArray(),
			Shop = shopList.ToArray()
		};
		_storage.Save(_progress);
		PushLog($"Bought {pick.item.Name} for {pick.price} gold.");
		RefreshShopLists();
		RefreshHubView();
	}

	private void SellCheapestItem()
	{
		var inventory = (_progress.Inventory ?? Array.Empty<InventoryItemSnapshot>()).ToList();
		if (inventory.Count == 0)
		{
			PushLog("Nothing to sell.");
			return;
		}

		var priced = inventory.Select((item, index) => (item, index, price: CalculatePrice(item)))
			.OrderBy(entry => entry.price)
			.ToList();

		var pick = priced.First();
		inventory.RemoveAt(pick.index);

		var shop = (_progress.Shop ?? Array.Empty<InventoryItemSnapshot>()).ToList();
		shop.Add(pick.item);

		_progress = _progress with
		{
			Gold = _progress.Gold + pick.price,
			Inventory = inventory.ToArray(),
			Shop = shop.ToArray()
		};
		_storage.Save(_progress);
		PushLog($"Sold {pick.item.Name} for {pick.price} gold.");
		RefreshShopLists();
		RefreshHubView();
	}

	private void PopulateItemColumn(VBoxContainer container, InventoryItemSnapshot[] entries)
	{
		ClearChildren(container);
		if (entries.Length == 0)
		{
			container.AddChild(new Label { Text = "None yet." });
			return;
		}

		foreach (var entry in entries)
		{
			container.AddChild(BuildInventoryEntry(entry));
		}
	}

	private Control BuildInventoryEntry(InventoryItemSnapshot entry)
	{
		var row = new HBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
		var icon = GetItemIcon(entry.Name);
		if (icon is not null)
		{
			var texture = new TextureRect
			{
				Texture = icon,
				CustomMinimumSize = new Vector2(28, 28),
				StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
				SizeFlagsHorizontal = 0
			};
			row.AddChild(texture);
		}

		var label = new Label
		{
			Text = FormatItem(entry),
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		row.AddChild(label);
		return row;
	}

	private static string FormatItem(InventoryItemSnapshot item)
	{
		var effects = item.Effects.Count == 0
			? "No effect"
			: string.Join(", ", item.Effects.Select(pair => $"{pair.Key}:{pair.Value}"));
		var buffSuffix = string.IsNullOrWhiteSpace(item.BuffName) ? string.Empty : $" [{item.BuffName}]";
		return $"{item.Name} — {effects}{buffSuffix}";
	}

	private Texture2D? GetItemIcon(string itemName)
	{
		var key = itemName.ToLowerInvariant();
		if (key.Contains("hp"))
		{
			return _itemIcons.GetValueOrDefault("hp");
		}

		if (key.Contains("mp"))
		{
			return _itemIcons.GetValueOrDefault("mp");
		}

		return _itemIcons.GetValueOrDefault("gold");
	}

	private static void ClearChildren(Node parent)
	{
		foreach (Node child in parent.GetChildren().ToArray())
		{
			child.QueueFree();
		}
	}
}
