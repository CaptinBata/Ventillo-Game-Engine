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
        string type { get; set; }
        IEnumerable fn { get; set; }
        public CoRoutine(string type, IEnumerable fn)
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

        void StartInternalCoRoutine(IEnumerable coRoutine)
        {
            Engine.coroutines.Add(new CoRoutine("Engine", coRoutine));
        }

        void StartCoRoutine(IEnumerable coRoutine)
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
            var now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            while ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - now < secsInMilli)
            {
                yield return null;
            }

            yield return null;
        }

        IEnumerable CheckForNextScene()
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

        IEnumerable CheckForNullObjects()
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
    }
}

