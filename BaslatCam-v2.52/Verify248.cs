#if DIAGNOSTICS
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Automation;
public partial class StartWindow {
 private async System.Threading.Tasks.Task VerifyFixes248(){
  string report=OwnPath15("Dogrulama-v2.52.txt");File.WriteAllText(report,"");Action<bool,string> check=(ok,label)=>{if(!ok)throw new Exception(label);File.AppendAllText(report,"PASS: "+label+"\r\n");};
  cfg=Defaults();cfg.PanelPinned=true;cfg.AutoCollapseOverlap=false;cfg.RightPanelWidth=320;ShowResidentMenu();panelTimer.Stop();weatherTimer220.Stop();
  var tile=Leaves(cfg.Tiles).First(t=>ResolveIcon(t)!=null);cfg.TileShape247="square";Refresh();UpdateLayout();
  var button=TileButtons().First(b=>((Tile)b.DataContext).Id==tile.Id);var icon=FindVisual<Image>(button).First();double iw=icon.ActualWidth,ih=icon.ActualHeight;
  check(iw>0&&ih>0,"Temel ikon ölçüsü alındı");
  foreach(string shape in new[]{"round","concave","cloud"}){cfg.TileShape247=shape;ApplyTheme247();UpdateLayout();check(Math.Abs(icon.ActualWidth-iw)<.01&&Math.Abs(icon.ActualHeight-ih)<.01,shape+": ikon ölçüsü korunur, içerik küçültülmez");check(button.Padding==new Thickness(0)&&button.Clip!=null,shape+": şekle göre kırpma etkin");}
  string fixture=OwnPath15("picture-fixture.png");using(var bitmap=new System.Drawing.Bitmap(240,160)){using(var g=System.Drawing.Graphics.FromImage(bitmap))g.Clear(System.Drawing.Color.DarkCyan);bitmap.Save(fixture,System.Drawing.Imaging.ImageFormat.Png);}
  var photo=new Tile {Name="Resim kırpma",Picture=fixture,Size=3,Live=false};cfg.Tiles.Add(photo);cfg.TileShape247="square";Refresh();UpdateLayout();var photoButton=TileButtons().First(b=>((Tile)b.DataContext).Id==photo.Id);var state=(PreviewState)photoButton.GetValue(PreviewStateProperty);double pw=state.Image.ActualWidth,ph=state.Image.ActualHeight;
  cfg.TileShape247="cloud";ApplyTheme247();UpdateLayout();check(state.Image.ActualWidth==pw&&state.Image.ActualHeight==ph,"Resimli kutucukta görüntü alanı küçülmez");
  string before=json.Serialize(cfg);
  settingsProbe=view=>{var option=FindVisual<CheckBox>(view.Window).First(c=>Convert.ToString(c.Content).StartsWith("Kutucuk gruplarının arka"));option.IsChecked=false;check(!cfg.ShowGroupSurface248&&FindVisual<GroupGlass246>(this).All(g=>!g.Theme247.ShowGroupSurface248),"Grup arka yüzeyi ve çerçevesi canlı gizlenir");view.Cancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));};Settings();settingsProbe=null;if(probeError!=null)throw probeError;check(json.Serialize(cfg)==before,"İptal grup çerçevesi tercihini geri alır");
  startupWriteProbe226=(enabled,command)=>{};settingsProbe=view=>{FindVisual<CheckBox>(view.Window).First(c=>Convert.ToString(c.Content).StartsWith("Kutucuk gruplarının arka")).IsChecked=false;view.Save.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));};Settings();settingsProbe=null;startupWriteProbe226=null;if(probeError!=null)throw probeError;check(!json.Deserialize<Config>(File.ReadAllText(FilePath)).ShowGroupSurface248,"Grup çerçevesi tercihi kalıcı kaydedilir");
  Capture(this,OwnPath15("Kutucuklar-v2.52.png"));
  string audio=Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"media244-audio.mp3");EnsureMusic241();AddMusicList241("Deneme",new[]{audio});BuildSystemPanel();UpdateLayout();
  var side=(StackPanel)VisualTreeHelper.GetParent(musicPlay241);var sideNames=side.Children.OfType<Button>().Select(AutomationProperties.GetName).ToArray();check(sideNames.SequenceEqual(new[]{"Önceki parça","Çal / duraklat","Sonraki parça","Karıştır","Tekrar","Görüntüye getir"}),"Yan oynatıcı: önceki, oynat, sonraki, karıştır, tekrar, görüntü");
  for(int i=1;i<side.Children.Count;i++){var left=(FrameworkElement)side.Children[i-1];var right=(FrameworkElement)side.Children[i];check(right.TranslatePoint(new Point(),side).X>=left.TranslatePoint(new Point(left.ActualWidth,0),side).X,"Yan oynatıcı düğmeleri çakışmaz · "+i);}
  dashboardCards["music"].BringIntoView();UpdateLayout();Capture(this,OwnPath15("Player-yan-v2.52.png"));
  ShowMusic241();UpdateLayout();musicWindow241.UpdateLayout();var floating=(StackPanel)VisualTreeHelper.GetParent(musicCentralPlay242);check(floating.Children.OfType<Button>().Select(AutomationProperties.GetName).SequenceEqual(new[]{"Önceki","Çal / duraklat","Sonraki","Karıştır","Tekrar","Tam ekran"}),"Yüzen oynatıcıda aynı düğme sırası");Capture(musicWindow241,OwnPath15("Player-yuzen-v2.52.png"));musicWindow241.Close();
  await VerifyCompact244();check(true,"Medya oynatma, Türkçe liste, bağımsız pencere, ses, tam ekran ve Esc regresyonları geçti");
 }
}
#endif
