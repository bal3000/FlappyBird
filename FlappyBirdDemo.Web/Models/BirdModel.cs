namespace FlappyBirdDemo.Web.Models
{
    public class BirdModel
    {
        public int DistanceFromGround { get; private set; } = 100;

        public void Fall(int gravity)
        {
            DistanceFromGround -= gravity;
        }
    }
}