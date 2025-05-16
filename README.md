![image](https://github.com/user-attachments/assets/05ea511e-c5c0-4ca3-9231-5710ea0b2119)# **Kütüphane Yönetim Sistemi**

## **Proje Açıklaması**
Bu proje, kütüphane yönetim süreçlerini dijitalleştirmek amacıyla geliştirilmiştir. Kullanıcılar, kitap ekleme, güncelleme, silme ve arama işlemlerini kolayca gerçekleştirebilir. Ayrıca ödünç alma ve iade süreçleri yönetilebilir.

## **Kullanılan Teknolojiler**
- **ASP.NET Core MVC** - Backend geliştirme için kullanılmıştır.
- **SQLite** - Hafif ve kolay yönetilebilir veritabanı çözümü.
- **Entity Framework Core** - ORM kullanılarak veritabanı işlemlerini kolaylaştırır.
- **HTML, CSS, JavaScript** - Kullanıcı arayüzü geliştirmek için kullanılmıştır.
- **Bootstrap** - Responsive tasarım sağlamak için kullanılmıştır.

## **Kurulum**
1. Proje dosyalarını indirin veya klonlayın:
   ```sh
   git clone https://github.com/username/kutuphane-yonetim.git
   ```
2. Proje dizinine gidin:
   ```sh
   cd LibraryManagement
   ```
3. Gerekli bağımlılıkları yükleyin:
   ```sh
   dotnet restore
   ```
4. Veritabanını oluşturun ve migrasyonları çalıştırın:
   ```sh
   dotnet ef database update
   ```
5. Uygulamayı başlatın:
   ```sh
   dotnet run
   ```
6. Tarayıcınızda aşağıdaki bağlantıyı açarak projeyi inceleyin:
   ```
   http://localhost:5069
   ```

## **Kod Yapısı**
- `Models/` - Veritabanı modelleri
- `Controllers/` - Kullanıcı isteklerini işleyen denetleyiciler
- `Views/` - Kullanıcı arayüzü bileşenleri
- `Data/` - Veritabanı bağlantıları ve yapılandırmaları
- `wwwroot/` - Statik dosyalar (CSS, JavaScript, görseller)

## **Örnek Ekran Görüntüleri**
1. **Ana Sayfa** - Kullanıcıların boştaki kitapları görebildiği ana ekran
![image](https://github.com/user-attachments/assets/13589d63-de31-4717-a4f4-0016778f917a)

2. **Giriş Ekranı** - Kullanıcı giriş ekranı
![image](https://github.com/user-attachments/assets/abd6c899-3b95-4408-b6a1-31b9fec726ca)

4. **Kayıt Ekranı** - Kullanıcı kayıt ekranı
![image](https://github.com/user-attachments/assets/5c4e7cf9-052f-4888-b81d-29c4f5820d55)

5. **Kitap Ekranı** - Kullanıcıların gördüğü kitap ekranı
![image](https://github.com/user-attachments/assets/c7ef0dc4-a3a3-42f0-a568-85395f070a4d)

6. **Profil Ekranı** - Profil bilgilerini görme ve düzenleme ekranı
![image](https://github.com/user-attachments/assets/281d3d26-e91f-4234-8230-5fa541daddf8)

7. **Admin Kitap Ekranı** - Adminlerin gördüğü kitap ekranı
![image](https://github.com/user-attachments/assets/d8be83f0-542e-4a48-ba99-0b22523ab8a3)

8. **Kitap Güncelleme Sayfası** - Mevcut kitap bilgilerini güncelleme ekranı
![image](https://github.com/user-attachments/assets/813c431a-6c00-4899-893e-1a5ff0cb0dfa)

9. **Kitap Ekleme Sayfası** - Yeni kitap ekleme ekranı
![image](https://github.com/user-attachments/assets/ea8494ec-4ffd-4a35-9772-7faec2baf76b)

10. **Kullanıcı Yönetim Sayfası** - Kütüphane kullanıcılarının yönetildiği arayüz
![image](https://github.com/user-attachments/assets/c42c1737-92fe-4272-81a7-fc5be0055ee7)

---
