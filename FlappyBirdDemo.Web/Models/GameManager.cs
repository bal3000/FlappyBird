using System.ComponentModel;
using System.Threading.Tasks;

namespace FlappyBirdDemo.Web.Models
{
    public class GameManager : INotifyPropertyChanged
    {
        private readonly int _gravity = 2;

        public event PropertyChangedEventHandler PropertyChanged;

        public BirdModel Bird { get; private set; }
        public bool IsRunning { get; private set; } = false;

        public GameManager()
        {
            Bird = new BirdModel();
        }

        public async Task MainLoop()
        {
            IsRunning = true;
            while (IsRunning)
            {
                Bird.Fall(_gravity);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Bird)));

                if (Bird.DistanceFromGround <= 0)
                    GameOver();

                await Task.Delay(20);
            }
        }

        public async Task StartGame()
        {
            if (!IsRunning)
            {
                Bird = new BirdModel();
                await MainLoop();
            }
        }

        public void GameOver()
        {
            IsRunning = false;
        }
    }
}