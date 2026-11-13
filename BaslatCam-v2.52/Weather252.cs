using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

internal sealed class StormMotion252 : WeatherMotion240 {
 private readonly RainMotion240 rain=new RainMotion240();
 internal double Elapsed {get;private set;}
 internal double Flash {get{double t=Elapsed%9.7;return Math.Max(0,1-Math.Abs(t-.8)/.09)*.10+Math.Max(0,1-Math.Abs(t-1.06)/.13)*.065;}}
 private readonly BitmapSource bolt;
 internal StormMotion252(BitmapSource picture){bolt=picture;IsHitTestVisible=false;ClipToBounds=true;AddVisualChild(rain);}
 protected override int VisualChildrenCount {get{return 1;}}
 protected override Visual GetVisualChild(int index){return rain;}
 protected override Size MeasureOverride(Size size){rain.Measure(size);return new Size();}
 protected override Size ArrangeOverride(Size size){rain.Arrange(new Rect(size));return size;}
 internal override void Advance(double seconds){Elapsed+=seconds;rain.Advance(seconds);InvalidateVisual();}
 protected override void OnRender(DrawingContext dc){double flash=Flash;if(flash<=0)return;dc.DrawRectangle(new SolidColorBrush(Color.FromArgb((byte)(flash*255),220,235,255)),null,new Rect(RenderSize));double height=ActualHeight*.96,width=height*bolt.PixelWidth/bolt.PixelHeight;double x=ActualWidth*(.3+.35*(.5+.5*Math.Sin(Math.Floor(Elapsed/9.7)*2.1)))-width/2;dc.PushOpacity(flash/.1*.8);dc.DrawImage(bolt,new Rect(x,0,width,height));dc.Pop();}

}
internal sealed class WindMotion252 : WeatherMotion240 {
 private readonly Point[] origins=new Point[48];
 private readonly Brush[] colors={Brushes.OliveDrab,Brushes.DarkGoldenrod,Brushes.Peru,Brushes.Goldenrod};
 private static readonly Geometry Leaf=CreateLeaf();
 private readonly Pen vein=new Pen(new SolidColorBrush(Color.FromArgb(130,230,221,173)),.65);
 internal double Elapsed {get;private set;}
 private static Geometry CreateLeaf(){var shape=Geometry.Parse("M-9,0 C-3,-8 4,-8 10,0 C4,7 -3,7 -9,0 Z");shape.Freeze();return shape;}
 internal WindMotion252(){ClipToBounds=true;IsHitTestVisible=false;var random=new Random(5252);for(int i=0;i<origins.Length;i++)origins[i]=new Point(random.NextDouble(),random.NextDouble());vein.Freeze();}
 internal override void Advance(double seconds){Elapsed+=seconds;InvalidateVisual();}
 protected override void OnRender(DrawingContext dc){if(ActualWidth<=0||ActualHeight<=0)return;for(int i=0;i<origins.Length;i++){
  double depth=.45+(i%4)*.3;double x=(origins[i].X*(ActualWidth+100)+Elapsed*(42+depth*40))%(ActualWidth+100)-50;
  double y=(origins[i].Y*(ActualHeight+80)+Elapsed*(7+depth*9)+Math.Sin(Elapsed*1.1+i)*30)%(ActualHeight+80)-40;
  double angle=Elapsed*(30+(i%5)*17)+i*31;double flip=.25+.75*Math.Abs(Math.Cos(Elapsed*(.8+origins[i].X)+i));
  dc.PushOpacity(.45+depth*.23);dc.PushTransform(new TranslateTransform(x,y));dc.PushTransform(new RotateTransform(angle));dc.PushTransform(new ScaleTransform(depth,depth*flip));dc.DrawGeometry(colors[i%colors.Length],null,Leaf);dc.DrawLine(vein,new Point(-7,0),new Point(7,0));dc.Pop();dc.Pop();dc.Pop();dc.Pop();
 }}
}
