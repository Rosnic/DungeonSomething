using SadConsole;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSomething
{
    public class StatusConsole
    {
        // Status console for the player
        public ScreenSurface CreateStatusConsole(Player thePlayer)
        {
            ScreenSurface playerStatus = new ScreenSurface(30, 10);
            playerStatus.Position = new Point(0, 35);
            playerStatus.Surface.DefaultBackground = Color.Black;
            playerStatus.Surface.Print(2, 1, $"Player");
            playerStatus.Surface.DrawLine(new Point(0, 2), new Point(30, 2), 45);
            playerStatus.Surface.Print(2, 3, $"Health     : {thePlayer.health}/{thePlayer.maxHealth}");
            playerStatus.Surface.DrawBox(new Rectangle(0, 0, 30, 10), ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick, new ColoredGlyph(Color.White, Color.Black)));
            return playerStatus;
        }
    }
}