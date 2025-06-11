# FirebasePushNotifications


## Descripción general

Esta aplicación es un proyecto de ejemplo construido con **.NET MAUI** que muestra cómo implementar autenticación de usuario, gestión de sesiones y notificaciones push, empleando **Firebase Cloud Messaging (FCM)** y **Azure Functions** como backend de servicios.

---

## Funcionalidades principales

- **Login y logout** de usuario con validación contra un backend.
- **Registro y actualización de dispositivos** en el backend con su token FCM.
- **Suscripción automática** a un topic de FCM para recibir notificaciones.
- **Envío de notificaciones de prueba** desde la propia aplicación mediante llamada al backend.
- **Gestión de sesiones persistentes** mediante almacenamiento local.
- **Navegación protegida** con control de acceso a las páginas principales.
- **Manejo de permisos de notificaciones** para Android 13+.

---

## Tecnologías utilizadas

- [.NET MAUI](https://learn.microsoft.com/dotnet/maui/)
- [Firebase Cloud Messaging (FCM)](https://firebase.google.com/docs/cloud-messaging) usando el plugin [`Plugin.Firebase.CloudMessaging`](https://github.com/Baseflow/Plugin.Firebase)
- [Azure Functions](https://azure.microsoft.com/en-us/products/functions/) para backend de autenticación y notificaciones
- [CommunityToolkit.Mvvm](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/) para el patrón MVVM
- [Microsoft.Maui.Storage](https://learn.microsoft.com/dotnet/api/microsoft.maui.storage) para almacenamiento local de preferencias
- Inyección de dependencias a través de `MauiProgram`

---

## Pantallas de la aplicación

- **LoadingPage**: Verifica si el usuario está autenticado y redirige automáticamente.
- **LoginPage**: Formulario para introducir usuario y contraseña.
- **MainPage**: Página principal con botón para enviar notificaciones de prueba.
- **ProfilePage**: Permite al usuario cerrar sesión.

---

## Configuración previa necesaria

### 1. Configuración de Firebase

- Crear un proyecto en [Firebase Console](https://console.firebase.google.com/).
- Configurar Firebase Cloud Messaging.
- Descargar y agregar el archivo `google-services.json` al proyecto (para Android).
- Asegurarse de tener correctamente configurado el plugin `Plugin.Firebase.CloudMessaging`.

### 2. Configuración del backend (Azure Functions)

- Desarrollar y desplegar las funciones necesarias: `login`, `logout`, `registerdevice` y `sendnotifications`.
    - Estas funciones reciben los parámetros a través de query strings.
- Configurar correctamente las URLs y claves en el código de la app.

### 3. Ajuste de constantes en el código

- `FunctionBaseUrl`: URL base de las Azure Functions.
- `FunctionKey`: Clave de autorización para las funciones.
- `DemoUser`: Usuario de prueba utilizado en la app.

---

## Cómo ejecutar la aplicación

1. **Clona el repositorio** en tu equipo.
2. **Restaura los paquetes NuGet**.
3. **Configura los valores de** `FunctionBaseUrl` **y** `FunctionKey` **en el código**.
4. **Compila y ejecuta** la aplicación en el emulador o dispositivo.
5. **Concede permisos de notificaciones** si se ejecuta en Android 13 o superior.

---

## Estructura general del proyecto

```
├── Services/              # Lógica de autenticación
├── ViewModels/            # Lógica de presentación para las páginas
├── Views/
│   ├── Authentication/    # Páginas de login y registro
│   └── User/              # Páginas de usuario (perfil, principal)
├── MainPage.xaml          # Página principal para pruebas de notificaciones
├── AppShell.xaml          # Navegación de la aplicación
├── MauiProgram.cs         # Configuración de dependencias y servicios
└── App.xaml.cs            # Inicialización de la aplicación
```

---

## Notas adicionales

- El proyecto sigue el patrón **MVVM** para separar la lógica de presentación y de negocio.
- La navegación está protegida: solo usuarios autenticados pueden acceder a las páginas principales.
- El almacenamiento local se utiliza para gestionar la sesión del usuario de manera persistente.
- El envío de notificaciones de prueba permite validar la integración de FCM y Azure Functions.

---

## Licencia

Este proyecto es un ejemplo educativo y puede ser adaptado libremente para tus propios experimentos y pruebas.
