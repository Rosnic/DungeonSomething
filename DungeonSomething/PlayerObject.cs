using SadConsole;
using SadConsole.Entities;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DungeonSomething
{
    // A player character
    public class PlayerObject : IPlayerObject
    {
        private int _firePosX;
        private int _firePosY;
        public bool isInFire;

        public Point Position { get; private set; }

        public ColoredGlyph Looks { get; set; }

        private ColoredGlyph _looksOnMap = new ColoredGlyph();

        private ColoredGlyph _futureGlyph = new ColoredGlyph();

        public PlayerObject(ColoredGlyph looks, Point position, IScreenSurface mainSurface)
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

        // Checks if the player is in fire, whether or not they would move into a monster or another player, and redraws the player
        public void Move(Point newPositon, IScreenSurface surface)
        {
            if (newPositon.X > 0 && newPositon.X < 129 && newPositon.Y > 0 && newPositon.Y < 34)
            {
                _futureGlyph.CopyAppearanceFrom(surface.Surface[newPositon]);
                if (_futureGlyph.Foreground == Color.Red) isInFire = true;
                if (_futureGlyph.Foreground == Color.Brown || _futureGlyph.Foreground == Color.Maroon)
                {
                    newPositon = Position;
                }
                _looksOnMap.CopyAppearanceTo(surface.Surface[Position]);
                surface.Surface[newPositon].CopyAppearanceTo(_looksOnMap);
                Position = newPositon;
                DrawPlayerObject(surface);
            }
        }

        // Creates a ring of fire
        public void RingOfFire(IScreenSurface surface)
        {
            _firePosX = Position.X - 4;
            _firePosY = Position.Y - 2;
            surface.Surface.DrawCircle(new Rectangle(_firePosX, _firePosY, 9, 5), ShapeParameters.CreateBorder(new ColoredGlyph(Color.Red, Color.DarkGray, 35)));
            surface.IsDirty = true;
            Thread ringPart2 = new Thread(() => RingOfFirePart2(surface));
            ringPart2.Start();
            Thread.Sleep(500);
        }

        // Creates a second ring of fire
        public void RingOfFirePart2(IScreenSurface surface)
        {
            Thread.Sleep(1000);
            _firePosX -= 2;
            _firePosY -= 1;
            surface.Surface.DrawCircle(new Rectangle(_firePosX, _firePosY, 13, 7), ShapeParameters.CreateBorder(new ColoredGlyph(Color.Red, Color.DarkGray, 35)));
            surface.IsDirty = true;
            Thread ringPart3 = new Thread(() => RingOfFirePart3(surface));
            ringPart3.Start();
        }

        // Removes the first ring of fire
        public void RingOfFirePart3(IScreenSurface surface)
        {
            Thread.Sleep(3000);
            _firePosX += 2;
            _firePosY += 1;
            surface.Surface.DrawCircle(new Rectangle(_firePosX, _firePosY, 9, 5), ShapeParameters.CreateBorder(new ColoredGlyph(Color.DarkGray, Color.DarkGray, 35)));
            surface.IsDirty = true;
            Thread ringPart4 = new Thread(() => RingOfFirePart4(surface));
            ringPart4.Start();
        }

        // Removes the second ring of fire
        public void RingOfFirePart4(IScreenSurface surface)
        {
            Thread.Sleep(1000);
            _firePosX -= 2;
            _firePosY -= 1;
            surface.Surface.DrawCircle(new Rectangle(_firePosX, _firePosY, 13, 7), ShapeParameters.CreateBorder(new ColoredGlyph(Color.DarkGray, Color.DarkGray, 35)));
            surface.IsDirty = true;
        }
    }
}