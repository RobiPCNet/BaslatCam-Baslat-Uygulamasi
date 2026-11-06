# Başlat Cam v2.39 kaynak projesi

Geliştirici: Mustafa ARSLAN. Kodlama/görsel desteği: OpenAI Codex / ImageGen.

Bu paket yayımlanan v2.39 C# kaynağını değiştirmeden içerir. Kaynak tek dosyadır; arayüz C# içinde oluşturulur, ayrı XAML yoktur. 31 hava görseli EmbeddedResource olarak eklenir. Visual Studio proje dosyası paketleme için sonradan oluşturulmuştur; önceki sürüm doğrudan csc.exe ile derlenmiştir.

Windows üzerinde Visual Studio ve .NET Framework 4.8 Developer Pack ile BaslatCam.csproj dosyasını açıp derleyin. Alternatif: PowerShell içinde ./build.ps1; bu yol Windows .NET Framework derleyicisini kullanır, Python veya NuGet gerektirmez. Çıktı bin/Portable/BaslatCam-v2.39.exe olur. Derleme betiği uygulamayı çalıştırmaz.

Harici/özel DLL yoktur; başvurular Windows .NET Framework kitaplıklarıdır. Kişisel duzen.json, uygulama önbelleği, konum ve kullanıcı verileri pakete eklenmemiştir. Çalışırken kullanıcı verileri %LOCALAPPDATA%/başlat/V2.34 altında oluşur. Hava durumu gerçek veri için dış servis kullanır. Avast tespiti incelenmektedir; bu paket bir güvenlik düzeltmesi içermez.

DIAGNOSTICS ve FAST221 derleme sembolleri kontrollü test kodlarını etkinleştirir; normal projede kapalıdır. Doğrulama raporu ve sürüm kullanım notları docs altındadır. Derleme sonrası farklı proje/derleyici seçenekleri nedeniyle EXE hash'inin yayımlanan dosyayla aynı olması beklenmez; kaynak hash'i SHA256SUMS.txt ile doğrulanabilir.
