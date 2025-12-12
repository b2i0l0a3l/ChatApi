# 🚀 ChatApi - Professional Real-Time Chat API

![.NET 9](https://img.shields.io/badge/.NET-9.0-blue)
![License: MIT](https://img.shields.io/badge/License-MIT-green)
![Architecture](https://img.shields.io/badge/Architecture-Clean-orange)

A **modern, scalable, and professional** real-time chat API built with **Clean Architecture**, **SignalR**, and **Entity Framework Core**.

---

## ✨ الميزات الجديدة (v2.0)

### 🎯 تحسينات API الاحترافية

- ✅ **استجابات موحدة** - `ApiResponse<T>` wrapper لجميع الـ endpoints
- ✅ **معالجة أخطاء عالمية** - Global Exception Middleware
- ✅ **تسجيل الطلبات** - Request/Response Logging Middleware
- ✅ **حماية من الإساءة** - Rate Limiting Middleware
- ✅ **فحص الصحة** - Health Check endpoints
- ✅ **إصدارات API** - API Versioning (v1)
- ✅ **توثيق Swagger محسّن** - مع دعم JWT
- ✅ **رؤوس أمان** - Security Headers

### 💬 تحسينات SignalR Hub

- ✅ **تتبع المستخدمين المتصلين** - قائمة فورية بالمستخدمين Online
- ✅ **إشعارات فورية** - إشعارات عند استلام رسائل جديدة
- ✅ **مؤشر الكتابة** - Typing indicators
- ✅ **إدارة اتصال محسّنة** - معالجة أفضل للاتصال والانقطاع
- ✅ **أحداث منظمة** - Events مع بيانات منظمة
- ✅ **سجلات مفصلة** - Logging شامل مع emojis

### 🎨 Controllers المحسّنة

جميع الـ Controllers تم تحديثها مع:

- ✅ XML Documentation
- ✅ Structured responses
- ✅ Better error handling
- ✅ Comprehensive logging
- ✅ RESTful endpoints

---

## 📋 جدول المحتويات

- [البنية المعمارية](#-البنية-المعمارية)
- [البدء السريع](#-البدء-السريع)
- [API Endpoints](#-api-endpoints)
- [SignalR Hub](#-signalr-hub)
- [المصادقة](#-المصادقة)
- [الأمثلة](#-الأمثلة)
- [التكوين](#-التكوين)

---

## 🏗️ البنية المعمارية

```
ChatApi/
├── ChatApi.Api/              # API Layer - Controllers & Middleware
│   ├── Controllers/          # API Controllers (v1)
│   ├── Middleware/           # Custom Middleware
│   │   ├── GlobalExceptionMiddleware.cs
│   │   ├── RequestLoggingMiddleware.cs
│   │   └── RateLimitingMiddleware.cs
│   └── Common/               # Shared API Components
│       └── ApiResponse.cs
│
├── ChatApi.Application/      # Application Layer - Business Logic
│   ├── Interfaces/           # Service Interfaces
│   └── Services/             # Service Implementations
│
├── ChatApi.Core/             # Domain Layer - Entities & Interfaces
│   ├── Entities/             # Domain Entities
│   └── Interfaces/           # Repository Interfaces
│
└── ChatApi.Infrastructure/   # Infrastructure Layer - Data & External
    ├── Hubs/                 # SignalR Hubs
    │   └── ChatHub.cs        # Professional Chat Hub
    ├── Persistence/          # Database Context & Repositories
    └── Identity/             # Authentication & Authorization
```

---

## 🚀 البدء السريع

### المتطلبات

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQLite / SQL Server / PostgreSQL
- (اختياري) Docker

### التثبيت

```bash
# 1. استنساخ المشروع
git clone https://github.com/yourusername/ChatApi.git
cd ChatApi

# 2. استعادة الحزم
dotnet restore

# 3. تطبيق Migrations
dotnet ef database update --project ChatApi.Infrastructure

# 4. تشغيل API
dotnet run --project ChatApi.Api
```

### الوصول للتطبيق

- **API**: https://localhost:5230
- **Swagger UI**: https://localhost:5230 (في Development mode)
- **SignalR Hub**: https://localhost:5230/chatHub
- **Health Check**: https://localhost:5230/health

---

## 📡 API Endpoints

### 🔐 Authentication (`/api/v1/auth`)

| Method | Endpoint    | Description       | Auth |
| ------ | ----------- | ----------------- | ---- |
| POST   | `/register` | تسجيل مستخدم جديد | ❌   |
| POST   | `/login`    | تسجيل الدخول      | ❌   |
| POST   | `/refresh`  | تحديث Token       | ❌   |
| POST   | `/logout`   | تسجيل الخروج      | ✅   |

**مثال - التسجيل:**

```json
POST /api/v1/auth/register
Content-Type: multipart/form-data

{
  "userName": "john_doe",
  "email": "john@example.com",
  "password": "SecurePass123!",
  "phoneNumber": "+1234567890"
}

Response:
{
  "success": true,
  "message": "User registered successfully",
  "data": {
    "userId": "guid-here",
    "userName": "john_doe",
    "email": "john@example.com"
  },
  "timestamp": "2025-12-03T19:25:00Z"
}
```

---

### 💬 Messages (`/api/v1/chat`)

| Method | Endpoint        | Description              | Auth |
| ------ | --------------- | ------------------------ | ---- |
| GET    | `/messages`     | الحصول على الرسائل       | ✅   |
| GET    | `/unread-count` | عدد الرسائل غير المقروءة | ✅   |
| GET    | `/search`       | البحث في الرسائل         | ✅   |

**مثال - الحصول على الرسائل:**

```json
GET /api/v1/chat/messages?conversationId=guid-here&pageNumber=1&pageSize=20
Authorization: Bearer {token}

Response:
{
  "success": true,
  "message": "Messages retrieved successfully",
  "data": [
    {
      "id": "msg-guid",
      "content": "Hello!",
      "senderId": "user-guid",
      "senderName": "John Doe",
      "timestamp": "2025-12-03T19:20:00Z",
      "isRead": true
    }
  ],
  "metadata": {
    "currentPage": 1,
    "pageSize": 20,
    "totalPages": 5,
    "totalCount": 95
  }
}
```

---

### 💭 Conversations (`/api/v1/conversation`)

| Method | Endpoint | Description             | Auth |
| ------ | -------- | ----------------------- | ---- |
| GET    | `/me`    | محادثات المستخدم الحالي | ✅   |
| PUT    | `/title` | تحديث عنوان المحادثة    | ✅   |
| DELETE | `/{id}`  | حذف محادثة              | ✅   |

---

### 👥 Participants (`/api/v1/participant`)

| Method | Endpoint             | Description         | Auth |
| ------ | -------------------- | ------------------- | ---- |
| GET    | `/conversation/{id}` | المشاركون في محادثة | ✅   |

---

### 👤 Profile (`/api/v1/profile`)

| Method | Endpoint    | Description             | Auth |
| ------ | ----------- | ----------------------- | ---- |
| GET    | `/me`       | معلومات المستخدم الحالي | ✅   |
| GET    | `/{userId}` | معلومات مستخدم محدد     | ✅   |

---

### 🏥 Health (`/api/v1/health`)

| Method | Endpoint   | Description     | Auth |
| ------ | ---------- | --------------- | ---- |
| GET    | `/`        | فحص صحة النظام  | ❌   |
| GET    | `/version` | معلومات الإصدار | ❌   |
| GET    | `/ping`    | فحص الاستجابة   | ❌   |

---

## 🔌 SignalR Hub

### الاتصال

```javascript
const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:5230/chatHub", {
    accessTokenFactory: () => localStorage.getItem("token"),
  })
  .withAutomaticReconnect()
  .build();

await connection.start();
```

### الأحداث الرئيسية

#### 📨 إرسال رسالة

```javascript
await connection.invoke("SendMessage", receiverId, "Hello!");
```

#### 📬 استلام رسالة

```javascript
connection.on("ReceiveMessage", (messageData) => {
  console.log("New message:", messageData);
});
```

#### 🔔 إشعار برسالة جديدة

```javascript
connection.on("NewMessageNotification", (notification) => {
  showNotification(notification.title, notification.message);
});
```

#### 👥 المستخدمون المتصلون

```javascript
// الحصول على القائمة
const users = await connection.invoke("GetOnlineUsers");

// الاستماع للتحديثات
connection.on("OnlineUsersList", (data) => {
  updateUsersList(data.users);
});

connection.on("UserConnected", (user) => {
  addUserToList(user);
});

connection.on("UserDisconnected", (user) => {
  removeUserFromList(user);
});
```

#### ✍️ مؤشر الكتابة

```javascript
// إرسال
await connection.invoke("UserTyping", conversationId, receiverId);
await connection.invoke("UserStoppedTyping", conversationId, receiverId);

// استلام
connection.on("UserTyping", (data) => {
  showTypingIndicator(data.userName);
});
```

للمزيد من التفاصيل، راجع [دليل ChatHub الشامل](./CHATHUB_GUIDE.md)

---

## 🔐 المصادقة

### الحصول على Token

```bash
curl -X POST https://localhost:5230/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "john_doe",
    "password": "SecurePass123!"
  }'
```

### استخدام Token

```bash
curl -X GET https://localhost:5230/api/v1/profile/me \
  -H "Authorization: Bearer {your-token-here}"
```

---

## 💡 الأمثلة

### مثال كامل - تطبيق دردشة

```javascript
class ChatApplication {
  constructor() {
    this.connection = null;
    this.token = null;
  }

  async login(username, password) {
    const response = await fetch("/api/v1/auth/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ userName: username, password }),
    });

    const result = await response.json();
    if (result.success) {
      this.token = result.data.accessToken;
      localStorage.setItem("token", this.token);
      await this.connectToHub();
    }
  }

  async connectToHub() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("/chatHub", {
        accessTokenFactory: () => this.token,
      })
      .withAutomaticReconnect()
      .build();

    // Register events
    this.connection.on("ReceiveMessage", (msg) => this.onMessage(msg));
    this.connection.on("NewMessageNotification", (n) => this.onNotification(n));
    this.connection.on("OnlineUsersList", (data) =>
      this.updateOnlineUsers(data)
    );

    await this.connection.start();
    console.log("Connected!");
  }

  async sendMessage(receiverId, message) {
    await this.connection.invoke("SendMessage", receiverId, message);
  }

  onMessage(messageData) {
    // Display message in UI
    console.log("New message:", messageData);
  }

  onNotification(notification) {
    // Show browser notification
    new Notification(notification.title, {
      body: notification.message,
    });
  }

  updateOnlineUsers(data) {
    // Update online users list in UI
    console.log("Online users:", data.users);
  }
}

// Usage
const chat = new ChatApplication();
await chat.login("john_doe", "password");
await chat.sendMessage("receiver-guid", "Hello!");
```

---

## ⚙️ التكوين

### appsettings.json

```json
{
  "ConnectionStrings": {
    "MyCon": "Data Source=ChatApi.db"
  },
  "JWT": {
    "ValidAudience": "https://localhost:7014",
    "ValidIssuer": "https://localhost:7014",
    "Secret": "your-secret-key-here"
  },
  "AllowedOrigins": ["http://localhost:3000", "http://localhost:5173"],
  "ApiSettings": {
    "MaxRequestsPerMinute": 60,
    "EnableRateLimiting": true,
    "EnableRequestLogging": true
  }
}
```

---

## 🛡️ الأمان

- ✅ JWT Authentication
- ✅ HTTPS Enforcement
- ✅ CORS Configuration
- ✅ Rate Limiting
- ✅ Security Headers
- ✅ Input Validation
- ✅ SQL Injection Protection (EF Core)

---

## 📊 المراقبة

### Health Check

```bash
curl https://localhost:5230/health
```

### Logs

جميع العمليات مسجلة مع مستويات مختلفة:

- `Information` - عمليات عادية
- `Warning` - تحذيرات
- `Error` - أخطاء

---

## 🚀 النشر

### Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY . .
EXPOSE 80
ENTRYPOINT ["dotnet", "ChatApi.Api.dll"]
```

```bash
docker build -t chatapi .
docker run -p 8080:80 chatapi
```

---

## 📝 الترخيص

MIT License - راجع [LICENSE](LICENSE) للتفاصيل

---

## 🤝 المساهمة

المساهمات مرحب بها! يرجى:

1. Fork المشروع
2. إنشاء branch للميزة (`git checkout -b feature/AmazingFeature`)
3. Commit التغييرات (`git commit -m 'Add AmazingFeature'`)
4. Push للـ branch (`git push origin feature/AmazingFeature`)
5. فتح Pull Request

---

## 📞 الدعم

- 📧 Email: support@chatapi.com
- 🐛 Issues: [GitHub Issues](https://github.com/yourusername/ChatApi/issues)
- 📖 Documentation: [Wiki](https://github.com/yourusername/ChatApi/wiki)

---

## 🎯 الخطط المستقبلية

- [ ] Message encryption
- [ ] File sharing
- [ ] Voice messages
- [ ] Video calls
- [ ] Group chats
- [ ] Message reactions
- [ ] Read receipts
- [ ] Message search
- [ ] User blocking
- [ ] Admin panel

---

**تم التحديث:** 2025-12-03  
**الإصدار:** 2.0.0  
**المطور:** ChatApi Team

---

⭐ إذا أعجبك المشروع، لا تنسى إعطائه نجمة على GitHub!
