# Başlat Cam v2.30.1

- Sağ panelin aç/kapat oku Başlat görünümünün dikey ortasında kalır. Panel kapandığında üst tarafa sıçramaz; açık ve kapalı konumlar aynı hizada tutulur. Düğmenin açıklaması durumuna göre aç/kapat olarak değişir.
- Raptiye ile sabitlenen panel fare ayrılması veya grup çakışması nedeniyle otomatik kapanmaz. Önceden kurulmuş çakışma zamanlayıcısı da raptiyeyi yeniden kontrol eder.
- Kullanıcı okla veya Paneli kapat seçeneğiyle sabit paneli elle kapatabilir. Yeniden açıldığında sabitleme tercihi korunur. Raptiye kaldırıldığında mevcut süreli otomatik kapanma davranışları devam eder.
- Başlat görünümünün ESC veya başka programa geçişle gizlenme davranışı korunur; raptiye sağ sistem panelinin otomatik daralmasını yönetir.

122 kontrol geçti: önceki 114 ve açık/kapalı ok orta hizası, raptiyede iki otomatik kapanış yolunun engellenmesi, bekleyen zamanlayıcı, elle kapatma/yeniden açmada raptiyenin korunması ve sabitleme kaldırıldığında otomatik kapanış. Zamanlayıcılar kontrollü çakışma/fare girdileriyle sınandı. Gerçek güç işlemleri ve güvenlik ayarı değişiklikleri yapılmadı.

Eski uygulamadan Uygulamadan çık ile çıkıp yeni EXE'yi açın. Veri klasörü %LOCALAPPDATA%\başlat\V2.30.1; önceki ayarlar aktarılır, eski klasörler korunur. Otomatik başlatma açıksa normal açılış kaydı yeni konuma günceller.

Geliştirici: Mustafa ARSLAN. Kodlama desteği: OpenAI Codex (ChatGPT).
