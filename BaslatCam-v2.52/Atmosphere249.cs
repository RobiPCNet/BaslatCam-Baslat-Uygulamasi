using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

public sealed class GlassReflection249 : FrameworkElement {
 public Config Theme;public Func<Geometry> Outline;private bool running;private double lastSpeed=1;
 internal double CycleSeconds250 {get{return 14/lastSpeed;}}
 public static double SafeSpeed250(double speed){return double.IsNaN(speed)||double.IsInfinity(speed)?1:Math.Max(.25,Math.Min(3,speed));}
 public static readonly DependencyProperty ProgressProperty=DependencyProperty.Register("Progress",typeof(double),typeof(GlassReflection249),new FrameworkPropertyMetadata(0.0,FrameworkPropertyMetadataOptions.AffectsRender));
 public double Progress {get{return (double)GetValue(ProgressProperty);}set{SetValue(ProgressProperty,value);}}
 public GlassReflection249(){IsHitTestVisible=false;Loaded+=(s,e)=>Sync();Unloaded+=(s,e)=>Stop();IsVisibleChanged+=(s,e)=>Sync();}
 public void Stop(){BeginAnimation(ProgressProperty,null);running=false;}
 public void Sync(){var window=Window.GetWindow(this);bool active=(window==null||window.IsVisible&&window.WindowState!=WindowState.Minimized)&&Theme!=null&&Theme.GlassReflection249&&Theme.ThemeGlow247>0&&IsLoaded&&IsVisible&&!SystemParameters.HighContrast;double speed=Theme==null?1:SafeSpeed250(Theme.GlassSpeed250);if(active!=running||active&&Math.Abs(speed-lastSpeed)>.001){double start=Progress%1;Stop();lastSpeed=speed;if(active){var animation=new DoubleAnimation(start,start+1,TimeSpan.FromSeconds(14/speed)){RepeatBehavior=RepeatBehavior.Forever};Timeline.SetDesiredFrameRate(animation,15);BeginAnimation(ProgressProperty,animation);running=true;}}InvalidateVisual();}
 protected override void OnRender(DrawingContext dc){if(Theme==null||!Theme.GlassReflection249||ActualWidth<2||ActualHeight<2||SystemParameters.HighContrast)return;
  double w=ActualWidth,h=ActualHeight;Geometry edge=Outline==null?new RectangleGeometry(new Rect(1,1,w-2,h-2),16,16):Outline();
  var path=PathGeometry.CreateFromGeometry(edge);double strength=Math.Max(0,Math.Min(1,Theme.ThemeGlow247));
  for(int k=0;k<2;k++){
   double phase=(Progress+(k==0?.08:.56))%1;Point point,tangent;path.GetPointAtFractionLength(phase,out point,out tangent);
   var glow=new RadialGradientBrush {MappingMode=BrushMappingMode.Absolute,Center=point,GradientOrigin=point,RadiusX=42,RadiusY=42};glow.GradientStops.Add(new GradientStop(Color.FromArgb((byte)(200*strength),115,222,255),0));glow.GradientStops.Add(new GradientStop(Colors.Transparent,1));
   dc.PushClip(edge);dc.DrawGeometry(null,new Pen(glow,8),edge);
   double size=16+8*Math.Sin(Progress*Math.PI*2+k);var shine=new LinearGradientBrush();shine.GradientStops.Add(new GradientStop(Colors.Transparent,0));shine.GradientStops.Add(new GradientStop(Color.FromArgb((byte)(255*strength),215,250,255),.5));shine.GradientStops.Add(new GradientStop(Colors.Transparent,1));
   Vector direction=new Vector(tangent.X,tangent.Y);if(direction.Length>0)direction.Normalize();else direction=new Vector(1,0);dc.DrawLine(new Pen(shine,2),point-direction*size,point+direction*size);dc.Pop();
  }
 }
}
internal sealed class ReflectionAdorner249 : Adorner {
 internal readonly GlassReflection249 Surface;internal AdornerLayer OwnerLayer;
 public ReflectionAdorner249(Border border,Config cfg):base(border){IsHitTestVisible=false;Surface=new GlassReflection249 {Theme=cfg};Surface.Outline=()=>new RectangleGeometry(new Rect(1,1,Math.Max(1,border.ActualWidth-2),Math.Max(1,border.ActualHeight-2)),border.CornerRadius.TopLeft,border.CornerRadius.TopLeft);AddVisualChild(Surface);}
 protected override int VisualChildrenCount {get{return 1;}}
 protected override Visual GetVisualChild(int index){return Surface;}
 protected override Size MeasureOverride(Size size){Surface.Measure(AdornedElement.RenderSize);return AdornedElement.RenderSize;}
 protected override Size ArrangeOverride(Size size){Surface.Arrange(new Rect(size));return size;}
}
public partial class StartWindow {
 private readonly Dictionary<Border,ReflectionAdorner249> reflections249=new Dictionary<Border,ReflectionAdorner249>();
 private void SyncReflectionVisibility249(){foreach(var surface in FindVisual<GlassReflection249>(this)){if(WorkVisible)surface.Sync();else surface.Stop();}foreach(var overlay in reflections249.Values){if(WorkVisible)overlay.Surface.Sync();else overlay.Surface.Stop();}}
 private GlassReflection249 NewCardReflection251(){var surface=new GlassReflection249 {Theme=cfg,Tag="card-reflection251",Margin=new Thickness(-1)};surface.Outline=()=>new RectangleGeometry(new Rect(1,1,Math.Max(1,surface.ActualWidth-2),Math.Max(1,surface.ActualHeight-2)),19,19);Panel.SetZIndex(surface,10);return surface;}
 private void ApplyReflections249(){
  var targets=new HashSet<Border>();foreach(var card in glassCards){var host=card.Child as Grid;var local=host==null?null:host.Children.OfType<GlassReflection249>().FirstOrDefault();if(local!=null){local.Theme=cfg;local.Sync();}else targets.Add(card);}if(weatherHeader220!=null)targets.Add(weatherHeader220);
  DependencyObject current=search;while(current!=null){current=VisualTreeHelper.GetParent(current);var border=current as Border;if(border!=null){targets.Add(border);break;}}
  var stale=new List<Border>();foreach(var pair in reflections249)if(!targets.Contains(pair.Key)){pair.Value.Surface.Stop();var layer=pair.Value.OwnerLayer;if(layer!=null)layer.Remove(pair.Value);stale.Add(pair.Key);}foreach(var key in stale)reflections249.Remove(key);
  foreach(var border in targets){ReflectionAdorner249 overlay;if(!reflections249.TryGetValue(border,out overlay)){var layer=AdornerLayer.GetAdornerLayer(border);if(layer==null)continue;overlay=new ReflectionAdorner249(border,cfg);reflections249[border]=overlay;overlay.OwnerLayer=layer;layer.Add(overlay);}overlay.Surface.Theme=cfg;overlay.Surface.Sync();}
 }
}
internal sealed class CloudMotion249 : WeatherMotion240 {
 private readonly DrawingGroup cloud;
 internal readonly bool Night,Partly;
 internal double Elapsed {get;private set;}
 internal CloudMotion249(bool night,bool partly,BitmapSource picture){Night=night;Partly=partly;ClipToBounds=true;
  if(night){var converted=new FormatConvertedBitmap(picture,PixelFormats.Bgra32,null,0);int stride=converted.PixelWidth*4;byte[] pixels=new byte[stride*converted.PixelHeight];converted.CopyPixels(pixels,stride,0);for(int i=0;i<pixels.Length;i+=4){pixels[i]=(byte)(pixels[i]*.58);pixels[i+1]=(byte)(pixels[i+1]*.44);pixels[i+2]=(byte)(pixels[i+2]*.35);}picture=BitmapSource.Create(converted.PixelWidth,converted.PixelHeight,96,96,PixelFormats.Bgra32,null,pixels,stride);picture.Freeze();}
  cloud=new DrawingGroup();using(var dc=cloud.Open())dc.DrawImage(picture,new Rect(0,0,380,175));cloud.Freeze();}

 internal override void Advance(double seconds){Elapsed+=Math.Max(0,seconds);InvalidateVisual();}
 protected override void OnRender(DrawingContext dc){if(ActualWidth<1||ActualHeight<1)return;dc.PushTransform(new ScaleTransform(ActualWidth/960,ActualHeight/540));int count=Partly?4:7;
  for(int i=0;i<count;i++){double scale=.85+(i%3)*.3;double x=((i*223+Elapsed*(4+i%3*1.5))%1500)-380;double y=10+(i%3)*90+(i/3)*16+Math.Sin(Elapsed*.035+i)*6;dc.PushOpacity(Partly?.45:.65);dc.PushTransform(new TranslateTransform(x,y));dc.PushTransform(i%2==0?(Transform)new ScaleTransform(scale,scale):new MatrixTransform(-scale,0,0,scale,380*scale,0));dc.DrawDrawing(cloud);dc.Pop();dc.Pop();dc.Pop();}dc.Pop();}
}
