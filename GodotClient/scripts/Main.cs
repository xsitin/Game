using System.Collections.Generic;
using System.IO;
using System.Text;
using Game.Core.Combat;
using Game.Core.Progression;
using Godot;

namespace GameGodot;

public partial class Main : Control
{
	private RichTextLabel _status = null!;
	private OptionButton _skillSelect = null!;
	private OptionButton _targetSelect = null!;
	private readonly Queue<string> _history = new();
	private BattleEngine _battle = null!;
	private IProgressStorage _storage = null!;
	private ProgressState _progress = new();

	public override void _Ready()
	{
		_status = GetNode<RichTextLabel>("RootMargin/Layout/Status");
		_skillSelect = GetNode<OptionButton>("RootMargin/Layout/SkillRow/SkillSelect");
		_targetSelect = GetNode<OptionButton>("RootMargin/Layout/SkillRow/TargetSelect");

		var castButton = GetNode<Button>("RootMargin/Layout/SkillRow/CastButton");
		var attackButton = GetNode<Button>("RootMargin/Layout/Actions/AttackButton");
		var healButton = GetNode<Button>("RootMargin/Layout/Actions/HealButton");
		var endTurnButton = GetNode<Button>("RootMargin/Layout/Actions/EndTurnButton");
		var restartButton = GetNode<Button>("RootMargin/Layout/RestartButton");

		castButton.Pressed += ResolveSelectedSkill;
		attackButton.Pressed += () => Resolve(BattleAction.Attack);
		healButton.Pressed += () => Resolve(BattleAction.Heal);
		endTurnButton.Pressed += () => Resolve(BattleAction.Skip);
		restartButton.Pressed += Restart;
		_skillSelect.ItemSelected += _ => RefreshTargets();

		var savePath = ProjectSettings.GlobalizePath("user://migration-progress.json");
		_storage = new JsonFileProgressStorage(savePath);
		_progress = _storage.Load();
		TryImportLegacyProgressIfNeeded(savePath);

		Restart();
	}


	private void TryImportLegacyProgressIfNeeded(string progressPath)
	{
		if (File.Exists(progressPath))
		{
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
		PushLog($"Imported legacy save from {legacyPath}.");
	}

	private void Restart()
	{
		var setup = BattleSetupFactory.CreateProgressiveEncounter(
			_progress.HeroLevels,
			_progress.EncounterCounter,
			randomSeed: 42);

		_battle = new BattleEngine(setup);
		_history.Clear();
		PushLog($"Encounter #{_progress.EncounterCounter} started.");
		RefreshSkillSelectors();
		Redraw();

		_progress = new ProgressState
		{
			EncounterCounter = _progress.EncounterCounter + 1,
			HeroLevels = _progress.HeroLevels
		};
		_storage.Save(_progress);
	}

	private void Resolve(BattleAction action)
	{
		foreach (var message in _battle.ApplyHeroAction(action))
		{
			PushLog(message);
		}

		RefreshSkillSelectors();
		Redraw();
	}

	private void ResolveSelectedSkill()
	{
		if (_battle.IsBattleFinished)
		{
			return;
		}

		var skillIndex = _skillSelect.GetSelectedId();
		var targetIndex = _targetSelect.GetSelectedId();

		foreach (var message in _battle.ApplyHeroSkill(skillIndex < 0 ? 0 : skillIndex, targetIndex < 0 ? 0 : targetIndex))
		{
			PushLog(message);
		}

		RefreshSkillSelectors();
		Redraw();
	}

	private void RefreshSkillSelectors()
	{
		_skillSelect.Clear();
		_targetSelect.Clear();

		if (_battle.IsBattleFinished || _battle.ActiveCombatant.IsEnemy)
		{
			return;
		}

		var skills = _battle.GetActiveHeroSkills();
		for (var i = 0; i < skills.Count; i++)
		{
			var skill = skills[i];
			_skillSelect.AddItem($"{skill.Name} ({skill.ManaCost} MP)", i);
		}

		if (skills.Count > 0)
		{
			_skillSelect.Select(0);
		}

		RefreshTargets();
	}

	private void RefreshTargets()
	{
		_targetSelect.Clear();
		if (_battle.IsBattleFinished || _battle.ActiveCombatant.IsEnemy)
		{
			return;
		}

		var selectedSkill = _skillSelect.GetSelectedId();
		var targets = _battle.GetTargetsForActiveHeroSkill(selectedSkill < 0 ? 0 : selectedSkill);
		for (var i = 0; i < targets.Count; i++)
		{
			var target = targets[i];
			_targetSelect.AddItem($"{target.Name} ({target.Health} HP)", i);
		}

		if (targets.Count > 0)
		{
			_targetSelect.Select(0);
		}
	}

	private void PushLog(string message)
	{
		_history.Enqueue(message);
		while (_history.Count > 10)
		{
			_history.Dequeue();
		}
	}

	private void Redraw()
	{
		var builder = new StringBuilder();
		builder.AppendLine(_battle.BuildStatusText());
		builder.AppendLine();
		builder.AppendLine("[b]Log[/b]");

		foreach (var message in _history)
		{
			builder.AppendLine($"• {message}");
		}

		if (!_battle.IsBattleFinished)
		{
			builder.AppendLine();
			builder.AppendLine($"Active: [b]{_battle.ActiveCombatant.Name}[/b]");
		}

		_status.Text = builder.ToString();
	}
}
