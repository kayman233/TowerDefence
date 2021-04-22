using Runtime;

namespace Main
{
    public class LostController: IController
    {
        public void OnStart()
        {
        }

        public void OnStop()
        {
        }

        public void Tick()
        {
            Game.Player.CheckForLose();
        }
    }
}