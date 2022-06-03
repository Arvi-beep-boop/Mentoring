using System;
using System.Collections.Generic;

namespace SpaceInvaders
{
    /*
     * classes:
     * - Charcter
     * - Player : Character
     * - Enemy : Character
     * 
     * Features:
     * - movement
     * - shooting
     * - HP
     * 
     */

    /*
     *       x x x x
     *       x x x x
     * 
     *          !
     *          U
     */

    class GameInput
    {
        public float AxisX = 0.0f;
        public bool Shoot = false;

        public void Update()
        {
            AxisX = 0.0f;
            Shoot = false;

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        AxisX = -1.0f;
                        break;
                    case ConsoleKey.RightArrow:
                        AxisX = 1.0f;
                        break;
                    case ConsoleKey.UpArrow:
                        Shoot = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    abstract class GameObject
    {
        public float Health = 100.0f;

        public float Position_X = 0.0f;
        public float Position_Y = 0.0f;

        public String Tag = "";

        public abstract void Update(GameWorld World, float dt);
        public abstract void Draw();
    }

    class Player : GameObject
    {
        public override void Update(GameWorld World, float dt)
        {
            Position_X += World.Input.AxisX * 1.0f;

            if (World.Input.Shoot)
            {
                Projectile projectile = new Projectile();
                projectile.Position_X = Position_X;
                projectile.Position_Y = Position_Y - 1.0f;
                World.Spawning.Add(projectile);
            }
        }

        public override void Draw()
        {
            if (0.0f <= Position_X && Position_X < Console.BufferWidth
                && 0.0f <= Position_Y && Position_Y < Console.BufferHeight)
            {
                Console.SetCursorPosition((int)Position_X, (int)Position_Y);
                Console.Write("U");
            }
        }
    }

    class Enemy : GameObject
    {
        float Direction = 1.0f;
        float Cooldown = 5.0f;

        public override void Update(GameWorld World, float dt)
        {
            Position_X += Direction * dt;
            Cooldown -= dt;

            if (Cooldown <= 0.0f)
            {
                Cooldown = 5.0f;
                Direction *= -1.0f;
            }
        }

        public override void Draw()
        {
            if (0.0f <= Position_X && Position_X < Console.BufferWidth
                && 0.0f <= Position_Y && Position_Y < Console.BufferHeight)
            {
                Console.SetCursorPosition((int)Position_X, (int)Position_Y);
                Console.Write("x");
            }
        }
    }

    class Projectile : GameObject
    {
        public override void Update(GameWorld World, float dt)
        {
            Position_Y -= 4.0f * dt;

            List<GameObject> objs = World.FindGameObjectsInRadius(Position_X, Position_Y, 2.0f);
            foreach (GameObject obj in objs)
            {
                if (obj.Tag == "enemy")
                {
                    World.Destroying.Add(obj);
                    World.Destroying.Add(this);
                }
            }
        }

        public override void Draw()
        {
            if (0.0f <= Position_X && Position_X < Console.BufferWidth
                && 0.0f <= Position_Y && Position_Y < Console.BufferHeight)
            {
                Console.SetCursorPosition((int)Position_X, (int)Position_Y);
                Console.Write("!");
            }
        }
    }

    class GameWorld
    {
        public List<GameObject> GameObjects = new List<GameObject>();
        public GameInput Input = new GameInput();

        public List<GameObject> Spawning = new List<GameObject>();
        public List<GameObject> Destroying = new List<GameObject>();

        public void Update(float dt)
        {
            Input.Update();

            foreach (GameObject obj in Spawning)
            {
                GameObjects.Add(obj);
            }
            Spawning.Clear();

            foreach (GameObject obj in Destroying)
            {
                GameObjects.Remove(obj);
            }
            Destroying.Clear();

            foreach (GameObject obj in GameObjects)
            {
                obj.Update(this, dt);
            }
        }

        public void Draw()
        {
            foreach (GameObject obj in GameObjects)
            {
                obj.Draw();
            }
        }

        public List<GameObject> FindGameObjectsInRadius(float x, float y, float radius)
        {
            List<GameObject> result = new List<GameObject>();

            foreach (GameObject obj in GameObjects)
            {
                if (MathF.Abs(obj.Position_X - x) < radius && MathF.Abs(obj.Position_Y - y) < radius)
                {
                    result.Add(obj);
                }
            }

            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Enemy enemy = new Enemy();
            enemy.Tag = "enemy";

            Player player = new Player();
            player.Position_X = 10.0f;
            player.Position_Y = 20.0f;

            GameWorld world = new GameWorld();
            world.GameObjects.Add(enemy);
            world.GameObjects.Add(player);

            // main loop
            while (true)
            {
                float dt = 0.3f;

                System.Threading.Thread.Sleep((int)(1000.0f * dt));

                // update world
                world.Update(dt);

                // draw world
                Console.Clear();

                world.Draw();

                Console.SetCursorPosition(0, 0);
            }
        }
    }
}
