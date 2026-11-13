using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

internal abstract class WeatherMotion240 : FrameworkElement
{
    internal virtual bool Finished { get { return false; } }
    internal abstract void Advance(double seconds);
}

internal sealed class SkyMotion240 : WeatherMotion240
{
    private static readonly Rect Frame = new Rect(0, 0, 960, 540);
    private static readonly Geometry SunRidge = ReadRidge("M0,0 H960 V183 L930,184 901,192 866,206 834,214 811,216 785,213 744,210 714,215 692,218 672,216 657,213 635,215 616,214 600,212 583,214 566,218 548,224 529,230 511,235 487,234 465,235 446,235 424,239 405,238 389,240 370,236 350,232 328,230 307,234 284,230 257,233 224,238 195,227 153,218 0,210 Z");
    private static readonly Geometry MoonRidge = ReadRidge("M0,0 H960 V158 L920,175 875,181 835,196 795,219 774,230 740,237 718,245 685,260 658,267 638,265 628,262 610,269 593,277 583,275 571,279 552,272 540,273 525,269 503,261 487,255 474,253 458,246 441,251 429,252 414,249 401,251 386,245 373,241 361,233 350,230 340,223 330,217 318,223 306,220 280,230 258,226 229,227 200,241 0,210 Z");
    private readonly BitmapSource picture;
    private readonly bool moon;
    internal bool HasLandscape { get; private set; }
    private readonly double duration;
    private double elapsed;

    internal SkyMotion240(bool moon, bool hasLandscape, BitmapSource picture, double duration)
    {
        this.moon = moon;
        HasLandscape = hasLandscape;
        this.picture = picture;
        this.duration = duration;
        ClipToBounds = true;
    }

    private static Geometry ReadRidge(string path)
    {
        var geometry = Geometry.Parse(path);
        geometry.Freeze();
        return geometry;
    }

    private static readonly Brush Halo = CreateHalo();

    private static Brush CreateHalo()
    {
        var brush = new RadialGradientBrush();
        brush.GradientStops.Add(new GradientStop(Color.FromArgb(174, 255, 255, 255), .44));
        brush.GradientStops.Add(new GradientStop(Color.FromArgb(86, 216, 240, 255), .60));
        brush.GradientStops.Add(new GradientStop(Color.FromArgb(28, 154, 210, 255), .80));
        brush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 117, 185, 247), 1));
        brush.Freeze();
        return brush;
    }
    internal override bool Finished { get { return elapsed >= duration; } }

    internal override void Advance(double seconds)
    {
        elapsed = Math.Min(duration, elapsed + seconds);
        InvalidateVisual();
    }

    protected override void OnRender(DrawingContext dc)
    {
        if (ActualWidth <= 0 || ActualHeight <= 0) return;
        double scale = Math.Max(ActualWidth / Frame.Width, ActualHeight / Frame.Height);
        dc.PushTransform(new MatrixTransform(scale, 0, 0, scale,
            (ActualWidth - Frame.Width * scale) / 2, (ActualHeight - Frame.Height * scale) / 2));

        double progress = elapsed / duration;
        Point center;
        if (HasLandscape)
        {
            center = moon ? new Point(462 + 90 * progress, 323 - 185 * progress)
                : new Point(470 + 104 * progress, 328 - 196 * progress);
            dc.PushClip(moon ? MoonRidge : SunRidge);
        }
        else
        {
            center = new Point(480 + 72 * progress, 270 - 110 * progress);
        }

        if (moon)
        {
            dc.DrawImage(picture, new Rect(center.X - 76.5, center.Y - 51, 153, 102));
        }
        else
        {
            double visibility = HasLandscape ? Math.Max(0, Math.Min(1, (232 - center.Y) / 42)) : 1;
            if (visibility > 0)
            {
                Vector direction = new Point(710, 467) - center;
                double angle = Math.Atan2(direction.Y, direction.X) * 180 / Math.PI - 37;
                dc.PushOpacity(.55 * .48 * visibility);
                dc.PushTransform(new RotateTransform(angle, center.X, center.Y));
                dc.DrawImage(picture, new Rect(center.X - 19, center.Y - 8, 500, 333.333));
                dc.Pop();
                dc.Pop();
            }
            dc.DrawEllipse(Halo, null, center, 49, 49);
            dc.DrawEllipse(Brushes.White, null, center, 25, 25);
        }

        if (HasLandscape) dc.Pop();
        dc.Pop();
    }
}

internal sealed class RainMotion240 : WeatherMotion240
{
    private const int MaxDrops = 32;
    private readonly Point[] positions = new Point[MaxDrops];
    private readonly double[] durations = new double[MaxDrops];
    private sealed class DropStyle
    {
        internal readonly double Radius, Stretch, Trail, Duration, Acceleration;
        internal readonly Pen Shadow, Reflection;

        internal DropStyle(double radius, double stretch, double trail, double duration, double acceleration)
        {
            Radius = radius;
            Stretch = stretch;
            Trail = trail;
            Duration = duration;
            Acceleration = acceleration;
            Shadow = new Pen(Ink(85, 8, 21, 30), Math.Max(.9, radius * .62));
            Reflection = new Pen(Ink(180, 237, 249, 255), Math.Max(.45, radius * .20));
            Shadow.Freeze();
            Reflection.Freeze();
        }
    }

    private static readonly DropStyle[] Styles =
    {
        new DropStyle(1.4, 1.25, 6, 4.9, 1.45), // İnce, hızlı damla
        new DropStyle(2.7, 1.08, 8, 8.2, 2.15), // Yuvarlak, yavaş damla
        new DropStyle(5.1, 1.48, 13, 6.4, 1.95), // İri damla, geniş iz
        new DropStyle(2.1, 2.05, 16, 6.4, 1.95)  // Uzun iz, iri damlanın yolunu izler
    };
    private readonly double[] offsets = new double[MaxDrops];
    private readonly Brush highlight = Ink(245, 248, 253, 255);
    private readonly Brush shade = Ink(135, 2, 16, 27);
    private readonly Brush body = Ink(75, 205, 230, 242);
    private static readonly double[] EdgeChannels = { .031, .092, .966 };
    private readonly Brush streamShade = Ink(33, 5, 24, 35);
    private readonly Brush streamTint = Ink(23, 190, 229, 249);
    private readonly Pen streamLight = new Pen(Ink(86, 209, 237, 250), .65);

    private DrawingGroup surface;
    private double elapsed;
    internal const int Count = 20;
    private double streamWidth = 1.7;
    internal double StreamOpacity { get; private set; }

    internal RainMotion240()
    {
        StreamOpacity = .73;
        streamLight.Freeze();
        ClipToBounds = true;
        var random = new Random(239);
        for (int i = 0; i < MaxDrops; i++)
        {
            int kind = i % Styles.Length;
            if (kind == 3)
            {
                positions[i] = positions[i - 1];
                durations[i] = durations[i - 1];
                offsets[i] = (offsets[i - 1] + .90) % 1;
            }
            else
            {
                positions[i] = new Point(.07 + random.NextDouble() * .86, random.NextDouble());
                durations[i] = Styles[kind].Duration * (.88 + random.NextDouble() * .24);
                offsets[i] = random.NextDouble();
            }
        }
        SizeChanged += (sender, args) => RebuildSurface();
    }

    private static Brush Ink(byte a, byte r, byte g, byte b)
    {
        var brush = new SolidColorBrush(Color.FromArgb(a, r, g, b));
        brush.Freeze();
        return brush;
    }

    internal override void Advance(double seconds)
    {
        elapsed += seconds;
        InvalidateVisual();
    }

    private void DrawDrop(DrawingContext dc, double x, double y, double radius, double stretch)
    {
        dc.DrawEllipse(shade, null, new Point(x + .45, y + .65), radius, radius * stretch);
        dc.DrawEllipse(body, null, new Point(x, y), radius * .86, radius * stretch * .89);
        dc.DrawEllipse(highlight, null, new Point(x - radius * .33, y - radius * .42), radius * .36, radius * .23);
    }

    private void RebuildSurface()
    {
        if (ActualWidth <= 0 || ActualHeight <= 0) return;
        var layer = new DrawingGroup();
        using (var dc = layer.Open())
        {
            var random = new Random(312);
            int drops = Math.Min(900, (int)(ActualWidth * ActualHeight / 2200));
            for (int i = 0; i < drops; i++)
            {
                double x = random.NextDouble() * ActualWidth;
                double y = random.NextDouble() * ActualHeight;
                DrawDrop(dc, x, y, .9 + random.NextDouble() * 2.7, .8 + random.NextDouble() * .45);
            }
        }
        layer.Freeze();
        surface = layer;
        InvalidateVisual();
    }

    private Point PathPoint(int channel, double y)
    {
        double bend = Math.Sin(y * .015 + channel * 1.6) * 4;
        return new Point(ActualWidth * positions[channel].X + bend, y);
    }

    private double StreamX(int channel, double y)
    {
        double wave = y / 76;
        double phase = elapsed * .85 + channel * 2.1;
        return ActualWidth * EdgeChannels[channel]
            + 6 * Math.Sin(wave - phase)
            + 2.5 * Math.Sin(wave * 1.9 + phase * .55);
    }

    private void DrawEdgeStreams(DrawingContext dc)
    {
        for (int channel = 0; channel < EdgeChannels.Length; channel++)
        {
            double cycle = (elapsed + channel * 2.1) % (6.1 + channel * .7);
            double surgeY = -18 + (ActualHeight + 38) * Math.Pow(Math.Min(1, cycle / 2.4), 1.5);
            bool surging = cycle < 2.4;
            int segments = Math.Max(1, (int)Math.Ceiling(ActualHeight / 7));
            var ribbon = new StreamGeometry();
            var glint = new StreamGeometry();
            var innerFilm = new StreamGeometry();
            var rightEdge = new Point[segments + 1];
            using (var fill = ribbon.Open())
            using (var line = glint.Open())
            using (var film = innerFilm.Open())
            {
                for (int i = 0; i <= segments; i++)
                {
                    double y = i * ActualHeight / segments;
                    double distance = surgeY - y;
                    double swell = surging && distance >= 0
                        ? 3 * Math.Exp(-distance / 29)
                        : 0;
                    double ripple = Math.Sin(y / 19 - elapsed * 1.65 + channel);
                    double halfWidth = (1.45 + .85 * ripple + swell) * streamWidth;
                    double x = StreamX(channel, y);
                    var left = new Point(x - halfWidth, y);
                    rightEdge[i] = new Point(x + halfWidth, y);
                    var wetEdge = new Point(x + halfWidth * .33, y);
                    if (i == 0) film.BeginFigure(wetEdge, true, true);
                    else film.LineTo(wetEdge, true, false);
                    if (i == 0 || ripple < .38) line.BeginFigure(rightEdge[i], false, false);
                    else line.LineTo(rightEdge[i], true, true);
                    if (i == 0)
                    {
                        fill.BeginFigure(left, true, true);
                    }
                    else
                    {
                        fill.LineTo(left, true, false);
                    }
                }
                for (int i = segments; i >= 0; i--)
                {
                    fill.LineTo(rightEdge[i], true, false);
                    film.LineTo(rightEdge[i], true, false);
                }
            }
            dc.DrawGeometry(streamShade, null, ribbon);
            dc.DrawGeometry(streamTint, null, innerFilm);
            dc.DrawGeometry(null, streamLight, glint);
            if (surging)
            {
                dc.PushOpacity(.68);
                DrawDrop(dc, StreamX(channel, surgeY), surgeY, 4.8 * streamWidth, 1.7);
                dc.Pop();
            }
        }
    }

    protected override void OnRender(DrawingContext dc)
    {
        if (surface == null) RebuildSurface();
        if (surface == null) return;
        dc.DrawDrawing(surface);
        if (StreamOpacity > 0)
        {
            dc.PushOpacity(StreamOpacity);
            DrawEdgeStreams(dc);
            dc.Pop();
        }
        for (int i = 0; i < Count; i++)
        {
            int kind = i % Styles.Length;
            var style = Styles[kind];
            int channel = kind == 3 ? i - 1 : i;
            double phase = (elapsed / durations[i] + offsets[i]) % 1;
            double y = ActualHeight * (positions[i].Y * .28 + Math.Pow(phase, style.Acceleration)) - 12;
            double trail = style.Trail * (.30 + .70 * phase);
            var head = PathPoint(channel, y);
            double stretch = Math.Min(1.42, style.Stretch + phase * .12);
            double gap = style.Radius * stretch + 2.5;
            var near = PathPoint(channel, y - gap);
            var far = PathPoint(channel, y - gap - trail);
            double opacity = Math.Min(1, Math.Min(phase * 12, (1 - phase) * 13));
            dc.PushOpacity(opacity);
            dc.PushOpacity(.27);
            dc.DrawLine(style.Shadow, far, near);
            dc.DrawLine(style.Reflection, new Point(far.X - .6, far.Y), new Point(near.X - .6, near.Y));
            dc.Pop();
            DrawDrop(dc, head.X, head.Y, style.Radius, stretch);
            dc.Pop();
        }
    }
}

internal sealed class SnowMotion240 : WeatherMotion240
{
    private readonly Point[] origins = new Point[360];
    private readonly Brush[] flakes =
    {
        Ink(100, 225, 241, 255), Ink(165, 235, 246, 255), Ink(220, 247, 252, 255)
    };
    private readonly Brush halo = Ink(37, 215, 235, 255);
    private double elapsed;
    internal const int Count = 280;

    internal SnowMotion240()
    {
        ClipToBounds = true;
        var random = new Random(371);
        for (int i = 0; i < origins.Length; i++)
            origins[i] = new Point(random.NextDouble(), random.NextDouble());
    }

    private static Brush Ink(byte a, byte r, byte g, byte b)
    {
        var brush = new SolidColorBrush(Color.FromArgb(a, r, g, b));
        brush.Freeze();
        return brush;
    }

    internal override void Advance(double seconds)
    {
        elapsed += seconds;
        InvalidateVisual();
    }

    protected override void OnRender(DrawingContext dc)
    {
        for (int depth = 0; depth < flakes.Length; depth++)
        {
            for (int i = depth; i < Count; i += flakes.Length)
            {
                double radius = .6 + depth * 1.15 + origins[i].X * (.7 + depth * .65);
                double speed = 17 + depth * 18 + origins[i].Y * 14;
                double x = origins[i].X * ActualWidth
                    + Math.Sin(elapsed * (.75 + origins[i].X) + i) * (3 + depth * 4);
                double y = origins[i].Y * ActualHeight + elapsed * speed;
                var point = new Point(Wrap(x, ActualWidth), Wrap(y, ActualHeight));
                if (depth == 2)
                    dc.DrawEllipse(halo, null, point, radius * 1.55, radius * 1.55);
                dc.DrawEllipse(flakes[depth], null, point, radius, radius * .92);
                if(depth==2&&i%4==0){var pen=new Pen(flakes[depth],.8);dc.DrawLine(pen,new Point(point.X-radius*1.4,point.Y),new Point(point.X+radius*1.4,point.Y));dc.DrawLine(pen,new Point(point.X,point.Y-radius*1.4),new Point(point.X,point.Y+radius*1.4));}
            }
        }
    }

    private static double Wrap(double position, double extent)
    {
        double span = extent + 20;
        double wrapped = (position + 10) % span;
        return (wrapped < 0 ? wrapped + span : wrapped) - 10;
    }
}





