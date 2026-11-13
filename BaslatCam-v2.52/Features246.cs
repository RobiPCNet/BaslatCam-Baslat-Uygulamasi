using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public sealed class FrequentEntry246 {
    public Tile Item = new Tile();
    public bool Allowed = true, Pinned;
    public long Count, LastUsed;
}
public sealed class GroupGlass246 : StackPanel {
    public Config Theme247;
    protected override void OnRender(DrawingContext dc) {
        base.OnRender(dc);
        if (ActualWidth < 1 || ActualHeight < 38) return;
        if(Theme247!=null&&!Theme247.ShowGroupSurface248)return;
        var fill = new LinearGradientBrush(Color.FromArgb(24,35,127,179), Color.FromArgb(9,2,21,42),90);
        dc.DrawRoundedRectangle(fill, new Pen(new SolidColorBrush(Color.FromArgb(85,49,182,234)),1),new Rect(-5,34,ActualWidth+10,ActualHeight-30),16,16);
    }
}
public partial class StartWindow {
    private CancellationTokenSource mediaFolderCancel246;
    private void FeatureGlass246(ThemedWindow w) { w.SetGlassChrome230();SettingsControls230(w); }
    private Action<Window> featureProbe246;
    private void FeatureProbe246(Window w) { if(featureProbe246!=null)w.ContentRendered+=(s,e)=>w.Dispatcher.BeginInvoke(new Action(()=>featureProbe246(w))); }
    public sealed class MediaScan246 { public List<string> Files = new List<string>(); public int Skipped; }
    public static MediaScan246 ReadMediaFolder246(string folder, bool recursive, CancellationToken token) {
        var result = new MediaScan246();
        var pending = new Stack<string>(); pending.Push(Path.GetFullPath(folder));
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        while (pending.Count > 0) {
            token.ThrowIfCancellationRequested();
            string current = pending.Pop(); if (!visited.Add(current)) continue;
            try {
                result.Files.AddRange(Directory.GetFiles(current).Where(SupportedMusic241));
                if (recursive) foreach (string child in Directory.GetDirectories(current)) {
                    token.ThrowIfCancellationRequested();
                    try { if ((File.GetAttributes(child) & FileAttributes.ReparsePoint) == 0) pending.Push(child); }
                    catch (UnauthorizedAccessException) { result.Skipped++; }
                    catch (IOException) { result.Skipped++; }
                }
            } catch (UnauthorizedAccessException) { if (visited.Count == 1) throw; result.Skipped++; }
            catch (IOException) { if (visited.Count == 1) throw; result.Skipped++; }
        }
        result.Files = result.Files.Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(Path.GetFileName,StringComparer.CurrentCultureIgnoreCase).ThenBy(x=>x,StringComparer.OrdinalIgnoreCase).ToList();
        return result;
    }
    private string PickMediaFolder246() {
        string selected = null; bool accepted = false;
        var w = Dialog("Medya klasörü · Ses ve video",520,300);
        FeatureGlass246(w);
        if (musicWindow241 != null && musicWindow241.IsActive) w.Owner = musicWindow241;
        var content = new StackPanel { Margin = new Thickness(22) };
        var label = new TextBlock { Text="Bir klasör seçin",TextWrapping=TextWrapping.Wrap,Margin=new Thickness(3,8,3,12) };
        content.Children.Add(label);
        content.Children.Add(Btn("Klasör seç…",()=>{
            using(var picker = new System.Windows.Forms.FolderBrowserDialog { Description="Ses ve video klasörü",ShowNewFolderButton=false }) {
                var owner = new System.Windows.Forms.NativeWindow(); openMenus++;
                try { owner.AssignHandle(new System.Windows.Interop.WindowInteropHelper(w).Handle);
                    if (picker.ShowDialog(owner)==System.Windows.Forms.DialogResult.OK) { selected=picker.SelectedPath;label.Text=selected; }
                } finally { owner.ReleaseHandle();openMenus--; }
            }
        }));
        var include = new CheckBox { Foreground=Brushes.White,Content="Tüm alt klasörlerdeki ses ve videoları dahil et",IsChecked=music241.IncludeSubfolders246,Margin=new Thickness(3,12,3,12) };
        content.Children.Add(include);
        var buttons = new StackPanel { Orientation=Orientation.Horizontal,HorizontalAlignment=HorizontalAlignment.Right };
        buttons.Children.Add(Btn("İptal",()=>w.Close()));
        buttons.Children.Add(Btn("Listeye ekle",()=>{ if(string.IsNullOrEmpty(selected)) { label.Text="Önce klasör seçin";return; } music241.IncludeSubfolders246=include.IsChecked==true;SaveMusic241();accepted=true;w.Close(); }));
        content.Children.Add(buttons);w.Content=content;FeatureProbe246(w);w.ShowDialog();return accepted?selected:null;
    }
    private string FrequentKey246(string target) {
        string key=Environment.ExpandEnvironmentVariables((target??"").Trim().Trim('"'));
        try { if(Path.IsPathRooted(key)) key=Path.GetFullPath(key); } catch { }
        return key.TrimEnd('\\','/').ToUpperInvariant();
    }
    private List<FrequentEntry246> FrequentEntries246() {
        if(cfg.Frequent246==null)cfg.Frequent246=new List<FrequentEntry246>();
        return cfg.Frequent246;
    }
    private FrequentEntry246 FindFrequent246(string target) {
        string key=FrequentKey246(target);
        return FrequentEntries246().FirstOrDefault(e=>e!=null&&e.Item!=null&&FrequentKey246(e.Item.Target)==key);
    }
    private void RecordFrequent246(string target) {
        if(!cfg.FrequentEnabled246||string.IsNullOrWhiteSpace(target))return;
        var entry=FindFrequent246(target);
        if(entry==null) {
            Tile original=Leaves(cfg.Tiles).Concat(appRecords.Select(r=>r.Tile)).FirstOrDefault(t=>FrequentKey246(t.Target)==FrequentKey246(target));
            Tile tile=original==null?new Tile {Target=target,Name=Path.GetFileNameWithoutExtension(target)}:Clone(original);
            tile.Children=null;tile.Live=false;
            entry=new FrequentEntry246 { Item=tile };FrequentEntries246().Add(entry);
        }
        if(!entry.Allowed)return;
        entry.Count++;entry.LastUsed=DateTime.UtcNow.Ticks;Save();
    }
    private IEnumerable<FrequentEntry246> VisibleFrequent246(string query) {
        return FrequentEntries246().Where(e=>e!=null&&e.Item!=null&&e.Allowed&&(e.Pinned||e.Count>0)&&((query??"").Length==0||(e.Item.Name??"").IndexOf(query,StringComparison.CurrentCultureIgnoreCase)>=0))
            .OrderByDescending(e=>e.Pinned).ThenByDescending(e=>e.Count).ThenByDescending(e=>e.LastUsed);
    }
    private UIElement FrequentPanel246(string query) {
        var panel=new GroupGlass246 {Theme247=cfg,Width=300,Margin=new Thickness(14,0,10,22)};
        var head=new DockPanel { Margin=new Thickness(2,7,2,10) };
        var manage=Btn("…",()=>ManageFrequent246(true));manage.ToolTip="Kara Liste / Beyaz Liste";DockPanel.SetDock(manage,Dock.Right);head.Children.Add(manage);
        head.Children.Add(new TextBlock {Text="Sık kullanılanlar",FontSize=15,VerticalAlignment=VerticalAlignment.Center});panel.Children.Add(head);
        foreach(var entry in VisibleFrequent246(query)) {
            var captured=entry;var row=new Grid();row.ColumnDefinitions.Add(new ColumnDefinition {Width=new GridLength(42)});row.ColumnDefinitions.Add(new ColumnDefinition());
            ImageSource icon=(!string.IsNullOrEmpty(entry.Item.Picture)?PreviewImage(entry.Item.Picture):null)??ResolveIcon(entry.Item);
            if(icon!=null)row.Children.Add(new Image {Source=icon,Width=32,Height=32,Stretch=Stretch.Uniform});
            var text=new TextBlock {Text=entry.Item.Name,TextTrimming=TextTrimming.CharacterEllipsis,VerticalAlignment=VerticalAlignment.Center};Grid.SetColumn(text,1);row.Children.Add(text);
            var button=Btn("",()=>Launch(captured.Item.Target));button.Content=row;button.HorizontalContentAlignment=HorizontalAlignment.Stretch;button.BorderThickness=new Thickness(1);button.BorderBrush=new SolidColorBrush(Color.FromArgb(90,44,178,232));button.Background=new SolidColorBrush(Color.FromArgb(35,12,69,108));button.ToolTip=entry.Item.Name;
            var menu=new ContextMenu();var pin=new MenuItem {Header=entry.Pinned?"Sabitlemeyi kaldır":"Sık kullanılanlarda sabitle"};pin.Click+=(s,e)=>{captured.Pinned=!captured.Pinned;Save();Refresh();};menu.Items.Add(pin);
            var remove=new MenuItem {Header="Listeden çıkar"};remove.Click+=(s,e)=>{captured.Allowed=false;captured.Pinned=false;Save();Refresh();};menu.Items.Add(remove);
            var edit=new MenuItem {Header="İkon ve Resim seçimi…"};edit.Click+=(s,e)=>VisualDialog216(captured.Item);menu.Items.Add(edit);
            var permissions=new MenuItem {Header="Kara Liste / Beyaz Liste…"};permissions.Click+=(s,e)=>ManageFrequent246(true);menu.Items.Add(permissions);button.ContextMenu=menu;panel.Children.Add(button);
        }
        if(!VisibleFrequent246(query).Any())panel.Children.Add(new TextBlock { Text="Başlat Cam’dan açtıklarınız burada görünür.",TextWrapping=TextWrapping.Wrap,Margin=new Thickness(10) });
        return panel;
    }
    private void AddFrequentSettings246(StackPanel panel) {
        Option(panel,"Sık kullanılanlar bölgesini göster",cfg.FrequentEnabled246,v=>{cfg.FrequentEnabled246=v;Refresh();});
        Note(panel,"Yalnız Başlat Cam’dan açılan öğeler sayılır. Bilgisayardaki diğer etkinlikler izlenmez. Listeden çıkarılan öğeler İzinsiz kalır; sabitlenenler kullanım sayısına bakılmadan gösterilir.");
        panel.Children.Add(Btn("Sık kullanılanlar · Kara Liste / Beyaz Liste…",()=>ManageFrequent246(false)));
    }
    private void ManageFrequent246(bool persist) {
        var draft=json.Deserialize<List<FrequentEntry246>>(json.Serialize(FrequentEntries246()));
        var w=Dialog("Sık kullanılanlar · İzinler",720,600);var outer=new DockPanel {Margin=new Thickness(18)};
        var bottom=new StackPanel {Orientation=Orientation.Horizontal,HorizontalAlignment=HorizontalAlignment.Right};DockPanel.SetDock(bottom,Dock.Bottom);outer.Children.Add(bottom);
        FeatureGlass246(w);
        FrequentEntry246 selected=null;
        var tabs=new TabControl {Background=Brushes.Transparent,Foreground=Brushes.White};
        tabs.Template=(ControlTemplate)System.Windows.Markup.XamlReader.Parse("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' TargetType='TabControl'><DockPanel><TabPanel DockPanel.Dock='Top' IsItemsHost='True'/><Border CornerRadius='14' Background='#8820394C' BorderBrush='#6650BCE8' BorderThickness='1' Padding='8'><ContentPresenter ContentSource='SelectedContent'/></Border></DockPanel></ControlTemplate>");
        var tabStyle=new Style(typeof(TabItem));tabStyle.Setters.Add(new Setter(Control.ForegroundProperty,Brushes.White));tabStyle.Setters.Add(new Setter(Control.TemplateProperty,(ControlTemplate)System.Windows.Markup.XamlReader.Parse("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' TargetType='TabItem'><Border x:Name='frame' CornerRadius='10' Background='#7733536A' BorderBrush='#6650BCE8' BorderThickness='1' Padding='24,12' Margin='0,0,7,12'><ContentPresenter ContentSource='Header'/></Border><ControlTemplate.Triggers><Trigger Property='IsSelected' Value='True'><Setter TargetName='frame' Property='Background' Value='#AA146389'/><Setter TargetName='frame' Property='BorderBrush' Value='#FF51D9FF'/></Trigger></ControlTemplate.Triggers></ControlTemplate>")));tabs.Resources[typeof(TabItem)]=tabStyle;
        outer.Children.Add(tabs);
        Action rebuild=null;
        rebuild=()=>{int selection=tabs.SelectedIndex;tabs.Items.Clear();foreach(bool allowed in new[]{false,true}) {
            var rows=new StackPanel();var tab=new TabItem {Header=allowed?"Beyaz Liste":"Kara Liste",Content=new ScrollViewer {Content=rows,VerticalScrollBarVisibility=ScrollBarVisibility.Auto}};tabs.Items.Add(tab);
            var header=new Grid {Margin=new Thickness(4,8,4,12)};header.ColumnDefinitions.Add(new ColumnDefinition());header.ColumnDefinitions.Add(new ColumnDefinition {Width=new GridLength(80)});header.ColumnDefinitions.Add(new ColumnDefinition {Width=new GridLength(80)});
            foreach(var pair in new[]{new {Text="Dosya / Uygulama adı",Column=0},new {Text="İzinli",Column=1},new {Text="İzinsiz",Column=2}}){var label=new TextBlock {Text=pair.Text,Foreground=Brushes.White};Grid.SetColumn(label,pair.Column);header.Children.Add(label);}rows.Children.Add(header);
            foreach(var entry in draft.Where(e=>e!=null&&e.Item!=null&&e.Allowed==allowed).ToList()) {
                var row=new Grid {Margin=new Thickness(4),MinHeight=40};row.ColumnDefinitions.Add(new ColumnDefinition());row.ColumnDefinitions.Add(new ColumnDefinition {Width=new GridLength(80)});row.ColumnDefinitions.Add(new ColumnDefinition {Width=new GridLength(80)});
                var identity=new DockPanel();var icon=ResolveIcon(entry.Item);if(icon!=null)identity.Children.Add(new Image {Source=icon,Width=26,Height=26,Margin=new Thickness(0,0,10,0)});
                identity.Children.Add(new TextBlock {Text=entry.Item.Name,Foreground=Brushes.White,ToolTip=entry.Item.Target,TextTrimming=TextTrimming.CharacterEllipsis,VerticalAlignment=VerticalAlignment.Center});row.Children.Add(identity);
                row.Background=new SolidColorBrush(Color.FromArgb(20,57,174,235));row.MouseLeftButtonDown+=(s,e)=>{selected=entry;foreach(var r in rows.Children.OfType<Grid>().Skip(1))r.Background=new SolidColorBrush(Color.FromArgb(20,57,174,235));row.Background=new SolidColorBrush(Color.FromArgb(95,31,137,191));};
                var yes=new CheckBox {IsChecked=entry.Allowed,HorizontalAlignment=HorizontalAlignment.Center,VerticalAlignment=VerticalAlignment.Center};var no=new CheckBox {IsChecked=!entry.Allowed,HorizontalAlignment=HorizontalAlignment.Center,VerticalAlignment=VerticalAlignment.Center};Grid.SetColumn(yes,1);Grid.SetColumn(no,2);row.Children.Add(yes);row.Children.Add(no);
                yes.Click+=(s,e)=>{entry.Allowed=yes.IsChecked==true;if(!entry.Allowed)entry.Pinned=false;rebuild();};no.Click+=(s,e)=>{entry.Allowed=no.IsChecked!=true;if(!entry.Allowed)entry.Pinned=false;rebuild();};rows.Children.Add(row);
            }
        }tabs.SelectedIndex=selection<0?1:selection;};
        bottom.Children.Add(Btn("Ekle…",()=>{var tile=AddFrequentTarget246(w);if(tile==null)return;var entry=draft.FirstOrDefault(e=>FrequentKey246(e.Item.Target)==FrequentKey246(tile.Target));if(entry==null){entry=new FrequentEntry246();draft.Add(entry);}entry.Item=tile;entry.Allowed=true;entry.Pinned=true;rebuild();}));
        bottom.Children.Add(Btn("Listeden çıkar",()=>{if(selected==null)return;selected.Allowed=false;selected.Pinned=false;rebuild();}));bottom.Children.Add(Btn("İptal",()=>w.Close()));bottom.Children.Add(Btn("Tamam",()=>{cfg.Frequent246=draft;if(persist)Save();Refresh();w.Close();}));rebuild();w.Content=outer;FeatureProbe246(w);w.ShowDialog();
    }
    private Tile AddFrequentTarget246(Window owner) {
        var w=Dialog("Sık kullanılanlara ekle",540,460);w.Owner=owner;FeatureGlass246(w);Tile result=null;
        var p=new StackPanel {Margin=new Thickness(18)};var name=new TextBox {Margin=new Thickness(3)};var target=new TextBox {Margin=new Thickness(3)};var image=new TextBox {Margin=new Thickness(3)};
        Note(p,"Ad");p.Children.Add(name);Note(p,"Uygulama, dosya, klasör veya Store hedefi");p.Children.Add(target);
        p.Children.Add(Btn("Dosya seç…",()=>{var pick=new Microsoft.Win32.OpenFileDialog();if(pick.ShowDialog(w)==true){target.Text=pick.FileName;if(name.Text.Length==0)name.Text=Path.GetFileNameWithoutExtension(pick.FileName);}}));
        p.Children.Add(Btn("Klasör seç…",()=>{using(var pick=new System.Windows.Forms.FolderBrowserDialog()){var native=new System.Windows.Forms.NativeWindow();try{native.AssignHandle(new System.Windows.Interop.WindowInteropHelper(w).Handle);if(pick.ShowDialog(native)==System.Windows.Forms.DialogResult.OK){target.Text=pick.SelectedPath;if(name.Text.Length==0)name.Text=new DirectoryInfo(pick.SelectedPath).Name;}}finally{native.ReleaseHandle();}}}));
        Note(p,"Özel görsel (isteğe bağlı)");p.Children.Add(image);p.Children.Add(Btn("Görsel seç…",()=>{var pick=new Microsoft.Win32.OpenFileDialog {Filter="Görseller|*.png;*.jpg;*.jpeg;*.ico;*.bmp"};if(pick.ShowDialog(w)==true)image.Text=pick.FileName;}));
        var error=new TextBlock {TextWrapping=TextWrapping.Wrap};p.Children.Add(error);
        p.Children.Add(Btn("Ekle ve sabitle",()=>{if(string.IsNullOrWhiteSpace(name.Text)||string.IsNullOrWhiteSpace(target.Text)){error.Text="Ad ve hedef gereklidir.";return;}try{var tile=new Tile {Name=name.Text.Trim(),Target=target.Text.Trim(),Live=false};if(image.Text.Trim().Length>0)tile.Icon=ImportVisual216(tile,image.Text.Trim(),true);result=tile;w.Close();}catch(Exception ex){error.Text=ex.Message;}}));w.Content=new ScrollViewer {Content=p,VerticalScrollBarVisibility=ScrollBarVisibility.Auto};w.ShowDialog();return result;
    }
}
