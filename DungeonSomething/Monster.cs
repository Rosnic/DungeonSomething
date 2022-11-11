using SadConsole;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonSomething
{
    // A base for any future monster
    public class Monster
    {
        public Point MonstersPosition;
        public ColoredGlyph MonstersLooks;

        private int _attack;
        private int _health;
        private int _gold;
        private int _attackChance;
        private string _name;
        private int _maxHealth;
        private int _experience;

        public int attack
        {
            get { return _attack; }
            set { _attack = value; }
        }

        public int health
        {
            get { return _health; }
            set { _health = value; }
        }

        public int gold
        {
            get { return _gold; }
            set { _gold = value; }
        }

        public int attackChance
        {
            get { return _attackChance; }
            set { _attackChance = value; }
        }

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int maxHealth
        {
            get { return _maxHealth; }
            set { _maxHealth = value; }
        }

        public int experience
        {
            get { return _experience; }
            set { _experience = value; }
        }
    }
}