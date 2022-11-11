using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace DungeonSomething
{
    public class RootScreen : ScreenObject
    {
        private static ScreenSurface _gameMap;
        public PlayerObject player;
        public PlayerObject2 player2;
        private int _coolDownFlag = 0;
        public EvilMushroom monster;
        private Player _thePlayer;
        private StatusConsole _stats;
        private static MessageConsole _message;
        public List<string> whatToSay;
        public List<string> thingsToSay;
        public Color[] colours;
        private Exception _exception;
        public ScreenSurface statusConsole;
        public ScreenSurface monsterStats;
        public MonsterStatusConsole monsterStatusConsole;

        private static System.Timers.Timer _timer;
        private static System.Timers.Timer _timer2;
        private static System.Timers.Timer _timer3;

        private readonly object _speechLock = new object();

        public RootScreen()
        {
            monsterStatusConsole = new MonsterStatusConsole();
            _exception = new Exception("Crazy Exception!");
            _timer = new System.Timers.Timer(10000);
            _timer2 = new System.Timers.Timer(1000);
            _timer3 = new System.Timers.Timer(3000);
            _message = new MessageConsole();
            _stats = new StatusConsole();
            _thePlayer = new Player();
            colours = new Color[3] { Color.Magenta, Color.Pink, Color.Black };

            _gameMap = new ScreenSurface(Game.Instance.ScreenCellsX - 20, Game.Instance.ScreenCellsY - 10);
            _gameMap.UseMouse = false;

            FillBackground();

            statusConsole = _stats.CreateStatusConsole(_thePlayer);

            // Draws the different consoles/surfaces as childen to the root screen
            Children.Add(statusConsole);
            Children.Add(_gameMap);
            Children.Add(_message.CreateMessageConsole());

            // Used later in the program for text appearing in the message console
            thingsToSay = new List<string> { "", "", "", "", "" };
            whatToSay = new List<string> { "Flippendo!", "Wingardium Leviosa", "Damn You!", "Expelliarmus!", "Tea?" };

            // Draws the players and the monster on the game map
            player = new PlayerObject(new ColoredGlyph(Color.Black, Color.DarkGray, 2), _gameMap.Surface.Area.Center, _gameMap);
            player2 = new PlayerObject2(new ColoredGlyph(Color.Maroon, Color.DarkGray, 2), _gameMap.Surface.Area.Center - 2, _gameMap);
            monster = new EvilMushroom(new ColoredGlyph(Color.Brown, Color.DarkGray, 6), new Point(3, 5), _gameMap);

            monsterStats = monsterStatusConsole.CreateStatusConsoleMonster(monster);
            Children.Add(monsterStats);

            // Starts a timer for the monster movement
            MonsterMoveTimer();
            SpeechTimer();
        }

        public override void Update(TimeSpan delta)
        {
            statusConsole.Update(delta);
        }

        private void RinfOfFireCoolDown()
        {
            _timer.Elapsed += CoolDownOver;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void MonsterMoveTimer()
        {
            _timer2.Elapsed += MonsterMoved;
            _timer2.AutoReset = true;
            _timer2.Enabled = true;
        }

        private void SpeechTimer()
        {
            _timer3.Elapsed += SpeechTimerOver;
            _timer3.AutoReset = true;
            _timer3.Enabled = true;
        }

        private void FillBackground() // Fylder baggrunden med grå og laver en linje hele vejen rundt om _gameMap surface
        {
            _gameMap.Surface.DefaultBackground = Color.DarkGray;
            _gameMap.Surface.DrawBox(new Rectangle(0, 0, 130, 35), ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick, new ColoredGlyph(Color.Black, Color.DarkGray)));
        }

        private void CoolDownOver(object source, ElapsedEventArgs e)
        {
            // Cooldown for the fire ring ability of the player
            _coolDownFlag = 0;
        }

        private void MonsterMoved(object source, ElapsedEventArgs e)
        {
            MonsterMoves();
        }

        private async void SpeechTimerOver(object source, ElapsedEventArgs e)
        {
            Thread monsterSpeak = new Thread(() =>
            {
                // Locks the speech to be used by only one of the threads at a time, since it changes the contents of an array, that it reads from
                lock (_speechLock)
                {
                    Speech(_message.messageConsole, SpeechString(), whatToSay, thingsToSay);
                }
            });

            Thread playerSpeak = new Thread(() =>
            {
                lock (_speechLock)
                {
                    Speech(_message.messageConsole, SpeechString(), whatToSay, thingsToSay);
                }
            });

            monsterSpeak.Start();
            playerSpeak.Start();
            _message.messageConsole.Surface.Print(1, 4, await ReadDocument());
        }

        // Reads a .txt file and returns the value (all the text)
        public async Task<string> ReadDocument()
        {
            string textResult = "";
            string filePath = "..\\RandomThingsToSay.txt";
            if (File.Exists(filePath))
            {
                textResult = await File.ReadAllTextAsync(filePath);
            }
            else
            {
                textResult = "I Read This From A Document! :D";
                File.WriteAllText(filePath, textResult);
            }
            return textResult;
        }

        // Reads keyboard inputs and reacts as coded. Needs a "handled = true" to indiate, that the action is done, and it needs to react.
        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            bool handled = false;

            if (keyboard.IsKeyPressed(Keys.Up))
            {
                MoveKeysFunction(Direction.Up);
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.Down))
            {
                MoveKeysFunction(Direction.Down);
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.Left))
            {
                MoveKeysFunction(Direction.Left);
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.Right))
            {
                MoveKeysFunction(Direction.Right);
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.W))
            {
                MoveKeysFunction2(Direction.Up);
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.S))
            {
                MoveKeysFunction2(Direction.Down);
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.A))
            {
                MoveKeysFunction2(Direction.Left);
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.D))
            {
                MoveKeysFunction2(Direction.Right);
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.F))
            {
                if (_coolDownFlag == 0)
                {
                    player.RingOfFire(_gameMap);
                    _gameMap.IsDirty = true;
                    _coolDownFlag = 1;
                    RinfOfFireCoolDown();
                }
            }

            return handled;
        }

        // Moves the player and makes sure, that they take damage upon entering fire. Updates the players status console
        public void MoveKeysFunction(Direction direction)
        {
            Thread player1Move = new Thread(() => player.Move(player.Position + direction, _gameMap));
            player1Move.Start();
            if (player.isInFire == true)
            {
                _thePlayer.health -= 10;
                player.isInFire = false;
                statusConsole.Dispose();
                statusConsole = _stats.CreateStatusConsole(_thePlayer);
                Children.Add(statusConsole);
            }
        }

        // Movement for player 2
        public void MoveKeysFunction2(Direction direction)
        {
            Thread player2Move = new Thread(() => player2.Move(player2.Position + direction, _gameMap));
            player2Move.Start();
        }

        // Determines, whether the monster or player displays a new message
        public string SpeechString()
        {
            int rand = new Random().Next(1, 3);
            if (rand == 1) return "Player";
            else return "Monster";
        }

        public void Speech(IScreenSurface surface, string speaker, List<string> whatToSay, List<string> thingsToSay)
        {
            // Random try/catch exception block in here just to show it. It changes the player's colour and writes to the message surface
            // Prints the messages in the message surface as well as choosing which message and altering the choices of messages
            try
            {
                int posX = 1;
                int posY = 1;
                if (speaker == "Monster") posY = 2;
                thingsToSay[new Random().Next(0, 5)] = whatToSay[new Random().Next(0, 5)];
                Thread.Sleep(100);
                for (int i = 0; i < 40; i++) surface.Surface.SetForeground(1, i, Color.WhiteSmoke);
                surface.Surface.Print(posX, posY, "                                                       ");
                surface.Surface.Print(posX, posY, $"{speaker}: {thingsToSay[new Random().Next(0, 5)]}");
                if (speaker == "Player") throw _exception;
            }
            catch (Exception ex)
            {
                player.Looks.Foreground = colours[new Random().Next(0, 3)];
                surface.Surface.Print(1, 3, $"I caught an exception ({ex.Message}). Change colour!");
            }
        }

        // Movement, health checks and death for the monster
        public void MonsterMoves()
        {
            if (monster.mushroomIsInFire == true)
            {
                monster.health -= 10;
                monster.mushroomIsInFire = false;
                monsterStats.Dispose();
                monsterStats = monsterStatusConsole.CreateStatusConsoleMonster(monster);
                Children.Add(monsterStats);
            }
            // Disables the timers, that make the monster move and send messages, as well as clearing the message of the monster
            if (monster.health <= 0)
            {
                _timer2.AutoReset = false;
                _timer2.Enabled = false;
                monster.MonsterLooks.Glyph = 15;
                _timer3.AutoReset = false;
                _timer3.Enabled = false;
                _message.messageConsole.Surface.Print(1, 2, "                                               ");
            }

            // Random movement for the monster
            int doesItMove = monster.RLDUN();
            if (doesItMove == 1)
            {
                monster.MonsterMove(monster.MonsterPosition + Direction.Right, _gameMap);
            }
            else if (doesItMove == 2)
            {
                monster.MonsterMove(monster.MonsterPosition + Direction.Left, _gameMap);
            }
            else if (doesItMove == 3)
            {
                monster.MonsterMove(monster.MonsterPosition + Direction.Down, _gameMap);
            }
            else if (doesItMove == 4)
            {
                monster.MonsterMove(monster.MonsterPosition + Direction.Up, _gameMap);
            }
        }
    }
}