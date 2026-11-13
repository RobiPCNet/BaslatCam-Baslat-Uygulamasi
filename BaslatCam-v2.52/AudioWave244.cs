using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;

internal sealed class MediaWaveform244 : FrameworkElement
{
    internal float[] Peaks = new float[0];
    internal double Progress;
    protected override void OnRender(DrawingContext dc)
    {
        double middle = ActualHeight / 2;
        dc.DrawLine(new Pen(new SolidColorBrush(Color.FromArgb(80,38,190,225)),1),
            new Point(0,middle),new Point(ActualWidth,middle));
        if(Peaks.Length==0)return;
        int bars=Math.Max(1,Math.Min(Peaks.Length,(int)(ActualWidth/2.5)));
        var unplayed=new Pen(new LinearGradientBrush(Color.FromArgb(190,25,124,220),Color.FromArgb(220,40,207,230),90),1.8);
        var played=new Pen(new LinearGradientBrush(Color.FromRgb(77,220,255),Color.FromRgb(28,179,215),90),1.8);
        unplayed.StartLineCap=unplayed.EndLineCap=played.StartLineCap=played.EndLineCap=PenLineCap.Round;
        for(int i=0;i<bars;i++)
        {
            int start=i*Peaks.Length/bars,end=(i+1)*Peaks.Length/bars;
            float peak=0;
            for(int j=start;j<end;j++)peak=Math.Max(peak,Peaks[j]);
            double x=(i+.5)*ActualWidth/bars;
            double height=Math.Max(.5,Math.Sqrt(peak)*middle*.92);
            dc.DrawLine(i/(double)bars<=Progress?played:unplayed,new Point(x,middle-height),new Point(x,middle+height));
        }
    }
}

internal static class AudioWave244
{
    [DllImport("mfplat.dll")]private static extern int MFStartup(int version,int flags);
    [DllImport("mfplat.dll")]private static extern int MFShutdown();
    [DllImport("mfplat.dll")]private static extern int MFCreateMediaType(out IntPtr type);
    [DllImport("mfreadwrite.dll",CharSet=CharSet.Unicode)]
    private static extern int MFCreateSourceReaderFromURL(string path,IntPtr attributes,out IntPtr reader);
    [DllImport("ole32.dll")]private static extern int CoInitializeEx(IntPtr reserved,int flags);
    [DllImport("ole32.dll")]private static extern void CoUninitialize();
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]private delegate int SetSelection(IntPtr self,uint stream,[MarshalAs(UnmanagedType.Bool)]bool selected);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]private delegate int SetGuid(IntPtr self,ref Guid key,ref Guid value);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]private delegate int SetNumber(IntPtr self,ref Guid key,uint value);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]private delegate int GetNumber(IntPtr self,ref Guid key,out uint value);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]private delegate int SetType(IntPtr self,uint stream,IntPtr reserved,IntPtr type);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]private delegate int GetType(IntPtr self,uint stream,out IntPtr type);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]private delegate int Read(IntPtr self,uint stream,uint flags,out uint actual,out uint status,out long time,out IntPtr sample);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]private delegate int GetBuffer(IntPtr self,out IntPtr buffer);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]private delegate int LockBuffer(IntPtr self,out IntPtr data,out uint maximum,out uint length);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]private delegate int UnlockBuffer(IntPtr self);

    // COM slots follow IMFAttributes, IMFSourceReader, IMFSample and IMFMediaBuffer.
    private static T Method<T>(IntPtr instance,int slot) where T:class
    {
        return Marshal.GetDelegateForFunctionPointer(Marshal.ReadIntPtr(Marshal.ReadIntPtr(instance),slot*IntPtr.Size),typeof(T)) as T;
    }
    private static void Check(int result){Marshal.ThrowExceptionForHR(result);}
    private static void Release(ref IntPtr pointer){if(pointer!=IntPtr.Zero){Marshal.Release(pointer);pointer=IntPtr.Zero;}}

    internal static float[] Decode(string path,double seconds,CancellationToken cancel)
    {
        if(seconds<=0||double.IsNaN(seconds)||double.IsInfinity(seconds))return new float[0];
        IntPtr reader=IntPtr.Zero,type=IntPtr.Zero,current=IntPtr.Zero;
        bool com=CoInitializeEx(IntPtr.Zero,0)>=0,started=false;
        try
        {
            Check(MFStartup(0x20070,1));started=true;
            Check(MFCreateSourceReaderFromURL(path,IntPtr.Zero,out reader));
            var selection=Method<SetSelection>(reader,4);
            Check(selection(reader,0xfffffffe,false));
            Check(selection(reader,0xfffffffd,true));
            Check(MFCreateMediaType(out type));
            Guid major=new Guid("48eba18e-f8c9-4687-bf11-0a74c9f96a8f");
            Guid audio=new Guid("73647561-0000-0010-8000-00aa00389b71");
            Guid sub=new Guid("f7e34c9a-42e8-4714-b74b-cb29d72c35e5");
            Guid pcm=new Guid("00000001-0000-0010-8000-00aa00389b71");
            var set=Method<SetGuid>(type,24);
            Check(set(type,ref major,ref audio));Check(set(type,ref sub,ref pcm));
            Guid bitsKey=new Guid("f2deb57f-40fa-4764-aa33-ed4f2d1ff669");
            Check(Method<SetNumber>(type,21)(type,ref bitsKey,16));
            Check(Method<SetType>(reader,7)(reader,0xfffffffd,IntPtr.Zero,type));
            Check(Method<GetType>(reader,6)(reader,0xfffffffd,out current));
            Guid rateKey=new Guid("5faeeae7-0290-4c31-9e8a-c534f68d9dba");
            Guid channelsKey=new Guid("37e48bf5-645e-4c5b-89de-ada9e29b696a");
            uint rate,channels,bits;
            var get=Method<GetNumber>(current,7);
            Check(get(current,ref rateKey,out rate));Check(get(current,ref channelsKey,out channels));
            Check(get(current,ref bitsKey,out bits));
            if(rate==0||channels==0||bits!=16)return new float[0];
            var peaks=new float[768];
            var read=Method<Read>(reader,9);
            var budget=Stopwatch.StartNew();
            bool samples=false;
            while(!cancel.IsCancellationRequested&&budget.Elapsed.TotalSeconds<5)
            {
                IntPtr sample=IntPtr.Zero,buffer=IntPtr.Zero;
                try
                {
                    uint actual,status;long time;
                    Check(read(reader,0xfffffffd,0,out actual,out status,out time,out sample));
                    if((status&2)!=0)break;
                    if(sample==IntPtr.Zero)continue;
                    Check(Method<GetBuffer>(sample,41)(sample,out buffer));
                    IntPtr bytes;uint maximum,length;
                    Check(Method<LockBuffer>(buffer,3)(buffer,out bytes,out maximum,out length));
                    try
                    {
                        if(length>4*1024*1024)continue;
                        var data=new short[length/2];
                        Marshal.Copy(bytes,data,0,data.Length);
                        for(int frame=0;frame<data.Length/channels;frame+=4)
                        {
                            double position=time/10000000.0+frame/(double)rate;
                            int bin=(int)(position/seconds*peaks.Length);
                            if(bin<0||bin>=peaks.Length)continue;
                            for(int channel=0;channel<channels;channel++)
                            {
                                float peak=Math.Abs(data[frame*(int)channels+channel]/32768f);
                                peaks[bin]=Math.Max(peaks[bin],peak);
                            }
                        }
                        samples=true;
                    }
                    finally { Method<UnlockBuffer>(buffer,4)(buffer); }
                }
                finally { Release(ref buffer);Release(ref sample); }
            }
            return samples?peaks:new float[0];
        }
        catch(Exception error) {
#if DIAGNOSTICS
            Console.WriteLine("Waveform decode " + error.Message);
#endif
            return new float[0];
        }
        finally
        {
            Release(ref current);Release(ref type);Release(ref reader);
            if(started)MFShutdown();
            if(com)CoUninitialize();
        }
    }
}

