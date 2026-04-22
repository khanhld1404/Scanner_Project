using System.Threading;
namespace Keyence_Device
{
    class Beeper
    {
        public static void Success() {
            // Biến để xử lý âm thanh
            int err;
            Sound_Function.PlayInAppFolder("Success_beep.wav", true, true, out err);
        }

        public static void Success2()
        {
            // Biến để xử lý âm thanh
            int err;
            for (int i = 0; i<2; i++){
               Sound_Function.PlayInAppFolder("Success_beep.wav", true, true, out err);
               Thread.Sleep(200);
            }
        }

        public static void Error() {
            // Biến để xử lý âm thanh
            int err;
            for (int i = 0; i < 3; i++)
            {
                Sound_Function.PlayInAppFolder("Error_beep.wav", true, true, out err);
                Thread.Sleep(200);
            }
        }
    }
}