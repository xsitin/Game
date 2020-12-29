using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Game.Model;
using GameCore.Resources;

namespace Game.Control
{
    public partial class InterfaceControl : UserControl
    {
        private readonly List<BasicCreature>[] _creatures;
        private readonly Model.Game _game;

        private readonly Panel _panel = new()
            {Name = "Targets", Size = new Size(180, 170), Location = new Point(700, 10)};

        public InterfaceControl(Model.Game game)
        {
            DoubleBuffered = true;
            _game = game;
            Controls.Add(_panel);
            _creatures = new[]
            {
                _game.Heroes.SecondLine.Cast<BasicCreature>().ToList(),
                _game.Heroes.FirstLine.Cast<BasicCreature>().ToList(),
                _game.Enemy.FirstLine.Cast<BasicCreature>().ToList(),
                _game.Enemy.SecondLine.Cast<BasicCreature>().ToList()
            };
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            while (_game.CurrentCreature == null)
                _game.NextStep();
            var HpPotions = _game.CurrentCreature.Inventory.Where(x => x.Name == "HP Potion").ToList();
            var MpPotions = _game.CurrentCreature.Inventory.Where(x => x.Name == "MP Potion").ToList();
            e.Graphics.DrawImage(Helper.ImageTransfer[_game.CurrentCreature.Specialization], new Point(20, 20));
            e.Graphics.DrawString(_game.CurrentCreature.Name, new Font(FontFamily.GenericSerif, 12),
                new SolidBrush(Color.Coral), new Point(20, 120));
            e.Graphics.FillRectangle(new SolidBrush(Color.Gray), new RectangleF(175, 5, 1375, 165));
            e.Graphics.DrawRectangle(new Pen(Color.Black, 5), new Rectangle(170, 0, 1380, 170));
            if (HpPotions.Any())
            {
                e.Graphics.DrawImage(Resource.HPpotion, new Point(890, 10));
                if (!Controls.Find("HpPotion", true).Any())
                {
                    var hp = new Button {Name = "HpPotion", BackColor = Color.Transparent, FlatStyle = FlatStyle.Flat};
                    hp.FlatAppearance.BorderSize = 0;
                    hp.FlatAppearance.CheckedBackColor = Color.Transparent;
                    hp.FlatAppearance.MouseDownBackColor = Color.Transparent;
                    hp.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    hp.Bounds = new Rectangle(890, 10, 50, 60);
                    hp.Click += (a, b) =>
                    {
                        _game.CurrentCreature.HpChange(
                            HpPotions.First().Actions.First(x => x.characteristic == Characteristics.Health).value,
                            true);
                        _game.CurrentCreature.Inventory.Remove(HpPotions.First());
                        HpPotions.Remove(HpPotions.First());
                        Parent.Refresh();
                    };
                    Controls.Add(hp);
                }
            }

            if (MpPotions.Any())
            {
                e.Graphics.DrawImage(Resource.MPpotion, new Point(990, 10));
                if (!Controls.Find("MpPotion", true).Any())
                {
                    var mp = new Button {Name = "MpPotion", BackColor = Color.Transparent, FlatStyle = FlatStyle.Flat};
                    mp.FlatAppearance.BorderSize = 0;
                    mp.FlatAppearance.CheckedBackColor = Color.Transparent;
                    mp.FlatAppearance.MouseDownBackColor = Color.Transparent;
                    mp.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    mp.Bounds = new Rectangle(990, 10, 50, 60);
                    mp.Click += (a, b) =>
                    {
                        _game.CurrentCreature.Characteristics[Characteristics.Mana] +=
                            MpPotions.First().Actions.First(x => x.characteristic == Characteristics.Mana).value;
                        _game.CurrentCreature.Inventory.Remove(MpPotions.First());
                        MpPotions.Remove(MpPotions.First());
                        Parent.Refresh();
                    };
                    Controls.Add(mp);
                }
            }

            var dy = 0;
            if (_game.CurrentCreature is Hero)
                for (var k = 0; k < _game.CurrentCreature.Skills.Count; k++)
                {
                    var skill = _game.CurrentCreature.Skills[k];
                    e.Graphics.DrawString(skill.Name, new Font(FontFamily.GenericSerif, 12),
                        new SolidBrush(Color.Coral),
                        200, 10 + dy);
                    e.Graphics.DrawString("ManaCost: " + skill.ManaCost,
                        new Font(FontFamily.GenericSerif, 12),
                        new SolidBrush(Color.Coral), 400, 10 + dy);
                    e.Graphics.DrawString("Range: " + skill.Range, new Font(FontFamily.GenericSerif, 12),
                        new SolidBrush(Color.Coral), 550, 10 + dy);
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 3), new Rectangle(190, 10 + dy, 500, 20));
                    var skillButton = new Button
                    {
                        Name = "Skill",
                        BackColor = Color.Transparent,
                        FlatStyle = FlatStyle.Flat,
                        Bounds = new Rectangle(190, 10 + dy, 500, 20)
                    };
                    skillButton.FlatAppearance.BorderSize = 0;
                    skillButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
                    skillButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    skillButton.FlatAppearance.CheckedBackColor = Color.Transparent;
                    skillButton.Click += (sender, args) =>
                    {
                        _panel.Controls.Clear();
                        if (skill.Range == SkillRange.Single)
                        {
                            if (!Controls.Find("Targets", true).Any())
                                Controls.Add(_panel);
                            for (var i = 0;
                                i < _creatures.Length - (_game.CurrentCreature.Position == Position.Melee ? 1 : 0);
                                i++)
                            for (var j = 0; j < _creatures[i].Count; j++)
                            {
                                if (_creatures[i][j].Characteristics[Characteristics.Health] < 0)
                                {
                                    _creatures[i].RemoveAt(j);
                                    continue;
                                }

                                var i1 = i;
                                var j1 = j;
                                var cr = _creatures[i1][j1];
                                var butt = new EnemyButton(cr) {Location = new Point(40 * i1, 40 * j1)};
                                butt.Click += (o, eventArgs) =>
                                {
                                    var sk = skill;
                                    if (o is EnemyButton btn)
                                        _game.CurrentCreature.UseSkill(sk, btn.EnemyHero);
                                    _game.NextStep();
                                    if (Parent != null)
                                    {
                                        Parent.Controls["HitPoints"].Refresh();
                                        Parent.Controls["EnemyHitPoints"].Refresh();
                                        Parent.Controls["Interface"].Refresh();
                                        Parent.Refresh();
                                    }

                                    Controls.Clear();
                                    _panel.Controls.Clear();
                                    Refresh();
                                };
                                _panel.Controls.Add(butt);
                            }
                        }
                        else
                        {
                            var targets = new List<BasicCreature>();
                            switch (skill.Range)
                            {
                                case SkillRange.Enemies:
                                    targets.AddRange(_game.Enemy.GetTeamList());
                                    break;
                                case SkillRange.Friendly:
                                    targets.AddRange(_game.Heroes.GetTeamList());
                                    break;
                                case SkillRange.All:
                                    targets.AddRange(_game.Heroes.GetTeamList());
                                    targets.AddRange(_game.Enemy.GetTeamList());
                                    break;
                            }

                            _game.CurrentCreature.UseSkill(skill, targets.ToArray());
                            _game.NextStep();
                            if (Parent != null)
                            {
                                Parent.Controls["HitPoints"].Refresh();
                                Parent.Controls["EnemyHitPoints"].Refresh();
                                Parent.Controls["Interface"].Refresh();
                                Parent.Refresh();
                            }

                            Controls.Clear();
                            Refresh();
                        }
                    };
                    Controls.Add(skillButton);
                    dy += 50;
                }
        }
    }
}