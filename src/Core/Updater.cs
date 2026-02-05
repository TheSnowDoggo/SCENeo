using SCENeo.Ui;
using System.Diagnostics;

namespace SCENeo;

/// <summary>
/// A class for receiving delta timed updates with useful statistics and frame capping.
/// </summary>
public sealed class Updater
{
    public const double Uncapped = double.PositiveInfinity;

    private bool _active = false;

    private double _frameCap = Uncapped;

    private double _minimumDelta = -1;

    public Updater()
    {
    }

    public Updater(Action<double> updated)
    {
        Updated = updated;
    }

    /// <summary>
    /// The action fired on each update/frame with delta time.
    /// </summary>
    public Action<double> Updated;

    /// <summary>
    /// Gets or sets the frequency in seconds that <see cref="FPS"/> should update.
    /// </summary>
    public double FPSUpdatePeriod { get; set; } = 1.0;

    /// <summary>
    /// Gets the current delta time in seconds.
    /// </summary>
    public double Delta { get; private set; }

    /// <summary>
    /// Gets the actual time taken to update the last frame in seconds (excluding frame capping).
    /// </summary>
    /// <remarks>
    /// Note this is only useful for debugging.
    /// </remarks>
    public double RealDelta { get; private set; }

    /// <summary>
    /// Gets the current FPS.
    /// </summary>
    /// <remarks>
    /// Note this is not equivalent to the inverse of <see cref="Delta"/> as it's an average recording over <see cref="FPSUpdatePeriod"/>.
    /// </remarks>
    public double FPS { get; private set; }

    /// <summary>
    /// Gets or sets the maximum allowed frame updates per second.
    /// </summary>
    /// <remarks>
    /// Set to <see cref="double.PositiveInfinity"/> by default.
    /// </remarks>
    public double FrameCap
    {
        get { return _frameCap; }
        set
        {
            _frameCap     = value;
            _minimumDelta = 1.0 / _frameCap;
        }
    }

    /// <summary>
    /// Starts the update loop if not currently running.
    /// </summary>
    public void Start()
    {
        if (_active)
        {
            return;
        }

        var deltaTimer = Stopwatch.StartNew();
        var fpsTimer = Stopwatch.StartNew();
        var realTimer = Stopwatch.StartNew();

        int frameCount = 0;

        _active = true;

        while (_active)
        {
            realTimer.Restart();

            Updated?.Invoke(Delta);

            RealDelta = realTimer.Elapsed.TotalSeconds;

            while (deltaTimer.Elapsed.TotalSeconds < _minimumDelta) { }

            Delta = deltaTimer.Elapsed.TotalSeconds;

            deltaTimer.Restart();

            frameCount++;

            if (fpsTimer.Elapsed.TotalSeconds >= FPSUpdatePeriod)
            {
                FPS = frameCount / fpsTimer.Elapsed.TotalSeconds;

                frameCount = 0;

                fpsTimer.Restart();
            }
        }
    }

    /// <summary>
    /// Stops the update loop on the next frame.
    /// </summary>
    public void Stop()
    {
        _active = false;
    }
}