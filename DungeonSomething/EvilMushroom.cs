using SadConsole;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSomething
{
    public class EvilMushroom : Monster
    {
        public Point MonsterPosition;
        public ColoredGlyph MonsterLooks;
        private ColoredGlyph _mapLooks = new ColoredGlyph();
        private ColoredGlyph _futureGlyph = new ColoredGlyph();
        public MonsterStatusConsole MonsterStatusConsole = new MonsterStatusConsole();
        public bool mushroomIsInFire;

        public EvilMushroom(ColoredGlyph looks, Point position, IScreenSurface mainSurface)
        {
            MonsterLooks = looks;
            MonsterPosition = position;
            health = 50;
            maxHealth = 50;
            gold = 5;
            name = "Evil Mushroom";
            attackChance = 30;
            attack = 5;
            experience = 10;

            mainSurface.Surface[MonsterPosition].CopyAppearanceTo(_mapLooks);

            DrawMonsterObject(mainSurface);
        }

        private void DrawMonsterObject(IScreenSurface surface)
        {
            MonsterLooks.CopyAppearanceTo(surface.Surface[MonsterPosition]);
            surface.IsDirty = true;
        }

        // Monster movement and monster status console
        public void MonsterMove(Point newPositon, IScreenSurface surface)
        {
            if (newPositon.X > 0 && newPositon.X < 129 && newPositon.Y > 0 && newPositon.Y < 34)
            {
                _futureGlyph.CopyAppearanceFrom(surface.Surface[newPositon]);
                if (_futureGlyph.Foreground == Color.Red) mushroomIsInFire = true;
                if (_futureGlyph.Foreground == Color.Black || _futureGlyph.Foreground == Color.Magenta || _futureGlyph.Foreground == Color.Pink || _futureGlyph.Foreground == Color.Maroon)
                {
                    newPositon = MonsterPosition;
                }
                _mapLooks.CopyAppearanceTo(surface.Surface[MonsterPosition]);
                surface.Surface[newPositon].CopyAppearanceTo(_mapLooks);
                MonsterPosition = newPositon;
                DrawMonsterObject(surface);
            }
        }

        public int RLDUN()
        {
            Random theRand = new Random();
            return theRand.Next(1, 5);
        }
    }
}