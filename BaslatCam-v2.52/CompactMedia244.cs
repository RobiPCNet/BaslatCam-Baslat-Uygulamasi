using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

public partial class StartWindow
{
    private MediaWaveform244 musicSideWave244,musicWindowWave244;
    private TextBlock musicSideTotal244,musicFile244;
    private Popup musicVolumePopup244,musicPlaylistPopup244;
    private DispatcherTimer musicVolumeLeave244;
    private CancellationTokenSource musicWaveCancel244;
    private float[] musicPeaks244=new float[0];
    private Button musicSideShuffle244,musicSideRepeat244,musicMute244,musicWindowMute244;
    private Grid musicVisual244;

    private Button MediaIcon244(string glyph,string name,Action action,double size)
    {
        var button=MusicButton241(glyph,action,name);
        button.FontFamily=new FontFamily("Segoe MDL2 Assets");
        button.FontSize=size*.44;
        button.Width=button.Height=size;
        button.Padding=new Thickness(0);
        button.HorizontalContentAlignment=HorizontalAlignment.Center;
        button.VerticalAlignment=VerticalAlignment.Center;
        button.Template=RoundButton212((int)(size/2));
        button.Background=new SolidColorBrush(Color.FromArgb(60,35,101,132));
        if(glyph=="\uE8B7")button.Content=new System.Windows.Shapes.Path {
            Data=Geometry.Parse("M1,4 L1,2 8,2 11,5 23,5 23,20 1,20 Z M1,7 L23,7"),
            Fill=new LinearGradientBrush(Color.FromRgb(255,217,78),Color.FromRgb(232,165,29),90),
            Stroke=new SolidColorBrush(Color.FromRgb(255,225,115)),StrokeThickness=.7,
            Width=size*.65,Height=size*.54,Stretch=Stretch.Uniform };
        return button;
    }

    private UIElement BuildCompactMusic244()
    {
        EnsureMusic241();
        var body=new StackPanel{Margin=new Thickness(2,0,2,0)};
        var heading=new DockPanel{Height=26};
        var folder=MediaIcon244("","Klasör seç",ChooseMusicFolder241,25);
        DockPanel.SetDock(folder,Dock.Right);heading.Children.Add(folder);
        musicTitle241=DText(musicMessage241,12);
        musicTitle241.TextTrimming=TextTrimming.CharacterEllipsis;
        musicTitle241.VerticalAlignment=VerticalAlignment.Center;
        heading.Children.Add(musicTitle241);
        body.Children.Add(heading);
        musicSideWave244=new MediaWaveform244{Height=40,Peaks=musicPeaks244,Margin=new Thickness(0,3,0,0)};
        musicSideWave244.ToolTip="Seçili dosyanın ses dalgası";
        body.Children.Add(musicSideWave244);
        var timeline=new Grid();
        timeline.ColumnDefinitions.Add(new ColumnDefinition{Width=new GridLength(38)});
        timeline.ColumnDefinitions.Add(new ColumnDefinition());
        timeline.ColumnDefinitions.Add(new ColumnDefinition{Width=new GridLength(38)});
        musicTime241=DText("00:00",10,Muted212);
        musicTime241.VerticalAlignment=VerticalAlignment.Center;
        timeline.Children.Add(musicTime241);
        musicSideSeek243=new Slider{Minimum=0,Maximum=1,IsEnabled=false,Margin=new Thickness(2,0,2,0)};
        SoftMediaSlider242(musicSideSeek243);
        musicSideSeek243.ValueChanged+=(s,e)=>{
            if(!musicSync241&&musicOpened241&&musicPlayer241!=null){
                musicPlayer241.Position=TimeSpan.FromSeconds(musicSideSeek243.Value);UpdateMusic241();
            }
        };
        Grid.SetColumn(musicSideSeek243,1);timeline.Children.Add(musicSideSeek243);
        musicSideTotal244=DText("00:00",10,Muted212);
        musicSideTotal244.TextAlignment=TextAlignment.Right;musicSideTotal244.VerticalAlignment=VerticalAlignment.Center;
        Grid.SetColumn(musicSideTotal244,2);timeline.Children.Add(musicSideTotal244);
        body.Children.Add(timeline);
        var controls=new Grid{Height=42};
        musicMute244=VolumeControl244(false,28);
        musicMute244.HorizontalAlignment=HorizontalAlignment.Left;
        controls.Children.Add(musicMute244);
        var transport=new StackPanel{Orientation=Orientation.Horizontal,HorizontalAlignment=HorizontalAlignment.Center};
        transport.Children.Add(MediaIcon244("","Önceki parça",()=>StepMusic241(-1),30));
        musicPlay241=MediaIcon244("","Çal / duraklat",ToggleMusic241,38);
        AccentMusicButton243(musicPlay241,38);
        transport.Children.Add(musicPlay241);
        transport.Children.Add(MediaIcon244("","Sonraki parça",()=>StepMusic241(1),30));
        musicSideShuffle244=MediaIcon244("","Karıştır",ToggleShuffle244,20);
        musicSideRepeat244=MediaIcon244("","Tekrar",ToggleRepeat244,20);
        transport.Children.Add(musicSideShuffle244);transport.Children.Add(musicSideRepeat244);
        transport.Children.Add(MediaIcon244("","Görüntüye getir",ShowMusic241,20));
        controls.ColumnDefinitions.Add(new ColumnDefinition{Width=GridLength.Auto});
        controls.ColumnDefinitions.Add(new ColumnDefinition());
        controls.ColumnDefinitions.Add(new ColumnDefinition{Width=GridLength.Auto});
        Grid.SetColumn(transport,1);controls.Children.Add(transport);
        Button list=null;
        list=MediaIcon244("","Çalma listesi",()=>ShowPlaylist244(list),28);
        list.HorizontalAlignment=HorizontalAlignment.Right;Grid.SetColumn(list,2);controls.Children.Add(list);
        body.Children.Add(controls);
        UpdateMusic241();
        return body;
    }

    private void ToggleShuffle244()
    {
        music241.Shuffle242=!music241.Shuffle242;
        musicHistory242.Clear();musicVisited242.Clear();
        if(musicIndex241>=0)musicVisited242.Add(musicIndex241);
        SaveMusic241();UpdateMusic241();
    }
    private void ToggleRepeat244()
    {
        music241.Repeat242=music241.Repeat242=="off"?"list":music241.Repeat242=="list"?"one":"off";
        SaveMusic241();UpdateMusic241();
    }
    private Button VolumeControl244(bool floating,double size)
    {
        Button button=null;
        button=MediaIcon244("","Ses · Tıklayınca sustur / aç",()=>{
            music241.Muted244=!music241.Muted244;ApplyVolume244();SaveMusic241();
        },size);
        button.MouseEnter+=(s,e)=>ShowVolume244(button,floating);
        button.MouseLeave+=(s,e)=>{if(musicVolumePopup244!=null&&musicVolumePopup244.PlacementTarget==button&&musicVolumeLeave244!=null)musicVolumeLeave244.Start();};
        return button;
    }
    private void ApplyVolume244()
    {
        if(musicPlayer241!=null)musicPlayer241.Volume=music241.Muted244?0:music241.Volume;
        UpdateMusic241();
    }
    private void ShowVolume244(Button anchor,bool floating)
    {
        if(musicVolumePopup244!=null&&musicVolumePopup244.PlacementTarget==anchor){musicVolumeLeave244.Stop();return;}
        CloseVolume244();
        var volume=new Slider{Minimum=0,Maximum=1,Value=music241.Volume,Orientation=Orientation.Vertical,Height=100,Width=26};
        volume.ToolTip="Ses düzeyi";
        volume.Template=(ControlTemplate)System.Windows.Markup.XamlReader.Parse(
            "<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' TargetType='Slider'><Grid Background='Transparent'><Track x:Name='PART_Track' Orientation='Vertical' Minimum='{TemplateBinding Minimum}' Maximum='{TemplateBinding Maximum}' Value='{Binding Value,RelativeSource={RelativeSource TemplatedParent},Mode=TwoWay}'><Track.DecreaseRepeatButton><RepeatButton Command='{x:Static Slider.DecreaseLarge}'><RepeatButton.Template><ControlTemplate TargetType='RepeatButton'><Border Width='4' CornerRadius='2' Background='#43D9E7'/></ControlTemplate></RepeatButton.Template></RepeatButton></Track.DecreaseRepeatButton><Track.IncreaseRepeatButton><RepeatButton Command='{x:Static Slider.IncreaseLarge}'><RepeatButton.Template><ControlTemplate TargetType='RepeatButton'><Border Width='4' CornerRadius='2' Background='#456373'/></ControlTemplate></RepeatButton.Template></RepeatButton></Track.IncreaseRepeatButton><Track.Thumb><Thumb Width='14' Height='14'><Thumb.Template><ControlTemplate TargetType='Thumb'><Ellipse Fill='#C9F6FF' Stroke='#42BEDC' StrokeThickness='2'/></ControlTemplate></Thumb.Template></Thumb></Track.Thumb></Track></Grid></ControlTemplate>");
        volume.ValueChanged+=(s,e)=>{music241.Volume=volume.Value;music241.Muted244=false;ApplyVolume244();};
        var surface=new Border{Child=volume,Padding=new Thickness(8),CornerRadius=new CornerRadius(12),
            Background=new SolidColorBrush(Color.FromArgb(245,12,38,55)),BorderBrush=new SolidColorBrush(Color.FromArgb(100,69,182,216)),BorderThickness=new Thickness(1)};
        var popup=new Popup{Child=surface,PlacementTarget=anchor,Placement=PlacementMode.Top,AllowsTransparency=true,StaysOpen=true};
        musicVolumePopup244=popup;
        musicVolumeLeave244=new DispatcherTimer{Interval=TimeSpan.FromMilliseconds(240)};
        musicVolumeLeave244.Tick+=(s,e)=>{if(!anchor.IsMouseOver&&!surface.IsMouseOver&&!volume.IsMouseCaptureWithin)CloseVolume244();};
        surface.MouseEnter+=(s,e)=>musicVolumeLeave244.Stop();
        surface.MouseLeave+=(s,e)=>musicVolumeLeave244.Start();
        popup.Closed+=(s,e)=>{
            if(floating)musicVolume242=null;else musicVolume241=null;
            SaveMusic241();
        };
        if(floating)musicVolume242=volume;else musicVolume241=volume;
        popup.IsOpen=true;
    }
    private void CloseVolume244()
    {
        if(musicVolumeLeave244!=null){musicVolumeLeave244.Stop();musicVolumeLeave244=null;}
        var popup=musicVolumePopup244;musicVolumePopup244=null;
        if(popup!=null)popup.IsOpen=false;
    }
    private void ShowPlaylist244(Button anchor)
    {
        if(musicPlaylistPopup244!=null){
            var previous=musicPlaylistPopup244;musicPlaylistPopup244=null;previous.IsOpen=false;
            if(previous.PlacementTarget==anchor)return;
        }
        var panel=new DockPanel{Width=260,Margin=new Thickness(10)};
        var actions=new WrapPanel();
        actions.Children.Add(MusicButton241("Klasör",ChooseMusicFolder241,"Klasörden liste oluştur"));
        actions.Children.Add(MusicButton241("Listeleri düzenle",EditMusicLists241,"Listeleri düzenle"));
        DockPanel.SetDock(actions,Dock.Bottom);panel.Children.Add(actions);
        var combo=new ComboBox{FontFamily=new FontFamily("Segoe UI"),ItemsSource=music241.Lists.Select(l=>l.Name).ToList(),SelectedIndex=music241.Lists.Count==0?-1:music241.Selected,Height=30};
        StyleMusicList243(combo);DockPanel.SetDock(combo,Dock.Top);panel.Children.Add(combo);
        combo.SelectionChanged+=(s,e)=>{if(!musicSync241&&combo.SelectedIndex>=0)SelectMusicList241(combo.SelectedIndex);};
        var queue=new ListBox{FontFamily=new FontFamily("Segoe UI"),Height=230,Margin=new Thickness(0,8,0,8),Background=Brushes.Transparent,Foreground=Brushes.White,BorderThickness=new Thickness(0)};
        queue.MouseDoubleClick+=(s,e)=>{if(queue.SelectedIndex>=0)PlayMusic241(queue.SelectedIndex);};
        panel.Children.Add(queue);
        var surface=new Border{Child=panel,CornerRadius=new CornerRadius(14),BorderThickness=new Thickness(1),
            Background=new SolidColorBrush(Color.FromArgb(248,10,31,47)),BorderBrush=new SolidColorBrush(Color.FromArgb(100,80,184,219))};
        TextElement.SetFontFamily(surface,new FontFamily("Segoe UI"));
        var popup=new Popup{Child=surface,PlacementTarget=anchor,Placement=PlacementMode.Right,AllowsTransparency=true,StaysOpen=false};
        musicPlaylistPopup244=popup;musicLists241=combo;musicQueue241=queue;
        popup.Closed+=(s,e)=>{
            if(musicPlaylistPopup244==popup)musicPlaylistPopup244=null;
            if(musicLists241==combo)musicLists241=null;
            if(musicQueue241==queue)musicQueue241=null;
        };
        popup.IsOpen=true;UpdateMusic241();
    }

    private void CloseSideMediaPopups244()
    {
        if(musicVolumePopup244!=null&&Window.GetWindow(musicVolumePopup244.PlacementTarget)==this)CloseVolume244();
        if(musicPlaylistPopup244!=null&&Window.GetWindow(musicPlaylistPopup244.PlacementTarget)==this)musicPlaylistPopup244.IsOpen=false;
    }

    private void ShowFloatingMusic244()
    {
        EnsureMusic241();
        if(musicWindow241!=null){musicWindow241.Topmost=WorkVisible||musicFullscreen242;musicWindow241.Activate();return;}
        bool video=musicOpened241&&musicPlayer241!=null&&musicPlayer241.HasVideo;
        var window=Dialog("Müzikçalar",Math.Min(820,SystemParameters.WorkArea.Width-32),video?Math.Min(620,SystemParameters.WorkArea.Height-32):300);
        window.Owner=null;window.Tag="media-floating";window.ShowInTaskbar=true;window.Icon=Icon;
        window.Topmost=WorkVisible;
        DependencyPropertyChangedEventHandler visibilityChanged=(s,e)=>{if(musicWindow241==window)window.Topmost=WorkVisible||musicFullscreen242;};
        IsVisibleChanged+=visibilityChanged;
        window.SetGlassChrome230();window.MediaHeader244(false);
        window.SetSurface(Color.FromRgb(18,48,65),.65,SystemParameters.HighContrast);
        musicWindow241=window;
        var root=new Grid{Background=new LinearGradientBrush(Color.FromArgb(70,18,55,75),Color.FromArgb(110,5,21,35),90)};
        root.RowDefinitions.Add(new RowDefinition{Height=GridLength.Auto});
        root.RowDefinitions.Add(new RowDefinition());
        root.RowDefinitions.Add(new RowDefinition{Height=GridLength.Auto});
        var header=new DockPanel{Margin=new Thickness(12,8,10,8),Height=40};
        var close=MediaIcon244("","Görüntüyü kapat",()=>window.Close(),30);close.ToolTip="Görüntüyü kapat";
        DockPanel.SetDock(close,Dock.Right);header.Children.Add(close);
        var folder=MediaIcon244("","Klasör seç",ChooseMusicFolder241,32);
        DockPanel.SetDock(folder,Dock.Right);header.Children.Add(folder);
        musicFile244=DText("",11,Muted212);musicFile244.VerticalAlignment=VerticalAlignment.Center;
        musicFile244.MaxWidth=170;musicFile244.TextTrimming=TextTrimming.CharacterEllipsis;
        DockPanel.SetDock(musicFile244,Dock.Right);header.Children.Add(musicFile244);
        var name=new StackPanel{VerticalAlignment=VerticalAlignment.Center};
        musicLargeTitle241=DText(musicMessage241,17);musicLargeTitle241.TextTrimming=TextTrimming.CharacterEllipsis;
        musicArtist242=DText("Yerel medya",11,Muted212);
        name.Children.Add(musicLargeTitle241);name.Children.Add(musicArtist242);header.Children.Add(name);
        header.MouseLeftButtonDown+=(s,e)=>{if(!musicFullscreen242&&e.ClickCount==1)window.DragMove();};

        var visual=new Grid{Margin=new Thickness(10,0,10,0),MinHeight=130,Background=new SolidColorBrush(Color.FromArgb(50,0,10,20))};
        musicVisual244=visual;
        musicScreen241=new System.Windows.Shapes.Rectangle{IsHitTestVisible=false};visual.Children.Add(musicScreen241);
        musicWindowWave244=new MediaWaveform244{Height=100,Peaks=musicPeaks244,Margin=new Thickness(20),VerticalAlignment=VerticalAlignment.Center};
        visual.Children.Add(musicWindowWave244);
        Grid.SetRowSpan(visual,3);root.Children.Add(visual);
        var footer=new StackPanel{Margin=new Thickness(14,5,14,7)};
        var progress=new Grid();
        progress.ColumnDefinitions.Add(new ColumnDefinition{Width=new GridLength(48)});
        progress.ColumnDefinitions.Add(new ColumnDefinition());
        progress.ColumnDefinitions.Add(new ColumnDefinition{Width=new GridLength(48)});
        musicElapsed242=DText("00:00",11);musicElapsed242.VerticalAlignment=VerticalAlignment.Center;progress.Children.Add(musicElapsed242);
        musicSeek241=new Slider{Minimum=0,Maximum=1,IsEnabled=false,Margin=new Thickness(5,0,5,0)};
        SoftMediaSlider242(musicSeek241);
        musicSeek241.ValueChanged+=(s,e)=>{if(!musicSync241&&musicOpened241&&musicPlayer241!=null){musicPlayer241.Position=TimeSpan.FromSeconds(musicSeek241.Value);UpdateMusic241();}};
        Grid.SetColumn(musicSeek241,1);progress.Children.Add(musicSeek241);
        musicDuration242=DText("00:00",11);musicDuration242.VerticalAlignment=VerticalAlignment.Center;musicDuration242.TextAlignment=TextAlignment.Right;
        Grid.SetColumn(musicDuration242,2);progress.Children.Add(musicDuration242);footer.Children.Add(progress);
        var controls=new Grid{Height=52};
        musicWindowMute244=VolumeControl244(true,32);musicWindowMute244.HorizontalAlignment=HorizontalAlignment.Left;controls.Children.Add(musicWindowMute244);
        var transport=new StackPanel{Orientation=Orientation.Horizontal,HorizontalAlignment=HorizontalAlignment.Center};
        transport.Children.Add(MediaIcon244("","Önceki",()=>StepMusic241(-1),36));
        musicCentralPlay242=MediaIcon244("","Çal / duraklat",ToggleMusic241,46);AccentMusicButton243(musicCentralPlay242,46);
        transport.Children.Add(musicCentralPlay242);
        transport.Children.Add(MediaIcon244("","Sonraki",()=>StepMusic241(1),36));
        musicShuffle242=MediaIcon244("","Karıştır",ToggleShuffle244,24);
        musicRepeat242=MediaIcon244("","Tekrar",ToggleRepeat244,24);
        musicFullscreenButton242=MediaIcon244("","Tam ekran",ToggleMusicFullscreen242,24);
        transport.Children.Add(musicShuffle242);transport.Children.Add(musicRepeat242);transport.Children.Add(musicFullscreenButton242);
        controls.ColumnDefinitions.Add(new ColumnDefinition{Width=GridLength.Auto});
        controls.ColumnDefinitions.Add(new ColumnDefinition());
        controls.ColumnDefinitions.Add(new ColumnDefinition{Width=GridLength.Auto});
        Grid.SetColumn(transport,1);controls.Children.Add(transport);
        Button list=null;list=MediaIcon244("","Çalma listesi",()=>ShowPlaylist244(list),32);
        list.HorizontalAlignment=HorizontalAlignment.Right;Grid.SetColumn(list,2);controls.Children.Add(list);footer.Children.Add(controls);
        musicControls242=new Grid();
        musicControls242.RowDefinitions.Add(new RowDefinition{Height=GridLength.Auto});
        musicControls242.RowDefinitions.Add(new RowDefinition());
        musicControls242.RowDefinitions.Add(new RowDefinition{Height=GridLength.Auto});
        musicControls242.Children.Add(header);Grid.SetRow(footer,2);musicControls242.Children.Add(footer);
        Grid.SetRowSpan(musicControls242,3);root.Children.Add(musicControls242);
        Action visibility=()=>{
            visual.Margin=musicFullscreen242?new Thickness(0):new Thickness(10,56,10,96);
        };
        visibility();
        musicControls242.IsVisibleChanged+=(s,e)=>visibility();
        musicHide242=new DispatcherTimer{Interval=TimeSpan.FromSeconds(2.5)};
        musicHide242.Tick+=(s,e)=>{
            if(!musicFullscreen242||root.IsMouseCaptureWithin||openMenus>0||musicPlaylistPopup244!=null||musicVolumePopup244!=null){RevealMusic242();return;}
            musicControls242.Visibility=Visibility.Collapsed;visibility();window.Cursor=Cursors.None;musicHide242.Stop();
        };
        window.Content=root;
        window.MouseMove+=(s,e)=>RevealMusic242();
        window.PreviewMouseDown+=(s,e)=>{if(musicControls242.Visibility!=Visibility.Visible){e.Handled=true;RevealMusic242();}};
        window.PreviewKeyDown+=(s,e)=>{
            if(e.Key==Key.Escape){e.Handled=true;window.Close();}
            else if(e.Key==Key.F11){e.Handled=true;ToggleMusicFullscreen242();}
            else if(e.Key==Key.Space){e.Handled=true;ToggleMusic241();}
        };
        window.Closed+=(s,e)=>{
            IsVisibleChanged-=visibilityChanged;
            if(musicVolumePopup244!=null&&Window.GetWindow(musicVolumePopup244.PlacementTarget)==window)CloseVolume244();
            if(musicPlaylistPopup244!=null&&Window.GetWindow(musicPlaylistPopup244.PlacementTarget)==window)musicPlaylistPopup244.IsOpen=false;
            musicHide242.Stop();musicHide242=null;musicScreen241.Fill=null;
            musicScreen241=null;musicWindow241=null;musicSeek241=null;musicVolume242=null;
            musicLargeTitle241=null;musicArtist242=musicElapsed242=musicDuration242=musicFile244=null;
            musicControls242=null;musicWindowWave244=null;musicVisual244=null;
            musicCentralPlay242=musicShuffle242=musicRepeat242=musicFullscreenButton242=musicWindowMute244=null;
            musicFullscreen242=false;SyncMusicTimer241();
        };
        window.Show();UpdateMusicDrawing241();UpdateMusic241();SyncMusicTimer241();
    }

    private async void ReadWave244(string path,int generation,double seconds)
    {
        CancelWave244();
        musicPeaks244=new float[0];UpdateWave244(0);
        var cancel=new CancellationTokenSource();musicWaveCancel244=cancel;
        var token=cancel.Token;
        var peaks=await System.Threading.Tasks.Task.Run(()=>AudioWave244.Decode(path,seconds,token));
        if(!token.IsCancellationRequested&&generation==musicGeneration241&&!exitRequested){
            musicPeaks244=peaks;UpdateMusic241();
        }
        if(musicWaveCancel244==cancel)musicWaveCancel244=null;
        cancel.Dispose();
    }
    private void CancelWave244(){if(musicWaveCancel244!=null){musicWaveCancel244.Cancel();musicWaveCancel244=null;}}
    private void UpdateWave244(double progress)
    {
        foreach(var wave in new[]{musicSideWave244,musicWindowWave244})if(wave!=null){
            wave.Peaks=musicPeaks244;wave.Progress=progress;wave.InvalidateVisual();
        }
    }
    private void UpdateCompact244(double position,double length)
    {
        string glyph=musicPlaying241?"":"";
        if(musicPlay241!=null)musicPlay241.Content=glyph;
        if(musicCentralPlay242!=null)musicCentralPlay242.Content=glyph;
        if(musicTime241!=null)musicTime241.Text=MusicTime241(TimeSpan.FromSeconds(position));
        if(musicSideTotal244!=null)musicSideTotal244.Text=MusicTime241(TimeSpan.FromSeconds(Math.Ceiling(length)));
        if(musicTitle241!=null){
            musicTitle241.Text=string.IsNullOrWhiteSpace(musicTrackTitle242)?musicMessage241:musicTrackTitle242;
            musicTitle241.ToolTip=musicTrackArtist242;
        }
        if(musicFile244!=null){
            var list=CurrentMusicList241();
            musicFile244.Text=list!=null&&musicIndex241>=0&&musicIndex241<list.Files.Count?Path.GetFileName(list.Files[musicIndex241]):"";
        }
        foreach(var button in new[]{musicMute244,musicWindowMute244})if(button!=null)
            button.Content=music241.Muted244||music241.Volume==0?"":"";
        foreach(var button in new[]{musicSideShuffle244,musicShuffle242})if(button!=null)
            button.Foreground=music241.Shuffle242?Cyan212:Brushes.White;
        foreach(var button in new[]{musicSideRepeat244,musicRepeat242})if(button!=null){
            button.Foreground=music241.Repeat242=="off"?Brushes.White:Cyan212;
            button.ToolTip=music241.Repeat242=="one"?"Tek parça tekrar":music241.Repeat242=="list"?"Liste tekrarı":"Tekrar kapalı";
        }
        if(musicWindowWave244!=null)musicWindowWave244.Visibility=musicOpened241&&musicPlayer241!=null&&musicPlayer241.HasVideo?Visibility.Collapsed:Visibility.Visible;
        UpdateWave244(length>0?position/length:0);
    }
}

