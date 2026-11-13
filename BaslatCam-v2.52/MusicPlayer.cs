using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

public sealed class MusicList241
{
    public string Name { get; set; }
    public List<string> Files = new List<string>();

    public MusicList241() { Name = "Çalma listesi"; }
}

public sealed class MusicState241
{
    public List<MusicList241> Lists = new List<MusicList241>();
    public int Selected;
    public double Volume = .5;
    public bool Shuffle242;
    public bool Muted244;
    public bool IncludeSubfolders246 = true;
    public string Repeat242 = "off";
}

public partial class StartWindow
{
    private MusicState241 music241;
    private MediaPlayer musicPlayer241;
    private int musicGeneration241, musicIndex241 = -1, musicScan241;
    private bool musicPlaying241, musicOpened241, musicSync241;
    private string musicMessage241 = "Klasör seçerek başlayın";
    private TextBlock musicTitle241, musicTime241, musicLargeTitle241;
    private Button musicPlay241;
    private ComboBox musicLists241;
    private DispatcherTimer musicTimer241;
    private ThemedWindow musicWindow241;
    private System.Windows.Shapes.Rectangle musicScreen241;
    private Slider musicSeek241;
    private Slider musicVolume241;
    private ListBox musicQueue241;
    private Action<string, string> locationProbe241;
    private Action<bool> startupIntegrationProbe241;

    private void EnsureMusic241()
    {
        if (music241 != null) return;
        try {
            string path=OwnPath15("muzik.json");
            if(!File.Exists(path)){
                var previous=MigrationCandidates15().Select(dir=>Path.Combine(dir,"muzik.json")).FirstOrDefault(File.Exists);
                if(previous!=null){RequireOwnWrite15(path);File.Copy(previous,path);}
            }
            music241 = json.Deserialize<MusicState241>(File.ReadAllText(path));
        }
        catch { music241 = new MusicState241(); }
        if (music241 == null) music241 = new MusicState241();
        if (music241.Lists == null) music241.Lists = new List<MusicList241>();
        music241.Lists = music241.Lists.Where(l => l != null).ToList();
        foreach (var list in music241.Lists)
        {
            list.Name = string.IsNullOrWhiteSpace(list.Name) ? "Çalma listesi" : list.Name;
            list.Files = (list.Files ?? new List<string>()).Where(SupportedMusic241)
                .Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }
        if(!new[]{"off","list","one"}.Contains(music241.Repeat242))music241.Repeat242="off";
        music241.Selected = Math.Max(0, Math.Min(music241.Selected, music241.Lists.Count - 1));
        if (double.IsNaN(music241.Volume) || double.IsInfinity(music241.Volume)) music241.Volume = .5;
        music241.Volume = Math.Max(0, Math.Min(1, music241.Volume));
    }

    private static bool SupportedMusic241(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return false;
        return new[] { ".mp3", ".wav", ".wma", ".m4a", ".aac", ".flac", ".mp4", ".wmv" }
            .Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase);
    }

    private MusicList241 CurrentMusicList241()
    {
        return music241.Lists.Count == 0 ? null : music241.Lists[music241.Selected];
    }

    private void SaveMusic241()
    {
        string path = OwnPath15("muzik.json");
        string temporary = OwnPath15("muzik-yeni.json");
        try
        {
            RequireOwnWrite15(path);
            RequireOwnWrite15(temporary);
            File.WriteAllText(temporary, json.Serialize(music241));
            if (File.Exists(path)) File.Replace(temporary, path, null);
            else File.Move(temporary, path);
        }
        catch (Exception ex) { musicMessage241 = "Liste kaydedilemedi · " + ex.Message; }
    }

    private TextBlock musicSideArtist243;
    private Slider musicSideSeek243;

    private UIElement BuildMusic241(){return BuildCompactMusic244();}

    private void AccentMusicButton243(Button button, double size)
    {
        button.Width=button.Height=size;
        button.Template=RoundButton212((int)(size/2));
        button.Background=new LinearGradientBrush(Color.FromRgb(53,190,222),Color.FromRgb(12,108,151),90);
        button.Effect=new System.Windows.Media.Effects.DropShadowEffect {
            BlurRadius=12,ShadowDepth=0,Color=Color.FromRgb(42,181,222),Opacity=.3 };
    }

    private static void StyleMusicList243(ComboBox combo)
    {
        combo.Foreground = Brushes.White;
        ApplyScrollTheme334(combo.Resources);
        combo.Template=(ControlTemplate)System.Windows.Markup.XamlReader.Parse(
            "<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' TargetType='ComboBox'>"+
            "<Grid><ToggleButton Focusable='False' IsChecked='{Binding IsDropDownOpen,RelativeSource={RelativeSource TemplatedParent},Mode=TwoWay}'>"+
            "<ToggleButton.Template><ControlTemplate TargetType='ToggleButton'><Border x:Name='b' CornerRadius='10' Background='#66315569' BorderBrush='#426994A9' BorderThickness='1'><Path Data='M0,0 L4,4 8,0' Stroke='#9EDFEF' StrokeThickness='1.5' HorizontalAlignment='Right' VerticalAlignment='Center' Margin='0,0,12,0'/></Border><ControlTemplate.Triggers><Trigger Property='IsMouseOver' Value='True'><Setter TargetName='b' Property='BorderBrush' Value='#92D7ED'/></Trigger></ControlTemplate.Triggers></ControlTemplate></ToggleButton.Template></ToggleButton>"+
            "<ContentPresenter x:Name='selection' Margin='12,4,30,4' VerticalAlignment='Center' IsHitTestVisible='False' Content='{TemplateBinding SelectionBoxItem}' ContentTemplate='{TemplateBinding SelectionBoxItemTemplate}'/>"+
            "<Popup x:Name='PART_Popup' IsOpen='{TemplateBinding IsDropDownOpen}' Placement='Bottom' AllowsTransparency='True' Focusable='False' PopupAnimation='Fade'><Border MinWidth='{Binding ActualWidth,RelativeSource={RelativeSource TemplatedParent}}' MaxHeight='230' Margin='0,4,0,0' Padding='5' CornerRadius='12' Background='#F00F2C3D' BorderBrush='#557CA6B9' BorderThickness='1'><ScrollViewer CanContentScroll='True' VerticalScrollBarVisibility='Auto'><ItemsPresenter KeyboardNavigation.DirectionalNavigation='Contained'/></ScrollViewer></Border></Popup>"+
            "</Grid><ControlTemplate.Triggers><Trigger Property='HasItems' Value='False'><Setter TargetName='selection' Property='Content' Value='Henüz liste yok'/></Trigger></ControlTemplate.Triggers></ControlTemplate>");
        var style=new Style(typeof(ComboBoxItem));
        style.Setters.Add(new Setter(Control.ForegroundProperty,Brushes.White));
        style.Setters.Add(new Setter(Control.TemplateProperty,System.Windows.Markup.XamlReader.Parse(
            "<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' TargetType='ComboBoxItem'><Border Name='b' CornerRadius='7' Padding='9,6' Background='Transparent'><ContentPresenter/></Border><ControlTemplate.Triggers><Trigger Property='IsHighlighted' Value='True'><Setter TargetName='b' Property='Background' Value='#7040738D'/></Trigger><Trigger Property='IsSelected' Value='True'><Setter TargetName='b' Property='Background' Value='#80317996'/></Trigger></ControlTemplate.Triggers></ControlTemplate>")));
        combo.ItemContainerStyle=style;
    }

    private Button MusicButton241(string text, Action action, string description)
    {
        var button = Btn(text, action);
        button.FontFamily = new FontFamily("Segoe UI");
        button.Padding = new Thickness(8, 4, 8, 4);
        button.Margin = new Thickness(2);
        button.Template = RoundButton212(7);
        button.ToolTip = description;
        System.Windows.Automation.AutomationProperties.SetName(button, description);
        return button;
    }

    private async void ChooseMusicFolder241()
    {
        EnsureMusic241();
        string folder = PickMediaFolder246();
        if (string.IsNullOrEmpty(folder)) return;
        int scan = ++musicScan241;
        if (mediaFolderCancel246 != null) mediaFolderCancel246.Cancel();
        var cancel = new System.Threading.CancellationTokenSource();
        mediaFolderCancel246 = cancel;
        bool recursive = music241.IncludeSubfolders246;
        musicMessage241 = recursive ? "Alt klasörler okunuyor…" : "Klasör okunuyor…";
        UpdateMusic241();
        try {
            var result = await System.Threading.Tasks.Task.Run(() => ReadMediaFolder246(folder, recursive, cancel.Token));
            if (exitRequested || scan != musicScan241) return;
            if (result.Files.Count == 0) musicMessage241 = "Desteklenen ses veya video bulunamadı";
            else { AddMusicList241(new DirectoryInfo(folder).Name, result.Files); musicMessage241 = result.Files.Count + " ses / video eklendi"; }
            if (result.Skipped > 0) musicMessage241 += " · " + result.Skipped + " erişilemeyen klasör atlandı";
        }
        catch (OperationCanceledException) { }
        catch (Exception ex) { if (scan == musicScan241) musicMessage241 = "Klasör okunamadı · " + ex.Message; }
        finally { if (ReferenceEquals(mediaFolderCancel246, cancel)) mediaFolderCancel246 = null; cancel.Dispose(); }
        if (!exitRequested && scan == musicScan241) UpdateMusic241();
    }

    private void AddMusicList241(string name, IEnumerable<string> files)
    {
        var list = new MusicList241 { Name = name, Files = files.Where(SupportedMusic241)
            .Distinct(StringComparer.OrdinalIgnoreCase).ToList() };
        music241.Lists.Add(list);
        SelectMusicList241(music241.Lists.Count - 1);
    }

    private void SelectMusicList241(int index)
    {
        StopMusic241();
        musicTrackTitle242 = musicTrackArtist242 = "";
        musicHistory242.Clear();
        musicVisited242.Clear();
        music241.Selected = index;
        musicIndex241 = -1;
        musicMessage241 = "Liste hazır · Çal'a basın";
        SaveMusic241();
        UpdateMusic241();
    }

    private void EditMusicLists241()
    {
        EnsureMusic241();
        var window = Dialog("Müzik · Çalma listeleri", 640, 570);
        var panel = new DockPanel { Margin = new Thickness(16) };
        var actions = new WrapPanel();
        DockPanel.SetDock(actions, Dock.Bottom);
        panel.Children.Add(actions);
        var list = new ListBox { DisplayMemberPath = "Name", Margin = new Thickness(2, 6, 2, 10) };
        Action refresh = () =>
        {
            list.ItemsSource = null;
            list.ItemsSource = music241.Lists;
            list.SelectedIndex = music241.Lists.Count == 0 ? -1 : music241.Selected;
        };
        actions.Children.Add(MusicButton241("Yeni liste", () =>
        {
            AddMusicList241("Çalma listesi " + (music241.Lists.Count + 1), new string[0]);
            refresh();
        }, "Boş çalma listesi oluştur"));
        actions.Children.Add(MusicButton241("Dosya ekle", () =>
        {
            var selected = list.SelectedItem as MusicList241;
            if (selected == null) return;
            var picker = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = true, Filter = "Ses ve video|*.mp3;*.wav;*.wma;*.m4a;*.aac;*.flac;*.mp4;*.wmv"
            };
            if (picker.ShowDialog(window) != true) return;
            selected.Files = selected.Files.Concat(picker.FileNames).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            SaveMusic241();
            UpdateMusic241();
        }, "Seçili listeye dosya ekle"));
        actions.Children.Add(MusicButton241("Listeyi sil", () =>
        {
            var selected = list.SelectedItem as MusicList241;
            if (selected == null) return;
            StopMusic241();
            music241.Lists.Remove(selected);
            music241.Selected = Math.Max(0, Math.Min(music241.Selected, music241.Lists.Count - 1));
            musicIndex241 = -1;
            musicHistory242.Clear();
            musicVisited242.Clear();
            SaveMusic241();
            refresh();
            UpdateMusic241();
        }, "Seçili çalma listesini kaldır"));
        actions.Children.Add(MusicButton241("Kullan", () =>
        {
            if (list.SelectedIndex >= 0) SelectMusicList241(list.SelectedIndex);
            window.Close();
        }, "Seçili listeyi kullan"));
        var name = new TextBox { Margin = new Thickness(2, 8, 2, 8) };
        MusicList241 editing = null;
        DockPanel.SetDock(name, Dock.Top);
        panel.Children.Add(name);
        list.SelectionChanged += (sender, e) =>
        {
            var selected = list.SelectedItem as MusicList241;
            editing = selected;
            name.Text = selected == null ? "" : selected.Name;
        };
        name.LostKeyboardFocus += (sender, e) =>
        {
            var selected = editing;
            if (selected == null || string.IsNullOrWhiteSpace(name.Text)) return;
            selected.Name = name.Text.Trim();
            SaveMusic241();
            refresh();
            UpdateMusic241();
        };
        panel.Children.Add(list);
        refresh();
        window.Content = panel;
        if(musicWindow241!=null&&musicWindow241.IsActive)window.Owner=musicWindow241;
        window.ShowDialog();
    }

    private void ToggleMusic241()
    {
        EnsureMusic241();
        if (musicPlayer241 == null) { PlayMusic241(musicIndex241 < 0 ? 0 : musicIndex241); return; }
        if (musicPlaying241) { musicPlayer241.Pause(); musicPlaying241 = false; }
        else { musicPlayer241.Play(); musicPlaying241 = true; }
        UpdateMusic241();
        SyncMusicTimer241();
    }

    private void StopMusic241()
    {
        CancelWave244();
        musicGeneration241++;
        musicOpened241 = false;
        musicPlaying241 = false;
        if (musicPlayer241 != null) { musicPlayer241.Close(); musicPlayer241 = null; }
        if (musicScreen241 != null) musicScreen241.Fill = Brushes.Black;
        UpdateMusic241();
        SyncMusicTimer241();
    }

    private void PlayMusic241(int index)
    {
        EnsureMusic241();
        var list = CurrentMusicList241();
        if (list == null || list.Files.Count == 0)
        {
            musicMessage241 = "Önce klasör veya dosya ekleyin";
            UpdateMusic241();
            return;
        }
        StopMusic241();
        musicIndex241 = Math.Max(0, Math.Min(index, list.Files.Count - 1));
        string path = list.Files[musicIndex241];
        musicTrackTitle242 = musicTrackArtist242 = "";
        if (!File.Exists(path))
        {
            musicMessage241 = "Dosya bulunamadı · " + Path.GetFileName(path);
            UpdateMusic241();
            return;
        }
        int generation = musicGeneration241;
        musicVisited242.Add(musicIndex241);
        ReadMusicTags242(path,generation);
        var player = new MediaPlayer { Volume = music241.Muted244?0:music241.Volume };
        musicPlayer241 = player;
        musicMessage241 = "Açılıyor · " + Path.GetFileNameWithoutExtension(path);
        player.MediaOpened += (sender, e) =>
        {
            if (generation != musicGeneration241) return;
            musicOpened241 = true;
            if(player.NaturalDuration.HasTimeSpan)ReadWave244(path,generation,player.NaturalDuration.TimeSpan.TotalSeconds);
            musicMessage241 = Path.GetFileNameWithoutExtension(path);
            UpdateMusicDrawing241();
            UpdateMusic241();
        };
        player.MediaFailed += (sender, e) =>
        {
            if (generation != musicGeneration241) return;
            StopMusic241();
            musicMessage241 = "Dosya oynatılamadı · " + Path.GetFileName(path);
            UpdateMusic241();
        };
        player.MediaEnded += (sender, e) =>
        {
            if (generation != musicGeneration241) return;
            EndMusic242();
        };
        try
        {
            player.Open(new Uri(Path.GetFullPath(path)));
            player.Play();
            musicPlaying241 = true;
            SyncMusicTimer241();
        }
        catch (Exception ex)
        {
            StopMusic241();
            musicMessage241 = "Oynatılamadı · " + ex.Message;
        }
        UpdateMusic241();
    }

    private void StepMusic241(int offset)
    {
        EnsureMusic241();
        var list = CurrentMusicList241();
        if (list == null || list.Files.Count == 0) return;
        int index = NextMusic242(offset,list.Files.Count);
        PlayMusic241(index);
    }

    private void SyncMusicTimer241()
    {
        bool needed = musicPlayer241 != null && musicPlaying241 &&
            (WorkVisible || musicWindow241 != null && musicWindow241.IsVisible);
        if (needed && musicTimer241 == null)
        {
            musicTimer241 = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            musicTimer241.Tick += (sender, e) => UpdateMusic241();
        }
        if (musicTimer241 != null)
        {
            if (needed) musicTimer241.Start();
            else musicTimer241.Stop();
        }
    }

    private static string MusicTime241(TimeSpan value)
    {
        return value.TotalHours >= 1 ? value.ToString(@"h\:mm\:ss") : value.ToString(@"mm\:ss");
    }

    private void UpdateMusic241()
    {
        if (music241 == null || musicSync241) return;
        musicSync241 = true;
        try
        {
            if (musicVolume241 != null) musicVolume241.Value = music241.Volume;
            if (musicVolume242 != null) musicVolume242.Value = music241.Volume;
            if (musicTitle241 != null) musicTitle241.Text = musicMessage241;
            if (musicLargeTitle241 != null) musicLargeTitle241.Text = musicMessage241;
            if (musicPlay241 != null) musicPlay241.Content = musicPlaying241 ? "Ⅱ" : "▶";
            if (musicLists241 != null)
            {
                var names = music241.Lists.Select(l => l.Name).ToList();
                var previous = musicLists241.ItemsSource as List<string>;
                if (previous == null || !previous.SequenceEqual(names)) musicLists241.ItemsSource = names;
                musicLists241.SelectedIndex = names.Count == 0 ? -1 : music241.Selected;
            }
            double length = musicOpened241 && musicPlayer241 != null && musicPlayer241.NaturalDuration.HasTimeSpan
                ? musicPlayer241.NaturalDuration.TimeSpan.TotalSeconds : 0;
            double position = musicOpened241 && musicPlayer241 != null ? musicPlayer241.Position.TotalSeconds : 0;
            if (musicTime241 != null) musicTime241.Text = MusicTime241(TimeSpan.FromSeconds(position)) +
                (length > 0 ? " / " + MusicTime241(TimeSpan.FromSeconds(length)) : "");
            if (musicSeek241 != null && !musicSeek241.IsMouseCaptureWithin)
            {
                musicSeek241.Maximum = Math.Max(1, length);
                musicSeek241.Value = Math.Min(position, musicSeek241.Maximum);
                musicSeek241.IsEnabled = length > 0;
            }
            if(musicSideArtist243!=null)musicSideArtist243.Text=string.IsNullOrWhiteSpace(musicTrackArtist242)?"Yerel medya":musicTrackArtist242;
            if(musicSideSeek243!=null&&!musicSideSeek243.IsMouseCaptureWithin){
                musicSideSeek243.Maximum=Math.Max(1,length);
                musicSideSeek243.Value=Math.Min(position,musicSideSeek243.Maximum);
                musicSideSeek243.IsEnabled=length>0;
            }
            UpdateMusicWindow242(position,length);
            UpdateCompact244(position,length);
            if (musicQueue241 != null)
            {
                var list = CurrentMusicList241();
                var files = list == null ? new List<string>() : list.Files;
                var displayed = musicQueue241.ItemsSource as List<string>;
                var names = files.Select(Path.GetFileNameWithoutExtension).ToList();
                if (displayed == null || !displayed.SequenceEqual(names)) musicQueue241.ItemsSource = names;
                musicQueue241.SelectedIndex = musicIndex241;
            }
        }
        finally { musicSync241 = false; }
    }

    private void UpdateMusicDrawing241()
    {
        if (musicScreen241 == null) return;
        if(musicWindow241!=null&&!musicFullscreen242)
            musicWindow241.Height=musicOpened241&&musicPlayer241!=null&&musicPlayer241.HasVideo?
                Math.Min(620,SystemParameters.WorkArea.Height-32):300;
        if (musicOpened241 && musicPlayer241 != null && musicPlayer241.HasVideo)
        {
            var drawing = new VideoDrawing { Player = musicPlayer241,
                Rect = new Rect(0, 0, musicPlayer241.NaturalVideoWidth, musicPlayer241.NaturalVideoHeight) };
            musicScreen241.Fill = new DrawingBrush(drawing) { Stretch = Stretch.Uniform };
        }
        else musicScreen241.Fill = Brushes.Transparent;
    }

    private void ShowMusic241() { ShowFloatingMusic244(); }

    private void DisposeMusic241()
    {
        CloseVolume244();
        if(musicPlaylistPopup244!=null)musicPlaylistPopup244.IsOpen=false;
        musicScan241++;
        if (mediaFolderCancel246 != null) mediaFolderCancel246.Cancel();
        if (musicWindow241 != null) musicWindow241.Close();
        StopMusic241();
    }

    private void AddLocationMenu241(ContextMenu menu, Tile tile)
    {
        if (tile.Children != null) return;
        var item = new MenuItem { Header = "Konumunu aç" };
        item.Click += (sender, e) => OpenLocation241(tile);
        menu.Items.Insert(1, item);
    }

    private void OpenLocation241(Tile tile)
    {
        try
        {
            string explorer = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe");
            string target = ResolveIconTarget226(tile);
            string arguments;
            if (IsStoreTarget214(target)) arguments = "shell:AppsFolder";
            else if (Directory.Exists(target)) arguments = "\"" + target + "\"";
            else if (File.Exists(target)) arguments = "/select,\"" + target + "\"";
            else { status.Text = "Bu uygulamanın yerel dosya konumu bulunamadı."; return; }
            if (locationProbe241 != null) locationProbe241(explorer, arguments);
            else
            {
#if DIAGNOSTICS
                throw new InvalidOperationException("Tanılamada Explorer açılmaz.");
#else
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(explorer, arguments) { UseShellExecute = true });
#endif
            }
            HideMenu();
        }
        catch (Exception ex) { if (status != null) status.Text = "Konum açılamadı · " + ex.Message; }
    }

}


