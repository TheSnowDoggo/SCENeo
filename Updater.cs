using System.Diagnostics;

namespace SCENeo;

internal sealed class Updater
{
    private readonly Thread _thread;

    private bool _active = false;

    private int _fps = 0;

    private int _frameCap = -1;

    private double _minimumDelta = -1;

    private double _delta     = 0;

    private double _realDelta = 0;

    public Updater()
    {
        _thread = new(UpdateLoop);
    }

    public Action<double>? Update;

    public double FPSUpdateRate { get; set; } = 1.0;

    public double Delta     => _delta;

    public double RealDelta => _realDelta;

    public int FPS => _fps;

    public int FrameCap
    {
        get => _frameCap;
        set
        {
            _frameCap     = value;
            _minimumDelta = _frameCap != -1 ? 1.0 / _frameCap : -1;
        }
    }

    public void Start()
    {
        _thread.Start();
    }

    public void Stop()
    {
        _active = false;
    }

    private void UpdateLoop()
    {
        var deltaTimer = Stopwatch.StartNew();
        var fpsTimer   = Stopwatch.StartNew();
        var realTimer  = Stopwatch.StartNew();

        int frameCount = 0;

        _active = true;

        while (_active)
        {
            realTimer.Restart();

            Update?.Invoke(_delta);

            _realDelta = realTimer.Elapsed.TotalSeconds;

            while (deltaTimer.Elapsed.TotalSeconds < _minimumDelta)
            {
            }

            _delta = deltaTimer.Elapsed.TotalSeconds;
            deltaTimer.Restart();

            frameCount++;

            if (fpsTimer.Elapsed.TotalSeconds >= FPSUpdateRate)
            {
                _fps = frameCount;

                frameCount = 0;

                fpsTimer.Restart();
            }
        }
    }
}