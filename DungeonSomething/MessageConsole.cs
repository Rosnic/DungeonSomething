using SadConsole;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSomething
{
    public class MessageConsole
    {
        public ScreenSurface messageConsole;

        // Creates a new console for messages
        public ScreenSurface CreateMessageConsole()
        {
            messageConsole = new ScreenSurface(70, 10);
            messageConsole.Position = new Point(60, 35);
            messageConsole.Surface.DefaultBackground = Color.Black;
            messageConsole.Surface.DrawBox(new Rectangle(0, 0, 70, 10), ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick, new ColoredGlyph(Color.White, Color.Black)));
            return messageConsole;
        }
    }
}