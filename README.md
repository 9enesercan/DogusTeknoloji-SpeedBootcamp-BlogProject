# 🚀 .NET Core MVC Blog Projesi

Bu proje, ASP.NET Core MVC, Entity Framework Core ve Identity teknolojilerini kullanarak geliştirilmiş kapsamlı ve yönetilebilir bir blog uygulamasıdır. Temiz mimari, modern kullanıcı arayüzü ve esnek yapı kullanılarak oluşturulmuştur.

## 🚦 Başlangıç Rehberi

### 📌 Ön Gereksinimler

* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) veya üzeri
* [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) veya üzeri
* [MS SQL Server](https://www.microsoft.com/tr-tr/sql-server/sql-server-downloads) (veya SQL Express)

### 📌 Kurulum Adımları

1.  **Repoyu Klonla:**
    ```bash
    git clone https://github.com/9enesercan/DogusTeknoloji-SpeedBootcamp-BlogProject.git
    cd DogusTeknoloji-SpeedBootcamp-BlogProject
    ```

2.  **Uygulama Ayarları (`appsettings.json`):**
    Projenizdeki `appsettings.json` dosyasını açın ve veritabanı bağlantı dizeninizi (`ConnectionString`) güncelleyin:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Data Source=***;Initial Catalog=DbBlog;Integrated Security=True;Encrypt=False;"
      },
      // ... diğer ayarlar
    }
    ```
    *Not: `***` kısmını kendi sunucu adınızla değiştirin.*

3.  **Veritabanını Oluştur:**
    Visual Studio içerisinde `Package Manager Console`'u açın ve aşağıdaki komutları sırasıyla çalıştırın:
    ```powershell
    add-migration InitialCreate
    update-database
    ```
    Bu komutlar veritabanı şemasını oluşturacak ve gerekli tabloları yaratacaktır.

4.  **Uygulamayı Çalıştırma ve Seed Data Bilgileri:**
    * Uygulamayı Visual Studio üzerinden `IIS Express` veya `ProjeAdı` profili ile çalıştırın.
    * Alternatif olarak, projenin ana dizininde terminal veya komut istemcisini açıp şu komutu kullanın:
        ```bash
        dotnet run
        ```
    * Uygulama ilk çalıştığında, veritabanına aşağıdaki test kullanıcıları otomatik olarak eklenecektir (Seed Data):
        * **Admin Kullanıcısı:**
            * Email: `admin@site.com`
            * Şifre: `admin123`
        * **Normal Kullanıcı:**
            * Email: `user@site.com`
            * Şifre: `user123`

## ✨ Özellikler

### Kullanıcı Yönetimi
* Kullanıcı kayıt ve giriş işlemleri (ASP.NET Core Identity & Cookie Authentication).
* Admin ve Kullanıcı olmak üzere rol bazlı yetkilendirme.
* Admin paneli üzerinden kullanıcıları ve rollerini yönetme imkanı.

### Blog Yönetimi
* Blog yazıları için temel CRUD (Oluşturma, Okuma, Güncelleme, Silme) işlemleri.
* Yazıları kategorilere göre filtreleme.
* Blog yazılarına kapak fotoğrafı yükleme ve düzenleme.

### Yorum Sistemi
* Kullanıcıların blog yazılarına yorum yapabilmesi.
* Yorumlara yanıt verebilme (iç içe yorumlar).
* Kullanıcıların kendi yorumlarını düzenleyebilmesi ve silebilmesi.

### Güvenlik
* Rol Bazlı Yetkilendirme (Role-Based Authorization) ile sayfa ve işlem bazında erişim kontrolü.
* ASP.NET Core Identity kütüphanesi ile güvenli kullanıcı yönetimi ve kimlik doğrulama.
* Model validasyonları ve diğer güvenlik önlemleri.

### Tasarım
* Responsive (duyarlı) kullanıcı arayüzü (Bootstrap 5 kullanılarak).
* Blog kartları için animasyonlu efektler.
* Kullanıcı dostu ve modern bir görünüm.

### Ekstralar
* Uygulama ilk çalıştığında otomatik olarak eklenen test verileri (Seed Data): Fotoğraflı blog yazıları, yorumlar ve yanıtlar.
* İşlevsel ve esnek bir Admin yönetim paneli.
* SOLID prensiplerine uygun kod yapısı ve Generic Repository Pattern kullanımı ile sürdürülebilir ve test edilebilir mimari.

## 🛠 Kullanılan Teknolojiler

| Teknoloji             | Açıklama                                       |
| :-------------------- | :--------------------------------------------- |
| .NET 9                | Uygulamanın temelini oluşturan framework (MVC Mimarisi) |
| Entity Framework Core | Veritabanı işlemleri için ORM (Object-Relational Mapper) |
| MS SQL Server         | İlişkisel veritabanı yönetim sistemi           |
| ASP.NET Core Identity | Kullanıcı kimlik doğrulama ve yetkilendirme sistemi |
| Bootstrap 5           | Responsive front-end tasarım framework'ü       |
| JavaScript & jQuery   | İstemci tarafında interaktiflik ve dinamik içerik |
| HTML & CSS            | Web sayfalarının temel yapı taşları ve stil tanımları |
| Dependency Injection  | Bağımlılıkların yönetimi için kullanılan tasarım deseni |
| SOLID & Repository    | Temiz kodlama prensipleri ve veri erişim katmanı deseni |


## Uygulama Ekran Görüntüleri 
*Ana Sayfa
![image](https://github.com/user-attachments/assets/2735b6d7-660a-4914-a298-00b2f786cdf9)

*Admin Dashboard
![image](https://github.com/user-attachments/assets/70f052f9-10bd-410c-9f2b-5dcb52733279)

*Admin kullanıcılar , yorumlar ve katagorileri kontrolleri sağlayacağı ekranlar örnek
![image](https://github.com/user-attachments/assets/3737cdbd-95d4-4d9c-91e3-ba35ec79e60e)

*Blog oluşturma 
![image](https://github.com/user-attachments/assets/ac21988c-1c36-4d26-be7a-edfd94ca3f37)

*Profilde bulunan blog kontrolleri
![image](https://github.com/user-attachments/assets/928cafe6-e238-43e0-8503-324be995d473)

