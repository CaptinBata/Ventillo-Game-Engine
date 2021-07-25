using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Window;
using SFML.System;
using SFML.Graphics;

using Ventillo.Utils;
using Ventillo.GameObjects;
using Ventillo.Interfaces;
using Ventillo.Debug;

namespace Ventillo
{
    internal class CoRoutine
    {
        public string type { get; set; }
        public IEnumerator fn { get; set; }
        public CoRoutine(string type, IEnumerator fn)
        {
            this.type = type;
            this.fn = fn;
        }

    }
    public class Engine
    {
        static internal RenderWindow window;
        static public List<string> keys = new List<string>();
        static public List<Vector> mouseClickPositions = new List<Vector>();
        static public double fps = 0;
        static List<CoRoutine> coroutines = new List<CoRoutine>();
        static public MinMax playableArea;
        static public List<IGame> scenes = new List<IGame>();
        static public Logger logger;
        bool debug = false;
        bool runGame = false;
        DebugObject debugObject;
        IGame game;
        uint FPSLimit = 0;

        public Engine(uint FPSLimit)
        {
            this.FPSLimit = FPSLimit;

            this.SetupEngine();
            this.SetupEvents();
            this.debugObject = new DebugObject(new Vector(0, 0), playableArea);
        }

        public void SetGame(IGame game)
        {
            this.runGame = true;
            this.game = game;
        }

        public void Start()
        {
            logger.Info("Engine started", new { });

            this.StartInternalCoRoutine(this.CheckForNextScene());
            this.StartInternalCoRoutine(this.CheckForNullObjects());

            this.GameLoop();
        }

        public void Stop()
        {
            this.runGame = false;
            for (var CoRoutineIndex = 0; CoRoutineIndex < coroutines.Count; CoRoutineIndex++)
            {
                var coRoutine = coroutines.ElementAt(CoRoutineIndex);

                if (coRoutine.type == "Game")
                {
                    coroutines.Remove(coRoutine);
                }
            }

            keys.Clear();

            this.game = null;
        }

        static public void SwitchScene(IGame game)
        {
            scenes.Add(game);
        }

        void StartInternalCoRoutine(IEnumerator coRoutine)
        {
            coroutines.Add(new CoRoutine("Engine", coRoutine));
        }

        void StartCoRoutine(IEnumerator coRoutine)
        {
            coroutines.Add(new CoRoutine("Game", coRoutine));
        }

        static public double GetWindowWidth()
        {
            return window.Size.X;
        }

        static public double GetWindowHeight()
        {
            return window.Size.Y;
        }

        static IEnumerator InternalWaitForSeconds(double seconds)
        {
            var secsInMilli = seconds * 1000;
            var now = new DateTime().GetTimeInMilliseconds();

            while (new DateTime().GetTimeInMilliseconds() - now < secsInMilli)
            {
                yield return null;
            }
        }

        static IEnumerator WaitForSeconds(double seconds)
        {
            var wait = InternalWaitForSeconds(seconds);
            while (wait.MoveNext()) { }
            yield return null;
        }

        IEnumerator CheckForNextScene()
        {
            while (true)
            {
                if (scenes.Count > 0)
                {
                    this.Stop();
                    this.SetGame(scenes.Last());
                    scenes.Remove(scenes.Last());
                }

                yield return null;
            }
        }

        IEnumerator CheckForNullObjects()
        {
            while (true)
            {
                for (var gameObjectIndex = 0; gameObjectIndex < this.game.gameObjects.Count; gameObjectIndex++)
                {
                    var gameObject = this.game.gameObjects.ElementAt(gameObjectIndex);

                    if (gameObject == null)
                    {
                        this.game.gameObjects.Remove(gameObject);
                    }
                }
                yield return null;
            }
        }

        void Draw()
        {
            this.ClearScreen();
            this.game.Draw();
            this.DrawDebug();
        }

        void DrawDebug()
        {
            if (this.debug)
            {
                for (var gameObjectIndex = 0; gameObjectIndex < this.game.gameObjects.Count; gameObjectIndex++)
                {
                    var gameObject = this.game.gameObjects.ElementAt(gameObjectIndex);
                    this.debugObject.DrawObjectBounds(gameObject);
                }
                this.debugObject.Draw();
            }
        }

        void Update()
        {
            this.game.Update();

            this.CheckDebug();

            this.debugObject.UpdateFPS(new DateTime().GetTimeInMilliseconds());

            keys.Clear();

            mouseClickPositions.Clear();
        }

        void ExecuteCoRoutines()
        {
            for (var CoRoutineIndex = 0; CoRoutineIndex < coroutines.Count; CoRoutineIndex++)
            {
                var coRoutine = coroutines.ElementAt(CoRoutineIndex);

                if (!coRoutine.fn.MoveNext())
                {
                    coroutines.Remove(coRoutine);
                }
            }
        }

        void GameLoop()
        {
            while (window.IsOpen)
            {
                if (this.runGame)
                {
                    window.DispatchEvents();

                    this.Update();
                    this.ExecuteCoRoutines();
                    this.Draw();

                    window.Display();
                }
            }
        }

        void CheckDebug()
        {
            for (var keyIndex = 0; keyIndex < keys.Count; keyIndex++)
            {
                var key = keys.ElementAt(keyIndex);

                switch (key)
                {
                    case "Q":
                        this.debug = !this.debug;
                        keys.Remove(key);
                        break;
                }
            }
        }

        void SetupEvents()
        {
            window.Closed += (object sender, EventArgs e) =>
            {
                window.Close();
                this.runGame = false;
            };

            window.KeyPressed += (object sender, KeyEventArgs e) =>
            {
                keys.Add(e.Code.ToString());
            };

            window.MouseButtonPressed += (object sender, MouseButtonEventArgs e) =>
            {
                mouseClickPositions.Add(new Vector(e.X, e.Y));
            };
        }

        void SetupEngine()
        {
            window = new RenderWindow(new VideoMode(1280, 720), "Ventillo Engine");

            window.SetFramerateLimit(this.FPSLimit);

            logger = new Logger(LoggerLevels.DEBUG);

            playableArea = new MinMax(
                new Vector(GetWindowWidth() * 0.15, GetWindowHeight() * 0.10),
                new Vector(GetWindowWidth() * 0.85, GetWindowHeight() * 0.9)
            );
        }

        void ClearScreen()
        {
            window.Clear();
        }
    }
}

