using SadConsole;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSomething
{
    public class MonsterStatusConsole
    {
        // Status console for the monster
        public ScreenSurface CreateStatusConsoleMonster(EvilMushroom theMushroom)
        {
            ScreenSurface monsterStatus = new ScreenSurface(30, 10);
            monsterStatus.Position = new Point(30, 35);
            monsterStatus.Surface.DefaultBackground = Color.Black;
            monsterStatus.Surface.Print(2, 1, $"Evil Mushroom");
            monsterStatus.Surface.DrawLine(new Point(0, 2), new Point(30, 2), 45);
            monsterStatus.Surface.Print(2, 3, $"Health     : {theMushroom.health}/{theMushroom.maxHealth}");
            monsterStatus.Surface.DrawBox(new Rectangle(0, 0, 30, 10), ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick, new ColoredGlyph(Color.White, Color.Black)));
            return monsterStatus;
        }
    }
}