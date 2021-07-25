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
    internal class Engine
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

        public Engine()
        {
            this.SetupEngine();
            this.SetupEvents();
            this.debugObject = new DebugObject(new Vector(0, 0), Engine.playableArea);
        }

        public void SetGame(IGame game)
        {
            this.runGame = true;
            this.game = game;
        }

        public void Start()
        {
            Engine.logger.Info("Engine started", new { });

            this.StartInternalCoRoutine(this.CheckForNextScene());
            this.StartInternalCoRoutine(this.CheckForNullObjects());

            this.GameLoop();
        }

        public void Stop()
        {
            this.runGame = false;
            foreach (var coRoutine in Engine.coroutines)
            {
                if (coRoutine.type == "Game")
                {
                    Engine.coroutines.Remove(coRoutine);
                }
            }

            Engine.keys.Clear();

            this.game = null;
        }

        static public void SwitchScene(IGame game)
        {
            Engine.scenes.Add(game);
        }

        void StartInternalCoRoutine(IEnumerator coRoutine)
        {
            Engine.coroutines.Add(new CoRoutine("Engine", coRoutine));
        }

        void StartCoRoutine(IEnumerator coRoutine)
        {
            Engine.coroutines.Add(new CoRoutine("Game", coRoutine));
        }

        static public double GetWindowWidth()
        {
            return Engine.window.Size.X;
        }

        static public double GetWindowHeight()
        {
            return Engine.window.Size.Y;
        }

        static IEnumerable WaitForSeconds(double seconds)
        {
            var secsInMilli = seconds * 1000;
            var now = new DateTime().GetTimeInMilliseconds();

            while ((new DateTime().GetTimeInMilliseconds()) - now < secsInMilli)
            {
                yield return null;
            }

            yield return null;
        }

        IEnumerator CheckForNextScene()
        {
            while (true)
            {
                if (Engine.scenes.Count > 0)
                {
                    this.Stop();
                    this.SetGame(Engine.scenes.Last());
                    Engine.scenes.Remove(Engine.scenes.Last());
                }

                yield return null;
            }
        }

        IEnumerator CheckForNullObjects()
        {
            while (true)
            {
                foreach (var gameObject in this.game.gameObjects)
                {
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
            this.game.draw();
            this.DrawDebug();
        }

        void DrawDebug()
        {
            if (this.debug)
            {
                foreach (var gameObject in this.game.gameObjects)
                {
                    this.debugObject.DrawObjectBounds(gameObject);
                }
                this.debugObject.Draw();
            }
        }

        void Update()
        {
            this.game.update();

            this.CheckDebug();

            this.debugObject.UpdateFPS(new DateTime().GetTimeInMilliseconds());

            Engine.keys.Clear();

            Engine.mouseClickPositions.Clear();
        }

        void ExecuteCoRoutines()
        {
            foreach (var coRoutine in Engine.coroutines)
            {
                if (!coRoutine.fn.MoveNext())
                {
                    Engine.coroutines.Remove(coRoutine);
                }
            }
        }

        void GameLoop()
        {
            while (Engine.window.IsOpen)
            {
                if (this.runGame)
                {
                    Engine.window.DispatchEvents();
                    this.Update();
                    this.ExecuteCoRoutines();
                    this.Draw();
                }
            }
        }

        void CheckDebug()
        {
            foreach (var key in Engine.keys)
            {
                switch (key)
                {
                    case "q":
                        this.debug = !this.debug;
                        Engine.keys.Remove(key);
                        break;
                }
            }
        }

        void SetupEvents()
        {
            Engine.window.Closed += (object sender, EventArgs e) =>
            {
                Engine.window.Close();
                this.runGame = false;
            };

            Engine.window.KeyPressed += (object sender, KeyEventArgs e) =>
            {
                Engine.keys.Add(e.Code.ToString());
            };

            Engine.window.MouseButtonPressed += (object sender, MouseButtonEventArgs e) =>
            {
                Engine.mouseClickPositions.Add(new Vector(e.X, e.Y));
            };
        }

        void SetupEngine()
        {
            Engine.playableArea = new MinMax(
                new Vector(Engine.GetWindowWidth() * 0.15, Engine.GetWindowHeight() * 0.10),
                new Vector(Engine.GetWindowWidth() * 0.85, Engine.GetWindowHeight() * 0.9)
            );
        }

        void ClearScreen()
        {
            Engine.window.Clear();
        }
    }
}

