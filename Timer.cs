namespace ProtoSim.DotNetUtilities {
    /// <summary>
    /// Provides extension methods for the <see cref="System.Timers.Timer"/> Type, adding functionality.
    /// </summary>
    public static class Timer {
        /// <summary>
        /// Restarts the given <paramref name="timer"/>.
        /// </summary>
        /// <remarks>Calls the <c>Stop()</c> and <c>Start()</c> methods on the <see cref="System.Timers.Timer"/>.</remarks>
        public static void Restart(this System.Timers.Timer? timer) {
            if (timer == null)
                return;

            timer.Stop();
            timer.Start();
        }
    }
}