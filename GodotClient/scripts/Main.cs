using System;
using System.Collections.Generic;
using System.IO;
using Game.Core.Combat;
using Game.Core.Progression;
using Godot;

namespace GameGodot;

public partial class Main : Control
{
	private enum UiPage
	{
		Menu,
		Hub,
		Battle,
		Shop
	}

	private Control _menuPanel = null!;
	private Control _hubPanel = null!;
	private Control _battlePanel = null!;
	private Control _shopPanel = null!;

	private Button _menuNewGameButton = null!;
	private Button _menuContinueButton = null!;
	private Button _menuLegacyButton = null!;

	private Button _hubEnterBattleButton = null!;
	private Button _hubShopButton = null!;
	private Button _hubBackButton = null!;

	private Button _shopBackButton = null!;
	private Button _shopBuyButton = null!;
	private Button _shopSellButton = null!;

	private GridContainer _heroSelectionGrid = null!;
	private Label _hubStatsLabel = null!;

	private VBoxContainer _inventoryList = null!;
	private VBoxContainer _shopList = null!;
	private PackedScene _heroCardPacked = null!;

	private readonly Dictionary<Specialization, Texture2D?> _heroPortraits = new();
	private readonly Dictionary<string, Texture2D?> _enemyPortraits = new(StringComparer.OrdinalIgnoreCase);
	private readonly Dictionary<string, Texture2D?> _itemIcons = new();

	private bool _hasSave;

	private RichTextLabel _status = null!;
	private OptionButton _skillSelect = null!;
	private OptionButton _targetSelect = null!;
	private ItemList _heroStatusList = null!;
	private ItemList _enemyStatusList = null!;
	private Label _activeTurnLabel = null!;
	private readonly Queue<string> _history = new();
	private BattleEngine _battle = null!;
	private IProgressStorage _storage = null!;
	private ProgressState _progress = new();

	public override void _Ready()
	{
		_menuPanel = GetNode<Control>("RootMargin/Layout/PanelStack/MenuPanel");
		_hubPanel = GetNode<Control>("RootMargin/Layout/PanelStack/HubPanel");
		_battlePanel = GetNode<Control>("RootMargin/Layout/PanelStack/BattlePanel");
		_shopPanel = GetNode<Control>("RootMargin/Layout/PanelStack/ShopPanel");

		_menuNewGameButton = _menuPanel.GetNode<Button>("MenuContent/MenuButtons/NewGameButton");
		_menuContinueButton = _menuPanel.GetNode<Button>("MenuContent/MenuButtons/ContinueButton");
		_menuLegacyButton = _menuPanel.GetNode<Button>("MenuContent/MenuButtons/LegacyButton");

		_hubStatsLabel = _hubPanel.GetNode<Label>("HubContent/HubStats");
		_heroSelectionGrid = _hubPanel.GetNode<GridContainer>("HubContent/HeroScroll/HeroSelectionGrid");
		_hubEnterBattleButton = _hubPanel.GetNode<Button>("HubContent/HubActions/EnterBattleButton");
		_hubShopButton = _hubPanel.GetNode<Button>("HubContent/HubActions/OpenShopButton");
		_hubBackButton = _hubPanel.GetNode<Button>("HubContent/HubActions/BackToMenuButton");

		_inventoryList = _shopPanel.GetNode<VBoxContainer>("ShopContent/ShopColumns/InventoryPanel/InventoryPanelLayout/InventoryScroll/InventoryList");
		_shopList = _shopPanel.GetNode<VBoxContainer>("ShopContent/ShopColumns/ShopItemsPanel/ShopPanelLayout/ShopScroll/ShopList");
		_shopBackButton = _shopPanel.GetNode<Button>("ShopContent/ShopButtons/BackToHubButton");
		_shopBuyButton = _shopPanel.GetNode<Button>("ShopContent/ShopButtons/BuyButton");
		_shopSellButton = _shopPanel.GetNode<Button>("ShopContent/ShopButtons/SellButton");

		var battleContent = _battlePanel.GetNode<VBoxContainer>("BattleContent");
		var statusContainer = battleContent.GetNode<ScrollContainer>("StatusContainer");
		_status = statusContainer.GetNode<RichTextLabel>("Status");
		_activeTurnLabel = battleContent.GetNode<Label>("ActiveTurnLabel");
		_skillSelect = battleContent.GetNode<OptionButton>("SkillRow/SkillSelect");
		_targetSelect = battleContent.GetNode<OptionButton>("SkillRow/TargetSelect");
		_heroStatusList = battleContent.GetNode<ItemList>("BattleCombatants/HeroPanel/HeroPanelLayout/HeroList");
		_enemyStatusList = battleContent.GetNode<ItemList>("BattleCombatants/EnemyPanel/EnemyPanelLayout/EnemyList");

		var castButton = battleContent.GetNode<Button>("SkillRow/CastButton");
		var attackButton = battleContent.GetNode<Button>("Actions/AttackButton");
		var healButton = battleContent.GetNode<Button>("Actions/HealButton");
		var endTurnButton = battleContent.GetNode<Button>("Actions/EndTurnButton");
		var restartButton = battleContent.GetNode<Button>("BattleActions/RestartButton");
		var backToHubButton = battleContent.GetNode<Button>("BattleActions/BackToHubButton");

		castButton.Pressed += ResolveSelectedSkill;
		attackButton.Pressed += () => Resolve(BattleAction.Attack);
		healButton.Pressed += () => Resolve(BattleAction.Heal);
		endTurnButton.Pressed += () => Resolve(BattleAction.Skip);
		restartButton.Pressed += Restart;
		backToHubButton.Pressed += ShowHubPage;
		_skillSelect.ItemSelected += _ => RefreshTargets();

		_menuNewGameButton.Pressed += StartNewCampaign;
		_menuContinueButton.Pressed += () => ShowHubPage();
		_menuLegacyButton.Pressed += () => TryImportLegacyProgressIfNeeded(ProjectSettings.GlobalizePath("user://migration-progress.json"), force: true);

		_hubEnterBattleButton.Pressed += EnterBattleFromHub;
		_hubShopButton.Pressed += ShowShopPage;
		_hubBackButton.Pressed += ShowMenuPage;

		_shopBackButton.Pressed += ShowHubPage;
		_shopBuyButton.Pressed += BuyBestAffordableItem;
		_shopSellButton.Pressed += SellCheapestItem;

		_heroCardPacked = ResourceLoader.Load<PackedScene>("res://scenes/HeroCard.tscn")!;
		LoadPortraits();
		LoadEnemyPortraits();
		LoadItemIcons();

		var savePath = ProjectSettings.GlobalizePath("user://migration-progress.json");
		_hasSave = File.Exists(savePath);
		_storage = new JsonFileProgressStorage(savePath);
		_progress = _storage.Load();
		TryImportLegacyProgressIfNeeded(savePath);

		UpdateMenuButtons();
		RefreshHubView();
		ShowPage(UiPage.Menu);
	}

	private void LoadPortraits()
	{
		var layout = new Dictionary<Specialization, string>
		{
			[Specialization.Warrior] = "res://assets/portraits/warrior.png",
			[Specialization.Archer] = "res://assets/portraits/archer.png",
			[Specialization.Wizard] = "res://assets/portraits/wizard.png"
		};

		foreach (var (key, path) in layout)
		{
			_heroPortraits[key] = ResourceLoader.Load<Texture2D>(path);
		}
	}

	private void LoadEnemyPortraits()
	{
		var layout = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			["skeleton"] = "res://assets/enemies/enemy_warrior.png",
			["ghoul"] = "res://assets/enemies/enemy_archer.png",
			["necromancer"] = "res://assets/enemies/enemy_wizard.png"
		};

		foreach (var (key, path) in layout)
		{
			_enemyPortraits[key] = ResourceLoader.Load<Texture2D>(path);
		}
	}

	private void LoadItemIcons()
	{
		_itemIcons["hp"] = ResourceLoader.Load<Texture2D>("res://assets/items/hp.png");
		_itemIcons["mp"] = ResourceLoader.Load<Texture2D>("res://assets/items/mp.png");
		_itemIcons["gold"] = ResourceLoader.Load<Texture2D>("res://assets/items/gold.png");
	}

	private void TryImportLegacyProgressIfNeeded(string progressPath, bool force = false)
	{
		if (!force && File.Exists(progressPath))
		{
			_hasSave = true;
			return;
		}

		var legacyPath = ProjectSettings.GlobalizePath("user://legacy-player.json");
		if (!File.Exists(legacyPath))
		{
			return;
		}

		var json = File.ReadAllText(legacyPath);
		if (!LegacyPlayerSaveMigration.TryMigrateFromLegacyPlayerJson(json, out var migrated))
		{
			return;
		}

		_progress = migrated;
		_storage.Save(_progress);
		_hasSave = true;
		PushLog($"Imported legacy save from {legacyPath}.");
		UpdateMenuButtons();
		RefreshHubView();
	}

	private void StartNewCampaign()
	{
		_progress = new ProgressState();
		_storage.Save(_progress);
		_hasSave = true;
		UpdateMenuButtons();
		RefreshHubView();
		ShowHubPage();
	}

	private void EnterBattleFromHub()
	{
		ShowPage(UiPage.Battle);
		Restart();
	}

	private void ShowShopPage()
	{
		ShowPage(UiPage.Shop);
		RefreshShopLists();
	}

	private void ShowHubPage()
	{
		ShowPage(UiPage.Hub);
		RefreshHubView();
	}

	private void ShowMenuPage()
	{
		ShowPage(UiPage.Menu);
	}

	private void UpdateMenuButtons()
	{
		_menuContinueButton.Disabled = !_hasSave;
	}

	private void ShowPage(UiPage page)
	{
		_menuPanel.Visible = page == UiPage.Menu;
		_hubPanel.Visible = page == UiPage.Hub;
		_battlePanel.Visible = page == UiPage.Battle;
		_shopPanel.Visible = page == UiPage.Shop;
	}

	private void Restart()
	{
		var setup = BattleSetupFactory.CreateProgressiveEncounter(
			_progress.GetHeroLevels(),
			_progress.EncounterCounter,
			randomSeed: 42);

		_battle = new BattleEngine(setup);
		_history.Clear();
		PushLog($"Encounter #{_progress.EncounterCounter} started.");
		RefreshSkillSelectors();
		RedrawUi();
		RefreshHubView();

		_progress = _progress with
		{
			EncounterCounter = _progress.EncounterCounter + 1
		};
		_storage.Save(_progress);
		RefreshHubView();
	}
}
