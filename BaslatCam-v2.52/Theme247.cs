using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Input;

public static class TileForm247 {
 public static Geometry Make(string shape,double w,double h,double amount) {
  w=Math.Max(1,w);h=Math.Max(1,h);double d=Math.Max(0,Math.Min(.25,amount));
  if(shape=="square")return new RectangleGeometry(new Rect(0,0,w,h));
  if(shape=="concave") {
   var g=new StreamGeometry();using(var c=g.Open()){c.BeginFigure(new Point(1,1),true,true);c.QuadraticBezierTo(new Point(w*.5,h*d*2),new Point(w-1,1),true,false);c.QuadraticBezierTo(new Point(w-w*d*2,h*.5),new Point(w-1,h-1),true,false);c.QuadraticBezierTo(new Point(w*.5,h-h*d*2),new Point(1,h-1),true,false);c.QuadraticBezierTo(new Point(w*d*2,h*.5),new Point(1,1),true,false);}g.Freeze();return g;
  }
  if(shape=="cloud") {
   var g=new StreamGeometry();using(var c=g.Open()){
    c.BeginFigure(new Point(w*.12,h*.77),true,true);
    c.BezierTo(new Point(w*.01,h*.77),new Point(-w*.01,h*.64),new Point(w*.06,h*.58),true,false);
    c.BezierTo(new Point(-w*.02,h*.49),new Point(w*.01,h*.35),new Point(w*.1,h*.34),true,false);
    c.BezierTo(new Point(w*.04,h*.19),new Point(w*.16,h*.1),new Point(w*.26,h*.15),true,false);
    c.BezierTo(new Point(w*.3,h*.03),new Point(w*.42,h*.03),new Point(w*.48,h*.11),true,false);
    c.BezierTo(new Point(w*.53,h*.01),new Point(w*.65,h*.03),new Point(w*.69,h*.12),true,false);
    c.BezierTo(new Point(w*.78,h*.03),new Point(w*.91,h*.1),new Point(w*.9,h*.21),true,false);
    c.BezierTo(new Point(w*.99,h*.23),new Point(w*.99,h*.36),new Point(w*.95,h*.4),true,false);
    c.BezierTo(new Point(w*1.02,h*.5),new Point(w*.99,h*.64),new Point(w*.91,h*.66),true,false);
    c.BezierTo(new Point(w*.92,h*.8),new Point(w*.8,h*.84),new Point(w*.73,h*.79),true,false);
    c.BezierTo(new Point(w*.67,h*.92),new Point(w*.54,h*.89),new Point(w*.5,h*.83),true,false);
    c.BezierTo(new Point(w*.43,h*.93),new Point(w*.32,h*.89),new Point(w*.29,h*.82),true,false);
    c.BezierTo(new Point(w*.2,h*.88),new Point(w*.14,h*.84),new Point(w*.12,h*.77),true,false);
   }
   var group=new GeometryGroup();group.Children.Add(g);group.Children.Add(new EllipseGeometry(new Point(w*.115,h*.92),w*.045,h*.035));group.Children.Add(new EllipseGeometry(new Point(w*.04,h*.975),w*.025,h*.02));group.Freeze();return group;
  }
  return new RectangleGeometry(new Rect(0,0,w,h),Math.Min(w,h)*Math.Min(.3,d),Math.Min(w,h)*Math.Min(.3,d));
 }
}
public sealed class TileChrome247 : Decorator {
 public Config Theme;
 public string PreviewState="";
 public static readonly DependencyProperty PhaseProperty=DependencyProperty.Register("Phase",typeof(double),typeof(TileChrome247),new FrameworkPropertyMetadata(0.0,FrameworkPropertyMetadataOptions.AffectsRender));
 public double Phase {get{return (double)GetValue(PhaseProperty);}set{SetValue(PhaseProperty,value);}}
 private bool moving;
 public TileChrome247(){IsHitTestVisible=false;Loaded+=(s,e)=>SyncMotion();Unloaded+=(s,e)=>StopMotion();IsVisibleChanged+=(s,e)=>SyncMotion();}
 public void StopMotion(){BeginAnimation(PhaseProperty,null);moving=false;}
 public void SyncMotion(){bool next=Theme!=null&&Theme.ThemeMode247!="glass"&&Theme.ThemeWaves247&&Theme.ThemeMotion247&&IsVisible&&IsLoaded&&!SystemParameters.HighContrast;if(next==moving)return;StopMotion();if(next){var animation=new DoubleAnimation(0,Math.PI*2,TimeSpan.FromSeconds(8)){RepeatBehavior=RepeatBehavior.Forever};Timeline.SetDesiredFrameRate(animation,15);BeginAnimation(PhaseProperty,animation);moving=true;}InvalidateVisual();}
 protected override void OnRender(DrawingContext dc){
  base.OnRender(dc);var b=TemplatedParent as Button;var cfg=Theme;if(b==null||cfg==null)return;
  double w=ActualWidth,h=ActualHeight;if(w<1||h<1)return;
  bool hover=PreviewState=="hover"||b.IsMouseOver,pressed=PreviewState=="pressed"||b.IsPressed;
  string mode=cfg.ThemeMode247;bool blue=mode=="blue",hybrid=mode=="hybrid";double glow=Math.Max(0,Math.Min(1,cfg.ThemeGlow247));
  Geometry geometry=TileForm247.Make(cfg.TileShape247,w,h,cfg.TileCurve247);
  dc.PushClip(geometry);
  Brush fill=b.Background;
  if(blue)fill=new LinearGradientBrush(Color.FromArgb((byte)(255*Math.Max(.18,cfg.TileOpacity)),19,95,145),Color.FromArgb((byte)(255*Math.Max(.18,cfg.TileOpacity)),1,20,47),90);
  dc.DrawGeometry(fill,null,geometry);
  if(mode!="glass"){
   dc.DrawRectangle(new LinearGradientBrush(Color.FromArgb((byte)(glow*(hover?80:42)),83,203,255),Colors.Transparent,90),null,new Rect(0,0,w,h*.6));
   if(cfg.ThemeWaves247){
    for(int k=0;k<2;k++){
     var wave=new StreamGeometry();using(var c=wave.Open()){double y=h*(.76+k*.09)+Math.Sin(Phase+k)*h*.025;c.BeginFigure(new Point(-2,y),false,false);c.BezierTo(new Point(w*.28,y-h*.28),new Point(w*.7,y+h*.23),new Point(w+2,y-h*.02),true,false);}
     dc.DrawGeometry(null,new Pen(new SolidColorBrush(Color.FromArgb((byte)(glow*(hover?120:65)),40,190,255)),k==0?2:1),wave);
    }
   }
  }
  if(hover)dc.DrawGeometry(new SolidColorBrush(Color.FromArgb(pressed?(byte)55:(byte)28,43,190,245)),null,geometry);
  dc.Pop();
  Color edge=SystemParameters.HighContrast?Colors.White:Color.FromArgb((byte)(mode=="glass"?(hover?180:55):Math.Min(255,65+glow*(hover?190:125))),61,201,255);
  dc.DrawGeometry(null,new Pen(new SolidColorBrush(edge),pressed?2:1.2),geometry);
 }
}
public partial class StartWindow {
 private static ControlTemplate tileTemplate247;
 private Action themePreview247;
 private void ThemeChoice247(ComboBox combo){
  combo.FontFamily=new FontFamily("Segoe UI");combo.Foreground=Brushes.White;combo.FontSize=14;
  combo.Template=(ControlTemplate)System.Windows.Markup.XamlReader.Parse("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' TargetType='ComboBox'><Grid><ToggleButton Focusable='False' IsChecked='{Binding IsDropDownOpen, RelativeSource={RelativeSource TemplatedParent}, Mode=TwoWay}'><ToggleButton.Template><ControlTemplate TargetType='ToggleButton'><Border CornerRadius='9' Background='#CC203F54' BorderBrush='#9955BFEA' BorderThickness='1' Padding='12,10'><Grid><TextBlock Text='⌄' Foreground='#8CDEFF' HorizontalAlignment='Right'/></Grid></Border></ControlTemplate></ToggleButton.Template></ToggleButton><ContentPresenter Content='{TemplateBinding SelectionBoxItem}' IsHitTestVisible='False' Margin='12,8,32,8' VerticalAlignment='Center'/><Popup Name='PART_Popup' IsOpen='{TemplateBinding IsDropDownOpen}' Placement='Bottom' AllowsTransparency='True' Focusable='False'><Border Background='#F013293A' BorderBrush='#AA55BFEA' BorderThickness='1' CornerRadius='8' MinWidth='{Binding ActualWidth, RelativeSource={RelativeSource TemplatedParent}}' Padding='4'><ScrollViewer MaxHeight='260'><ItemsPresenter KeyboardNavigation.DirectionalNavigation='Contained'/></ScrollViewer></Border></Popup></Grid></ControlTemplate>");
  var style=new Style(typeof(ComboBoxItem));style.Setters.Add(new Setter(Control.ForegroundProperty,Brushes.White));style.Setters.Add(new Setter(Control.FontFamilyProperty,new FontFamily("Segoe UI")));style.Setters.Add(new Setter(Control.TemplateProperty,(ControlTemplate)System.Windows.Markup.XamlReader.Parse("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' TargetType='ComboBoxItem'><Border x:Name='frame' Background='Transparent' Padding='12,9' CornerRadius='6'><ContentPresenter/></Border><ControlTemplate.Triggers><Trigger Property='IsHighlighted' Value='True'><Setter TargetName='frame' Property='Background' Value='#AA185D7D'/></Trigger><Trigger Property='IsSelected' Value='True'><Setter TargetName='frame' Property='Background' Value='#AA147BA3'/></Trigger></ControlTemplate.Triggers></ControlTemplate>")));combo.ItemContainerStyle=style;
 }
 private static ControlTemplate TileTemplate247(){if(tileTemplate247!=null)return tileTemplate247;var root=new FrameworkElementFactory(typeof(Grid));root.SetValue(Panel.BackgroundProperty,Brushes.Transparent);var chrome=new FrameworkElementFactory(typeof(TileChrome247));chrome.Name="chrome247";root.AppendChild(chrome);var presenter=new FrameworkElementFactory(typeof(ContentPresenter));presenter.SetValue(ContentPresenter.ContentProperty,new TemplateBindingExtension(ContentControl.ContentProperty));presenter.SetValue(ContentPresenter.ContentTemplateProperty,new TemplateBindingExtension(ContentControl.ContentTemplateProperty));presenter.SetValue(FrameworkElement.MarginProperty,new TemplateBindingExtension(Control.PaddingProperty));presenter.SetValue(FrameworkElement.HorizontalAlignmentProperty,HorizontalAlignment.Stretch);presenter.SetValue(FrameworkElement.VerticalAlignmentProperty,VerticalAlignment.Stretch);root.AppendChild(presenter);var reflection=new FrameworkElementFactory(typeof(GlassReflection249));reflection.Name="reflection249";root.AppendChild(reflection);tileTemplate247=new ControlTemplate(typeof(Button)){VisualTree=root};return tileTemplate247;}
 private void ApplyTileTheme247(Button button){
  button.Template=TileTemplate247();button.ApplyTemplate();
  var reflection=button.Template.FindName("reflection249",button) as GlassReflection249;if(reflection!=null){reflection.Theme=cfg;reflection.Outline=()=>TileForm247.Make(reflection.Theme.TileShape247,button.ActualWidth,button.ActualHeight,reflection.Theme.TileCurve247);reflection.Sync();}
  var chrome=button.Template.FindName("chrome247",button) as TileChrome247;if(chrome!=null){chrome.Theme=cfg;chrome.InvalidateVisual();chrome.SyncMotion();}
  double w=button.ActualWidth>0?button.ActualWidth:button.Width,h=button.ActualHeight>0?button.ActualHeight:button.Height;
  if(!double.IsNaN(w)&&!double.IsNaN(h))button.Clip=TileForm247.Make(cfg.TileShape247,w,h,cfg.TileCurve247);
  button.Padding=new Thickness(0);
  var grid=button.Content as Grid;
  if(grid!=null){double inset=cfg.TileShape247=="cloud"?h*.13:cfg.TileShape247=="concave"?h*cfg.TileCurve247*.55:0;
   foreach(var text in grid.Children.OfType<TextBlock>().Where(t=>t.VerticalAlignment==VerticalAlignment.Bottom&&t.FontFamily.Source!="Segoe MDL2 Assets"))text.Margin=new Thickness(0,0,0,inset);
   foreach(var band in grid.Children.OfType<Border>().Where(t=>t.VerticalAlignment==VerticalAlignment.Bottom))band.Margin=new Thickness(0,0,0,inset);
  }
  button.MouseEnter-=TileHover247;button.MouseEnter+=TileHover247;button.MouseLeave-=TileHover247;button.MouseLeave+=TileHover247;button.PreviewMouseDown-=TileHover247;button.PreviewMouseDown+=TileHover247;button.PreviewMouseUp-=TileHover247;button.PreviewMouseUp+=TileHover247;
 }
 private void TileHover247(object sender,MouseEventArgs e){var b=sender as Button;if(b==null)return;var chrome=b.Template.FindName("chrome247",b) as TileChrome247;if(chrome!=null)chrome.InvalidateVisual();}
 private void ApplyTheme247(){
  ApplyReflections249();foreach(var button in TileButtons())ApplyTileTheme247(button);
  foreach(var group in FindVisual<GroupGlass246>(this)){group.Theme247=cfg;group.InvalidateVisual();}
  bool glass=cfg.ThemeMode247=="glass";
  DependencyObject searchParent=search;Border searchBorder=null;while(searchParent!=null){searchParent=VisualTreeHelper.GetParent(searchParent);searchBorder=searchParent as Border;if(searchBorder!=null)break;}
  foreach(var card in glassCards)card.BorderBrush=new SolidColorBrush(glass?Color.FromArgb(70,180,206,220):Color.FromArgb((byte)(80+130*Math.Max(0,Math.Min(1,cfg.ThemeGlow247))),53,193,247));
  if(searchBorder!=null){searchBorder.Background=glass?(Brush)new SolidColorBrush(Color.FromArgb(45,255,255,255)):new LinearGradientBrush(Color.FromArgb(125,28,80,119),Color.FromArgb(100,4,23,47),90);searchBorder.BorderBrush=new SolidColorBrush(glass?Color.FromArgb(55,255,255,255):Color.FromArgb(180,39,178,240));}
 }
 private void AddThemeSettings247(Panel appearance){
  var box=new StackPanel {Margin=new Thickness(0,8,0,18)};
  box.Children.Add(new TextBlock {Text="Tema ve kutucuk biçimi",FontSize=21,Margin=new Thickness(4,8,4,12)});
  var modes=new ComboBox {Tag="theme-mode247",Margin=new Thickness(4),ItemsSource=new[]{"Saydam Cam","Parlak Mavi","Hibrit"},SelectedIndex=cfg.ThemeMode247=="glass"?0:cfg.ThemeMode247=="blue"?1:2};box.Children.Add(modes);
  var shapes=new ComboBox {Tag="tile-shape247",Margin=new Thickness(4),ItemsSource=new[]{"Sert köşeli","Yuvarlatılmış","İçbükey","Düşünce bulutu"},SelectedIndex=cfg.TileShape247=="square"?0:cfg.TileShape247=="concave"?2:cfg.TileShape247=="cloud"?3:1};box.Children.Add(shapes);
  ThemeChoice247(modes);ThemeChoice247(shapes);
  Option(box,"Kutucuk gruplarının arka yüzeyini ve çerçevesini göster",cfg.ShowGroupSurface248,v=>{cfg.ShowGroupSurface248=v;ApplyTheme247();});
  var previews=new WrapPanel {Margin=new Thickness(4,12,4,10)};box.Children.Add(previews);
  Action rebuild=()=>{previews.Children.Clear();foreach(string state in new[]{"normal","hover","pressed"}){
   var column=new StackPanel {Margin=new Thickness(5)};var sample=new Button {Width=145,Height=120,Background=new SolidColorBrush(Color.FromArgb((byte)(255*cfg.TileOpacity),23,107,154)),Foreground=Brushes.White,Content=new StackPanel {Children={new TextBlock {Text="\uE8F1",FontFamily=new FontFamily("Segoe MDL2 Assets"),FontSize=36,HorizontalAlignment=HorizontalAlignment.Center,Margin=new Thickness(0,9,0,9)},new TextBlock {Text="Örnek kutucuk",TextAlignment=TextAlignment.Center}}}};
   ApplyTileTheme247(sample);var chrome=(TileChrome247)sample.Template.FindName("chrome247",sample);chrome.PreviewState=state;column.Children.Add(sample);column.Children.Add(new TextBlock {Text=state=="normal"?"Normal":state=="hover"?"Fare üzerinde":"Basılı",HorizontalAlignment=HorizontalAlignment.Center,Margin=new Thickness(0,7,0,0)});previews.Children.Add(column);
  }};
  modes.SelectionChanged+=(s,e)=>{cfg.ThemeMode247=new[]{"glass","blue","hybrid"}[Math.Max(0,modes.SelectedIndex)];ApplyTheme247();rebuild();};
  shapes.SelectionChanged+=(s,e)=>{cfg.TileShape247=new[]{"square","round","concave","cloud"}[Math.Max(0,shapes.SelectedIndex)];ApplyTheme247();rebuild();};
  var curveLabel=new TextBlock {Margin=new Thickness(4)};box.Children.Add(curveLabel);
  var curve=new Slider {Tag="theme-curve247",Minimum=0,Maximum=25,Value=Math.Max(0,Math.Min(25,cfg.TileCurve247*100)),Margin=new Thickness(4)};box.Children.Add(curve);
  curve.IsEnabled=shapes.SelectedIndex==1||shapes.SelectedIndex==2;shapes.SelectionChanged+=(s,e)=>curve.IsEnabled=shapes.SelectedIndex==1||shapes.SelectedIndex==2;
  Action curveText=()=>curveLabel.Text="Kavis / içbükeylik: %"+Math.Round(curve.Value);curveText();curve.ValueChanged+=(s,e)=>{cfg.TileCurve247=curve.Value/100;curveText();ApplyTheme247();rebuild();};
  var glowLabel=new TextBlock {Margin=new Thickness(4)};box.Children.Add(glowLabel);
  var glow=new Slider {Tag="theme-glow247",Minimum=0,Maximum=100,Value=Math.Max(0,Math.Min(100,cfg.ThemeGlow247*100)),Margin=new Thickness(4)};box.Children.Add(glow);
  Action glowText=()=>glowLabel.Text="Mavi ışık belirginliği: %"+Math.Round(glow.Value);glowText();glow.ValueChanged+=(s,e)=>{cfg.ThemeGlow247=glow.Value/100;glowText();ApplyTheme247();rebuild();};
  Option(box,"Cam kenarlarında hareketli ışık yansıması",cfg.GlassReflection249,v=>{cfg.GlassReflection249=v;ApplyTheme247();rebuild();});
  AddReflectionSpeed250(box,rebuild);
  Option(box,"Mavi dalga çizgilerini göster",cfg.ThemeWaves247,v=>{cfg.ThemeWaves247=v;ApplyTheme247();rebuild();});
  Option(box,"Dalgaları yavaş hareket ettir",cfg.ThemeMotion247,v=>{cfg.ThemeMotion247=v;ApplyTheme247();rebuild();});
  Note(box,"Ayarlar ana ekrana canlı uygulanır. Normal, fare üzerinde ve basılı durumları yukarıda görünür. Saydamlık aşağıdaki kutucuk opaklığından ayarlanır. Kaydet kalıcıdır; İptal / Esc / × önceki temaya döner. Hareket varsayılan kapalıdır.");
  box.Loaded+=(s,e)=>themePreview247=rebuild;box.Unloaded+=(s,e)=>{if(themePreview247==rebuild)themePreview247=null;};appearance.Children.Insert(0,box);rebuild();
 }
}
