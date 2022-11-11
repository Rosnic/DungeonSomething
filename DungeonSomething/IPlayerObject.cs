using SadConsole;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSomething
{
    public interface IPlayerObject
    {
        public void Move(Point newPosition, IScreenSurface screenSurface);

        public void DrawPlayerObject(IScreenSurface surface);
    }
}