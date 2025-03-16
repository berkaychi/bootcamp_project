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
   http://localhost:5000
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

Bu proje, kütüphane yönetimini kolaylaştırarak kitapların ve kullanıcıların verimli bir şekilde yönetilmesini sağlar. Her türlü geri bildirim ve katkıya açıktır!

Bu API için ön uç geliştiricilerin rahatça kullanabileceği detaylı bir dokümantasyon hazırlayacağım. Markdown formatında oluşturacağım, böylece GitHub, Notion veya herhangi bir dokümantasyon platformunda kolayca paylaşılabilir. 

Aşağıda, **UsersController** içinde tanımlı tüm **GET** ve **POST** işlemleri için hazırladığım API dokümantasyonunu bulabilirsin. Eğer Swagger gibi bir dokümantasyon aracı ile çalışıyorsan, bu yapıyı OpenAPI'ye çevirebilirim.

---

## 📌 **ClimbUp API - User Endpoints**
Bu dokümantasyon, **UsersController** içindeki tüm kullanıcı işlemlerini içermektedir.

### 📍 **Base URL**  
```
https://api.climbup.com/api/users
```

---

## 🔹 **Kullanıcı Kayıt İşlemi**
### **POST /register**
Yeni bir kullanıcı kaydı oluşturur.

- **Endpoint:** `/register`
- **Authorization:** ❌ Gerekli değil
- **Headers:**
  ```json
  {
    "Content-Type": "application/json"
  }
  ```
- **Body (JSON):**
  ```json
  {
    "fullName": "John Doe",
    "userName": "johndoe",
    "email": "johndoe@example.com",
    "password": "StrongPassword123"
  }
  ```
- **Response (201 Created)**
  ```json
  {
    "message": "User registered successfully"
  }
  ```
- **Hata Yanıtları:**
  - `400 Bad Request` → Geçersiz veya eksik girişler
  - `409 Conflict` → E-posta veya kullanıcı adı zaten kayıtlı

---

## 🔹 **Kullanıcı Listesi**
### **GET /getusers**
Kayıtlı tüm kullanıcıları getirir.

- **Endpoint:** `/getusers`
- **Authorization:** ❌ Gerekli değil
- **Response (200 OK):**
  ```json
  [
    {
      "fullName": "John Doe",
      "userName": "johndoe",
      "email": "johndoe@example.com",
      "dateAdded": "2025-03-16T12:34:56Z"
    }
  ]
  ```
---

## 🔹 **Belirli Bir Kullanıcıyı Getirme**
### **GET /getuser/{id}**
ID'si verilen kullanıcının bilgilerini getirir.

- **Endpoint:** `/getuser/{id}`
- **Authorization:** ❌ Gerekli değil
- **Path Parameters:**
  | Parametre | Tip | Zorunlu | Açıklama |
  |-----------|-----|---------|----------|
  | `id` | string | ✔️ | Kullanıcının ID'si |

- **Response (200 OK):**
  ```json
  {
    "fullName": "John Doe",
    "userName": "johndoe",
    "email": "johndoe@example.com",
    "dateAdded": "2025-03-16T12:34:56Z"
  }
  ```
- **Hata Yanıtları:**
  - `404 Not Found` → Kullanıcı bulunamadı

---

## 🔹 **Giriş İşlemi**
### **POST /login**
Kullanıcının giriş yapmasını sağlar.

- **Endpoint:** `/login`
- **Authorization:** ❌ Gerekli değil
- **Headers:**
  ```json
  {
    "Content-Type": "application/json"
  }
  ```
- **Body (JSON):**
  ```json
  {
    "email": "johndoe@example.com",
    "password": "StrongPassword123"
  }
  ```
- **Response (200 OK)**
  ```json
  {
    "token": "jwt_token_here"
  }
  ```
- **Hata Yanıtları:**
  - `400 Bad Request` → Yanlış e-posta veya şifre
  - `401 Unauthorized` → E-posta doğrulanmamış

---

## 🔹 **E-posta Doğrulama**
### **POST /confirm-email**
E-posta adresini doğrulamak için kullanılır.

- **Endpoint:** `/confirm-email`
- **Authorization:** ❌ Gerekli değil
- **Body (JSON):**
  ```json
  {
    "email": "johndoe@example.com",
    "code": "123456"
  }
  ```
- **Response (200 OK)**
  ```json
  {
    "message": "Email confirmed"
  }
  ```
- **Hata Yanıtları:**
  - `400 Bad Request` → Yanlış kod veya süre dolmuş

---

## 🔹 **Şifre Sıfırlama (Unuttum)**
### **POST /password/reset**
Şifre sıfırlama bağlantısı göndermek için kullanılır.

- **Endpoint:** `/password/reset`
- **Authorization:** ❌ Gerekli değil
- **Body (JSON):**
  ```json
  {
    "email": "johndoe@example.com"
  }
  ```
- **Response (200 OK)**
  ```json
  {
    "message": "Password reset email sent"
  }
  ```
- **Hata Yanıtları:**
  - `400 Bad Request` → Geçersiz e-posta

---

## 🔹 **Şifre Sıfırlama (Yeni Şifre Tanımlama)**
### **POST /password/reset/confirm**
Yeni şifre belirlemek için kullanılır.

- **Endpoint:** `/password/reset/confirm`
- **Authorization:** ❌ Gerekli değil
- **Body (JSON):**
  ```json
  {
    "userId": "user-id-here",
    "token": "reset-token-here",
    "password": "NewStrongPassword123"
  }
  ```
- **Response (200 OK)**
  ```json
  {
    "message": "Password reset successful"
  }
  ```
- **Hata Yanıtları:**
  - `400 Bad Request` → Geçersiz veya süresi dolmuş token

---

## 🔹 **Şifre Değiştirme (Oturum Açıkken)**
### **POST /change-password**
Giriş yapmış kullanıcının şifresini değiştirir.

- **Endpoint:** `/change-password`
- **Authorization:** ✅ Gerekli
- **Headers:**
  ```json
  {
    "Authorization": "Bearer jwt_token_here"
  }
  ```
- **Body (JSON):**
  ```json
  {
    "currentPassword": "OldPassword123",
    "newPassword": "NewStrongPassword123"
  }
  ```
- **Response (200 OK)**
  ```json
  {
    "message": "Password changed"
  }
  ```
- **Hata Yanıtları:**
  - `400 Bad Request` → Yanlış mevcut şifre

---

## 🔹 **Profil Güncelleme**
### **PUT /updateprofile**
Kullanıcının profil bilgilerini günceller.

- **Endpoint:** `/updateprofile`
- **Authorization:** ✅ Gerekli
- **Headers:**
  ```json
  {
    "Authorization": "Bearer jwt_token_here"
  }
  ```
- **Body (JSON):**
  ```json
  {
    "fullName": "John Doe",
    "userName": "johndoe",
    "email": "johndoe@example.com"
  }
  ```
- **Response (200 OK)**
  ```json
  {
    "message": "Profile updated",
    "token": "new_jwt_token_here"
  }
  ```

---

Bu dokümantasyonu Markdown olarak kaydedebilir veya API'nin içine **Swagger/OpenAPI** olarak entegre edebilirim. Formatı değiştirmek istersen haber ver! 🚀
