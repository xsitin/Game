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
        public InterfaceControl(Model.Game game)
        {
            _game = game;
            _game.NextStep();
            var panel = new Panel {Name = "Targets", Size = new Size(180, 170), Location = new Point(700, 10)};
            Controls.Add(panel);
            _creatures = new[]
            {
                _game.Heroes.SecondLine.Cast<BasicCreature>().ToList(),
                _game.Heroes.FirstLine.Cast<BasicCreature>().ToList(),
                _game.Enemy.FirstLine.Cast<BasicCreature>().ToList(),
                _game.Enemy.SecondLine.Cast<BasicCreature>().ToList()
            };
        }

        public override void Refresh()
        {
            _creatures = new[]
            {
                _game.Heroes.SecondLine.Cast<BasicCreature>().ToList(),
                _game.Heroes.FirstLine.Cast<BasicCreature>().ToList(),
                _game.Enemy.FirstLine.Cast<BasicCreature>().ToList(),
                _game.Enemy.SecondLine.Cast<BasicCreature>().ToList()
            };
            base.Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(Helper.ImageTransfer[_game.CurrentCreature.Specialization], new Point(20, 20));
            e.Graphics.DrawString(_game.CurrentCreature.Name, new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Coral), new Point(20, 120));
            e.Graphics.FillRectangle(new SolidBrush(Color.Gray), new RectangleF(175, 5, 1375, 165));
            e.Graphics.DrawRectangle(new Pen(Color.Black, 5), new Rectangle(170, 0, 1380, 170));
            int dy = 0;
            if (!(_game.CurrentCreature is Hero hero))
            {
                if (Controls.Contains(Controls["Targets"]))
                    Controls["Targets"].Controls.Clear();
                Controls.Clear();
            }
            else
                foreach (var skill in _game.CurrentCreature.Skills)
                {
                    e.Graphics.DrawString(skill.Name, new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Coral),
                        200, 10 + dy);
                    e.Graphics.DrawString("ManaCost: " + skill.ManaCost.ToString(), new Font(FontFamily.GenericSerif, 12),
                        new SolidBrush(Color.Coral), 400, 10 + dy);
                    e.Graphics.DrawString("Range: " + skill.Range.ToString(), new Font(FontFamily.GenericSerif, 12),
                        new SolidBrush(Color.Coral), 550, 10 + dy);
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 3), new Rectangle(190, 10 + dy, 500, 20));
                    var skillButton = new Button
                    {
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
                        Controls["Targets"].Controls.Clear();
                        if (skill.Range is SkillRange.Single)
                        {
                            for (int i = 0; i < _creatures.Length; i++)
                            {
                                for (int j = 0; j < _creatures[i].Count; j++)
                                {
                                    if (_creatures[i][j].Characteristics[Characteristics.Health] < 0)
                                    {
                                        _creatures[i].RemoveAt(j);
                                        continue;
                                    }
                                    
                                    var butt = new EnemyButton(_creatures[i][j]) {Location = new Point(40 * i, 40 * j)};
                                    var i1 = i;
                                    var j1 = j;
                                    butt.Click += (o, eventArgs) =>
                                    {
                                        try
                                        {
                                            _game.CurrentCreature.UseSkill(skill, _creatures[i1][j1]);
                                            _game.NextStep();
                                            Parent.Controls["HitPoints"].Refresh();
                                            Parent.Controls["EnemyHitPoints"].Refresh();
                                            Parent.Controls["Field"].Refresh();
                                            Parent.Refresh();
                                            Refresh();
                                        }
                                        catch
                                        {
                                        }

                                        _game.NextStep();
                                    };
                                    Controls["Targets"].Controls.Add(butt);
                                }
                            }
                        }
                        else if (skill.Range is SkillRange.Enemies)
                        {
                            _game.CurrentCreature.UseSkill(skill, _game.Enemy.GetTeamList().ToArray());
                            _game.NextStep();
                            Parent.Controls["HitPoints"].Refresh();
                            Parent.Controls["EnemyHitPoints"].Refresh();
                            Parent.Controls["Field"].Refresh();
                            Parent.Refresh();
                            Refresh();
                        }
                        else if (skill.Range is SkillRange.All)
                        {
                            _game.CurrentCreature.UseSkill(skill, _game.Enemy.GetTeamList().Concat(_game.Heroes.GetTeamList()).ToArray());
                            _game.NextStep();
                            Parent.Controls["HitPoints"].Refresh();
                            Parent.Controls["EnemyHitPoints"].Refresh();
                            Parent.Controls["Field"].Refresh();
                            Parent.Refresh();
                            Refresh();
                        }
                    };
                    Controls.Add(skillButton);
                    dy += 50;
                }
        }
    }
}