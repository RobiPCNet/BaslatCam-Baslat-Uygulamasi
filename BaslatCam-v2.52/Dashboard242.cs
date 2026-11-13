using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

internal sealed class GpuUsage242 : IDisposable
{
    [StructLayout(LayoutKind.Sequential)]
    private struct Value { internal uint Status; internal double Number; }
    [StructLayout(LayoutKind.Sequential)]
    private struct Item { internal IntPtr Name; internal Value Value; }
    [DllImport("pdh.dll", CharSet = CharSet.Unicode)]
    private static extern uint PdhOpenQuery(string source, IntPtr user, out IntPtr query);
    [DllImport("pdh.dll", CharSet = CharSet.Unicode)]
    private static extern uint PdhAddEnglishCounter(IntPtr query, string path, IntPtr user, out IntPtr counter);
    [DllImport("pdh.dll")]
    private static extern uint PdhCollectQueryData(IntPtr query);
    [DllImport("pdh.dll", CharSet = CharSet.Unicode)]
    private static extern uint PdhGetFormattedCounterArray(IntPtr counter, uint format, ref uint bytes, ref uint count, IntPtr buffer);
    [DllImport("pdh.dll")]
    private static extern uint PdhCloseQuery(IntPtr query);
    private readonly object gate = new object();
    private IntPtr query, counter;
    private bool disposed;
    private DateTime retry = DateTime.MinValue;
    internal string Status { get; private set; }

    internal double? Read()
    {
        lock (gate)
        {
            if (disposed) return null;
            try
            {
                if (query == IntPtr.Zero)
                {
                    if (DateTime.UtcNow < retry) return null;
                    if (PdhOpenQuery(null, IntPtr.Zero, out query) != 0 ||
                        PdhAddEnglishCounter(query, @"\GPU Engine(*)\Utilization Percentage", IntPtr.Zero, out counter) != 0)
                        return Fail("GPU sayacı kullanılamıyor");
                    if (PdhCollectQueryData(query) != 0) return Fail("GPU sayacı okunamadı");
                    Status = "Örnek bekleniyor";
                    return null;
                }
                if (PdhCollectQueryData(query) != 0) return Fail("GPU sayacı okunamadı");
                for (int attempt = 0; attempt < 3; attempt++)
                {
                    uint bytes = 0, count = 0;
                    uint result = PdhGetFormattedCounterArray(counter, 0x200 | 0x8000, ref bytes, ref count, IntPtr.Zero);
                    if (result != 0x800007D2 && result != 0) return Fail("GPU örneği alınamadı");
                    if (bytes == 0 || bytes > 64 * 1024 * 1024) return Fail("GPU örneği bulunamadı");
                    IntPtr buffer = Marshal.AllocHGlobal((int)bytes);
                    try
                    {
                        result = PdhGetFormattedCounterArray(counter, 0x200 | 0x8000, ref bytes, ref count, buffer);
                        if (result == 0x800007D2) continue;
                        if (result != 0) return Fail("GPU örneği alınamadı");
                        var values = new List<KeyValuePair<string, double>>();
                        int stride = Marshal.SizeOf(typeof(Item));
                        for (int i = 0; i < count; i++)
                        {
                            var item = (Item)Marshal.PtrToStructure(IntPtr.Add(buffer, i * stride), typeof(Item));
                            if (item.Value.Status <= 1 && !double.IsNaN(item.Value.Number) && !double.IsInfinity(item.Value.Number))
                                values.Add(new KeyValuePair<string, double>(Marshal.PtrToStringUni(item.Name), item.Value.Number));
                        }
                        double? load = Aggregate(values);
                        Status = load.HasValue ? "En yoğun GPU motoru" : "GPU örneği bekleniyor";
                        return load;
                    }
                    finally { Marshal.FreeHGlobal(buffer); }
                }
                return Fail("GPU sayaçları değişti");
            }
            catch { return Fail("GPU sayacı kullanılamıyor"); }
        }
    }

    internal static double? Aggregate(IEnumerable<KeyValuePair<string, double>> values)
    {
        var groups = values.Where(p => !string.IsNullOrEmpty(p.Key) && p.Value >= 0 &&
                !double.IsNaN(p.Value) && !double.IsInfinity(p.Value))
            .GroupBy(p => Regex.Replace(Regex.Replace(p.Key, @"^pid_\d+_", ""), @"#\d+$", ""));
        var loads = groups.Select(g => g.Sum(p => p.Value)).ToList();
        return loads.Count == 0 ? (double?)null : Math.Max(0, Math.Min(100, loads.Max()));
    }

    private double? Fail(string status)
    {
        if (query != IntPtr.Zero) PdhCloseQuery(query);
        query = counter = IntPtr.Zero;
        retry = DateTime.UtcNow.AddMinutes(1);
        Status = status;
        return null;
    }

    public void Dispose()
    {
        lock (gate) { disposed = true; Fail("GPU ölçümü durduruldu"); }
    }
}

public partial class StartWindow
{
    private readonly GpuUsage242 gpu242 = new GpuUsage242();

    private List<string> PanelOrder242()
    {
        var names = CardNames();
        return (cfg.PanelOrder242 ?? "").Split(',').Where(names.ContainsKey)
            .Concat(names.Keys).Distinct().ToList();
    }

    private IEnumerable<KeyValuePair<string, string>> OrderedCards242()
    {
        var names = CardNames();
        return PanelOrder242().Select(key => new KeyValuePair<string, string>(key, names[key]));
    }

    private void MovePanelCard242(string key, int offset)
    {
        var order = PanelOrder242();
        int index = order.IndexOf(key), target = index + offset;
        if (index < 0 || target < 0 || target >= order.Count) return;
        order.RemoveAt(index);
        order.Insert(target, key);
        cfg.PanelOrder242 = string.Join(",", order);
        BuildSystemPanel();
    }

    private void AddPanelOrder242(StackPanel pane)
    {
        var list = new ListBox { Height = 166, DisplayMemberPath = "Value", SelectedValuePath = "Key", Margin = new Thickness(4, 8, 4, 6) };
        Action<string> refresh = key =>
        {
            list.ItemsSource = OrderedCards242().ToList();
            if (key != null) list.SelectedValue = key;
            else list.SelectedIndex = 0;
        };
        Note(pane, "Kart sırası · Seçtiğiniz kartı yukarı veya aşağı taşıyın. Değişiklik panelde anında görünür.");
        pane.Children.Add(list);
        var actions = new StackPanel { Orientation = Orientation.Horizontal };
        foreach (int offset in new[] { -1, 1 })
        {
            int direction = offset;
            actions.Children.Add(Btn(offset < 0 ? "↑ Yukarı" : "↓ Aşağı", () =>
            {
                string key = Convert.ToString(list.SelectedValue);
                MovePanelCard242(key, direction);
                refresh(key);
            }));
        }
        pane.Children.Add(actions);
        refresh(null);
    }
}



