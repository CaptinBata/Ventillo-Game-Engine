using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventillo.GameObjects;

namespace Ventillo.Interfaces
{
    interface IGame
    {
        List<GameObject> gameObjects { get; set; }
        void checkDelete();
        void update();
        void draw();
    }
}