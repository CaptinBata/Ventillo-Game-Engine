using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventillo.GameObjects;

namespace Ventillo.Interfaces
{
    public class IGame
    {
        public List<GameObject> gameObjects = new List<GameObject>();
        protected virtual void CheckDelete() { }
        public virtual void Update() { }
        public virtual void Draw() { }
    }
}