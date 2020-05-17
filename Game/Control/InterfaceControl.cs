using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public partial class InterfaceControl : UserControl
    {
        private Model.Game _game;
        private List<BasicCreature>[] _creatures;
        private Panel _panel = new Panel {Name = "Targets", Size = new Size(180, 170), Location = new Point(700, 10)};

        public InterfaceControl(Model.Game game)
        {
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
            e.Graphics.DrawImage(Helper.ImageTransfer[_game.CurrentCreature.Specialization], new Point(20, 20));
            e.Graphics.DrawString(_game.CurrentCreature.Name, new Font(FontFamily.GenericSerif, 12),
                new SolidBrush(Color.Coral), new Point(20, 120));
            e.Graphics.FillRectangle(new SolidBrush(Color.Gray), new RectangleF(175, 5, 1375, 165));
            e.Graphics.DrawRectangle(new Pen(Color.Black, 5), new Rectangle(170, 0, 1380, 170));
            int dy = 0;
            if (_game.CurrentCreature is Hero)
                for (var k = 0; k < _game.CurrentCreature.Skills.Count; k++)
                {
                    var skill = _game.CurrentCreature.Skills[k];
                    e.Graphics.DrawString(skill.Name, new Font(FontFamily.GenericSerif, 12),
                        new SolidBrush(Color.Coral),
                        200, 10 + dy);
                    e.Graphics.DrawString("ManaCost: " + skill.ManaCost.ToString(),
                        new Font(FontFamily.GenericSerif, 12),
                        new SolidBrush(Color.Coral), 400, 10 + dy);
                    e.Graphics.DrawString("Range: " + skill.Range.ToString(), new Font(FontFamily.GenericSerif, 12),
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
                            for (var i = 0; i < _creatures.Length; i++)
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