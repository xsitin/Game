using System;
using System.Linq;
using System.Text;
using Game.Core.Combat;
using Godot;

namespace GameGodot;

public partial class Main
{
	private void Resolve(BattleAction action)
	{
		foreach (var message in _battle.ApplyHeroAction(action))
		{
			PushLog(message);
		}

		RefreshSkillSelectors();
		RedrawUi();
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
		RedrawUi();
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

	private void RefreshCombatantLists()
	{
		if (_battle == null)
		{
			return;
		}

		_heroStatusList.Clear();
		foreach (var hero in _battle.Heroes)
		{
			_heroStatusList.AddItem(FormatCombatantEntry(hero), GetCombatPortrait(hero));
		}

		_enemyStatusList.Clear();
		foreach (var enemy in _battle.Enemies)
		{
			_enemyStatusList.AddItem(FormatCombatantEntry(enemy), GetCombatPortrait(enemy));
		}
	}

	private Texture2D? GetCombatPortrait(Combatant combatant)
	{
		var key = combatant.Name.ToLowerInvariant();
		if (combatant.IsEnemy)
		{
			if (_enemyPortraits.TryGetValue(key, out var enemyTexture))
			{
				return enemyTexture;
			}

			if (key.Contains("necromancer") && _enemyPortraits.TryGetValue("necromancer", out enemyTexture))
			{
				return enemyTexture;
			}

			if (key.Contains("ghoul") && _enemyPortraits.TryGetValue("ghoul", out enemyTexture))
			{
				return enemyTexture;
			}

			return _enemyPortraits.TryGetValue("skeleton", out enemyTexture) ? enemyTexture : null;
		}

		if (key.Contains("archer"))
		{
			return _heroPortraits.TryGetValue(Specialization.Archer, out var heroTexture) ? heroTexture : null;
		}

		if (key.Contains("priest") || key.Contains("mage") || key.Contains("wizard"))
		{
			return _heroPortraits.TryGetValue(Specialization.Wizard, out var heroTexture) ? heroTexture : null;
		}

		return _heroPortraits.TryGetValue(Specialization.Warrior, out var defaultTexture) ? defaultTexture : null;
	}

	private static string FormatCombatantEntry(Combatant combatant)
	{
		var alive = combatant.IsAlive ? "Alive" : "Down";
		return $"{combatant.Name} | HP {combatant.Health}/{combatant.MaxHealth} | Mana {combatant.Mana} | Init {combatant.Initiative} | {alive}";
	}

	private void PushLog(string message)
	{
		_history.Enqueue(message);
		while (_history.Count > 10)
		{
			_history.Dequeue();
		}
	}

	private void RedrawUi()
	{
		var builder = new StringBuilder();
		builder.AppendLine(_battle.BuildStatusText());
		builder.AppendLine();
		builder.AppendLine("[b]Log[/b]");

		foreach (var message in _history)
		{
			builder.AppendLine($"- {message}");
		}

		if (!_battle.IsBattleFinished)
		{
			builder.AppendLine();
			builder.AppendLine($"Active: [b]{_battle.ActiveCombatant.Name}[/b]");
		}

		_status.Text = builder.ToString();
		UpdateActiveTurnLabel();
		RefreshCombatantLists();
	}

	private void UpdateActiveTurnLabel()
	{
		if (_battle.IsBattleFinished)
		{
			_activeTurnLabel.Text = $"Battle finished: {_battle.Winner ?? "Unknown"}";
			return;
		}

		var prefix = _battle.ActiveCombatant.IsEnemy ? "Enemy turn" : "Hero turn";
		_activeTurnLabel.Text = $"{prefix}: {_battle.ActiveCombatant.Name}";
	}
}
