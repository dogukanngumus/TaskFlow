# 📦 Katman Tasarımları: Infrastructure & API

Bu bölümde, TaskFlow uygulamasının `Infrastructure` ve `API` katmanlarının teknik tasarımı, uygulanmış özellikleri, bilerek basit tutulan alanları ve gelecekte yapılabilecek geliştirmeler yer almaktadır.

---

## 🏗️ Infrastructure Katmanı

### 🎯 Amaç
Infrastructure katmanı, uygulamanın veri erişim detaylarını barındırır. Burada `Entity Framework Core` ile domain modellerin In-Memory veritabanı üzerinden işlenmesi sağlanır.

### ✅ Uygulananlar

- Entity Framework Core (v9) kullanıldı.
- InMemory veritabanı ile hızlı test ve demo imkânı sağlandı.
- `TaskDbContext` tanımlandı ve `TaskItem` için `DbSet` eklendi.
- `ITaskItemRepository` arayüzü, `TaskItemRepository` sınıfı ile implemente edildi.
- Value Object (`Title`) mapping'i `Owned Entity` olarak yapılandırıldı.
- Clean Architecture prensiplerine uyularak, bu katman Application katmanına bağımlı hale getirilmedi.

### 🛑 Yapılmayanlar (Basitlik İçin)

- Kalıcı bir veritabanı (SQL Server, PostgreSQL vs.) entegrasyonu yapılmadı.
- Migration yapısı oluşturulmadı.
- Transaction yönetimi, Unit of Work veya Retry mekanizmaları eklenmedi.
- Dış sistem bağlantıları (örneğin e-posta, üçüncü parti API entegrasyonları) yapılmadı.


## 🌐 API Katmanı

### 🎯 Amaç
API katmanı, dış istemcilerle HTTP üzerinden iletişim kurmak için RESTful servisleri sunar. Bu projede `Minimal API` yaklaşımı kullanılmıştır.

### ✅ Uygulananlar

- `Minimal API` ile CRUD endpoint'leri tanımlandı:
  - `GET /tasks`
  - `GET /tasks/{id}`
  - `POST /tasks`
  - `PUT /tasks/{id}`
  - `DELETE /tasks/{id}`
- `ICommandDispatcher` ve `IQueryDispatcher` aracılığıyla CQRS pattern'i kullanıldı.
- Swagger/OpenAPI desteği eklendi (`AddEndpointsApiExplorer`, `AddSwaggerGen`).
- Swagger üzerinden her endpoint için açıklamalar (`Summary`, `Description`) tanımlandı.
- Basit düzeyde `Rate Limiting` eklendi:
  - IP başına dakikada en fazla 10 istek.

### 🛑 Yapılmayanlar (Basitlik İçin)

- Authentication & Authorization (JWT, OAuth2 vs.) eklenmedi.
- Global error handling veya `ProblemDetails` yapılandırması yapılmadı.
- Endpoint versioning, pagination, filtering gibi REST best practiceleri uygulanmadı.
- Health checks, metrics, logging, tracing gibi production-ready özellikler dahil edilmedi.

### 🚀 Geliştirme Önerileri

- JWT Authentication + Authorization rolleri eklenebilir.
- Exception middleware ile global hata yönetimi sağlanabilir.
- Rate limiting stratejileri geliştirilebilir (token bucket, concurrency limiter).
- Endpoint versioning ve daha kapsamlı REST özellikleri uygulanabilir.
- OpenTelemetry, Serilog, Prometheus/Grafana ile monitoring artırılabilir.

---