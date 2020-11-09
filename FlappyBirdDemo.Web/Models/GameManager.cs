using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlappyBirdDemo.Web.Models
{
    public class GameManager
    {
        private readonly int _gravity = 2;

        public event EventHandler MainLoopCompleted;

        public BirdModel Bird { get; private set; }
        public List<PipeModel> Pipes { get; private set; }
        public bool IsRunning { get; private set; } = false;

        public GameManager()
        {
            Bird = new BirdModel();
            Pipes = new List<PipeModel>();
        }

        public async Task MainLoop()
        {
            IsRunning = true;
            while (IsRunning)
            {
                MoveObjects();
                CheckForCollisions();
                ManagePipes();

                MainLoopCompleted?.Invoke(this, EventArgs.Empty);
                await Task.Delay(20);
            }
        }

        public async Task StartGame()
        {
            if (!IsRunning)
            {
                Bird = new BirdModel();
                Pipes = new List<PipeModel>();
                await MainLoop();
            }
        }

        public void Jump()
        {
            if (IsRunning)
                Bird.Jump();
        }

        public void GameOver() => IsRunning = false;

        private void CheckForCollisions()
        {
            if (Bird.IsOnGround())
                GameOver();

            var centerPipe = Pipes.Where(x => x.IsCentered()).FirstOrDefault();
            // Check if pipe in middle
            if (centerPipe != null)
            {
                // Check for collision with bottom and top pipe
                // 150 is the ground height
                // 45 is height of bird
                var hasHitBottom = Bird.DistanceFromGround < centerPipe.GapBottom - 150;
                var hasHitTop = Bird.DistanceFromGround + 45 > centerPipe.GapTop - 150;

                if (hasHitTop || hasHitBottom)
                    GameOver();
            }

        }

        private void MoveObjects()
        {
            Bird.Fall(_gravity);
            Pipes.ForEach((pipe) => pipe.Move());
        }

        private void ManagePipes()
        {
            if (!Pipes.Any() || Pipes.Last().DistanceFromLeft <= 250)
                Pipes.Add(new PipeModel());

            if (Pipes.First().IsOffScreen())
                Pipes.Remove(Pipes.First());
        }
    }
}