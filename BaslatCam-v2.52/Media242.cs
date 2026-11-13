using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

public partial class StartWindow
{
    private Grid musicControls242;
        private TextBlock musicArtist242, musicElapsed242, musicDuration242;
    private Button musicCentralPlay242, musicShuffle242, musicRepeat242, musicFullscreenButton242;
    private DispatcherTimer musicHide242;
    private Slider musicVolume242;
    private bool musicFullscreen242;
    private Rect musicRestore242;
    private double musicMaxHeight242;
    private string musicTrackTitle242 = "", musicTrackArtist242 = "";
    private readonly Random musicRandom242 = new Random();
    private readonly System.Collections.Generic.Stack<int> musicHistory242 = new System.Collections.Generic.Stack<int>();
    private readonly System.Collections.Generic.HashSet<int> musicVisited242 = new System.Collections.Generic.HashSet<int>();

    private static void SoftMediaSlider242(Slider slider)
    {
        slider.IsMoveToPointEnabled = true;
        slider.MinHeight = 22;
        slider.Template = (ControlTemplate)System.Windows.Markup.XamlReader.Parse(
            "<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' TargetType='Slider'>" +
            "<Grid Background='Transparent'><Track x:Name='PART_Track' Minimum='{TemplateBinding Minimum}' Maximum='{TemplateBinding Maximum}' Value='{Binding Value,RelativeSource={RelativeSource TemplatedParent},Mode=TwoWay}'>" +
            "<Track.DecreaseRepeatButton><RepeatButton Command='{x:Static Slider.DecreaseLarge}'><RepeatButton.Template><ControlTemplate TargetType='RepeatButton'><Border Height='4' CornerRadius='2' Background='#52CBE5'/></ControlTemplate></RepeatButton.Template></RepeatButton></Track.DecreaseRepeatButton>" +
            "<Track.IncreaseRepeatButton><RepeatButton Command='{x:Static Slider.IncreaseLarge}'><RepeatButton.Template><ControlTemplate TargetType='RepeatButton'><Border Height='4' CornerRadius='2' Background='#506C7C'/></ControlTemplate></RepeatButton.Template></RepeatButton></Track.IncreaseRepeatButton>" +
            "<Track.Thumb><Thumb Width='15' Height='15'><Thumb.Template><ControlTemplate TargetType='Thumb'><Ellipse Fill='#C2F5FF' Stroke='#56CAE7' StrokeThickness='2'/></ControlTemplate></Thumb.Template></Thumb></Track.Thumb>" +
            "</Track></Grid></ControlTemplate>");
    }

    private void RevealMusic242()
    {
        if (musicControls242 == null) return;
        if(musicVisual244!=null)musicVisual244.Margin=musicFullscreen242?new Thickness(0):new Thickness(10,56,10,96);
        musicControls242.Visibility = Visibility.Visible;
        musicWindow241.Cursor = Cursors.Arrow;
        musicHide242.Stop();
        if (musicFullscreen242) musicHide242.Start();
    }

    private void ToggleMusicFullscreen242()
    {
        var window = musicWindow241;
        if (window == null) return;
        if (!musicFullscreen242)
        {
            musicRestore242 = new Rect(window.Left, window.Top, window.Width, window.Height);
            musicMaxHeight242 = window.MaxHeight;
            var screen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(window).Handle);
            var source = PresentationSource.FromVisual(window);
            var bounds = ScreenDip220(screen.Bounds, source.CompositionTarget.TransformFromDevice);
            window.MaxHeight = double.PositiveInfinity;
            window.WindowState = WindowState.Normal;
            window.Left = bounds.Left;
            window.Top = bounds.Top;
            window.Width = bounds.Width;
            window.Height = bounds.Height;
            window.Topmost = true;
            musicFullscreen242 = true;
        }
        else
        {
            window.Topmost = false;
            window.Left = musicRestore242.Left;
            window.Top = musicRestore242.Top;
            window.Width = musicRestore242.Width;
            window.Height = musicRestore242.Height;
            window.MaxHeight = musicMaxHeight242;
            musicFullscreen242 = false;
        }
        window.MediaChrome242(musicFullscreen242);
        window.MediaHeader244(false);
        window.Topmost=musicFullscreen242||WorkVisible;
        if(!musicFullscreen242)UpdateMusicDrawing241();
        musicFullscreenButton242.ToolTip = musicFullscreen242 ? "Pencereye dön" : "Tam ekran";
        RevealMusic242();
    }

    private void UpdateMusicWindow242(double position, double length)
    {
        if (musicArtist242 == null) return;
        musicArtist242.Text = string.IsNullOrWhiteSpace(musicTrackArtist242) ? "Yerel medya" : musicTrackArtist242;
        musicLargeTitle241.Text = string.IsNullOrWhiteSpace(musicTrackTitle242) ? musicMessage241 : musicTrackTitle242;
        musicElapsed242.Text = MusicTime241(TimeSpan.FromSeconds(position));
        musicDuration242.Text = MusicTime241(TimeSpan.FromSeconds(Math.Ceiling(length)));

    }

    private int NextMusic242(int direction, int count)
    {
        if (musicIndex241 < 0) return 0;
        if (!music241.Shuffle242) return (musicIndex241 + direction + count) % count;
        if (direction < 0 && musicHistory242.Count > 0) return musicHistory242.Pop();
        if (count < 2) return 0;
        musicHistory242.Push(musicIndex241);
        int next = musicRandom242.Next(count - 1);
        return next >= musicIndex241 ? next + 1 : next;
    }

    private void EndMusic242()
    {
        var list = CurrentMusicList241();
        if (list == null || list.Files.Count == 0) { StopMusic241(); return; }
        if (music241.Repeat242 == "one") { PlayMusic241(musicIndex241); return; }
        int next = musicIndex241 + 1;
        if (music241.Shuffle242)
        {
            var remaining = Enumerable.Range(0, list.Files.Count).Where(i => !musicVisited242.Contains(i)).ToList();
            if (remaining.Count == 0 && music241.Repeat242 == "list")
            {
                musicVisited242.Clear();
                remaining = Enumerable.Range(0, list.Files.Count).Where(i => list.Files.Count == 1 || i != musicIndex241).ToList();
            }
            next = remaining.Count == 0 ? list.Files.Count : remaining[musicRandom242.Next(remaining.Count)];
        }
        if (next >= list.Files.Count && music241.Repeat242 == "list") next = 0;
        if (next < list.Files.Count) PlayMusic241(next);
        else
        {
            StopMusic241();
            musicIndex241 = -1;
            musicVisited242.Clear();
            musicMessage241 = "Liste tamamlandı";
            UpdateMusic241();
        }
    }

    private async void ReadMusicTags242(string path, int generation)
    {
        musicTrackTitle242 = Path.GetFileNameWithoutExtension(path);
        musicTrackArtist242 = "";
        UpdateMusic241();
        var completion = new System.Threading.Tasks.TaskCompletionSource<string[]>();
        var worker = new System.Threading.Thread(() =>
        {
            object shellObject = null, folder = null, item = null;
            try
            {
                shellObject = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"));
                folder = ComCall214(shellObject, "NameSpace", Path.GetDirectoryName(path));
                item = ComCall214(folder, "ParseName", Path.GetFileName(path));
                string title = Convert.ToString(ComCall214(item, "ExtendedProperty", "System.Title"));
                object artist = ComCall214(item, "ExtendedProperty", "System.Music.Artist");
                var array = artist as Array;
                string artists = array == null ? Convert.ToString(artist) :
                    string.Join(", ", array.Cast<object>().Select(Convert.ToString));
                completion.TrySetResult(new[] { title, artists });
            }
            catch { completion.TrySetResult(new[] { "", "" }); }
            finally { Release214(item); Release214(folder); Release214(shellObject); }
        });
        worker.IsBackground = true;
        worker.SetApartmentState(System.Threading.ApartmentState.STA);
        worker.Start();
        var tags = await completion.Task;
        if (generation != musicGeneration241 || exitRequested) return;
        if (!string.IsNullOrWhiteSpace(tags[0])) musicTrackTitle242 = tags[0];
        musicTrackArtist242 = tags[1];
        UpdateMusic241();
    }
}





