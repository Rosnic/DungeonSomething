using SadConsole;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSomething
{
    public class PlayerObject2 : IPlayerObject
    {
        public bool isInFire;

        public Point Position { get; private set; }

        public ColoredGlyph Looks { get; set; }

        private ColoredGlyph _looksOnMap = new ColoredGlyph();

        private ColoredGlyph _futureGlyph = new ColoredGlyph();

        public PlayerObject2(ColoredGlyph looks, Point position, IScreenSurface mainSurface)
        {
            Position = position;
            Looks = looks;

            mainSurface.Surface[Position].CopyAppearanceTo(_looksOnMap);

            DrawPlayerObject(mainSurface);
        }

        public void DrawPlayerObject(IScreenSurface surface)
        {
            Looks.CopyAppearanceTo(surface.Surface[Position]);
            surface.IsDirty = true;
        }

        public void Move(Point newPositon, IScreenSurface surface)
        {
            if (newPositon.X > 0 && newPositon.X < 129 && newPositon.Y > 0 && newPositon.Y < 34)
            {
                _futureGlyph.CopyAppearanceFrom(surface.Surface[newPositon]);
                if (_futureGlyph.Foreground == Color.Red) isInFire = true;
                if (_futureGlyph.Foreground == Color.Brown || _futureGlyph.Foreground == Color.Magenta || _futureGlyph.Foreground == Color.Pink || _futureGlyph.Foreground == Color.Black)
                {
                    newPositon = Position;
                }
                _looksOnMap.CopyAppearanceTo(surface.Surface[Position]);
                surface.Surface[newPositon].CopyAppearanceTo(_looksOnMap);
                Position = newPositon;
                DrawPlayerObject(surface);
            }
        }
    }
}