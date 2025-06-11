# MAUINotificationApp

## Descripción general

**FirebasePushNotifications** es una aplicación de ejemplo desarrollada en **.NET MAUI** que ilustra cómo implementar autenticación de usuario, gestión de sesiones y notificaciones push usando **Firebase Cloud Messaging (FCM)** y **Azure Functions** como backend de servicios.

---

## Funcionalidades principales

- **Login y logout de usuario** con validación contra un backend.
- **Registro y actualización de dispositivos** en el backend junto con su token FCM.
- **Asociación y sincronización del token FCM** al usuario autenticado en el backend.
- **Suscripción automática a un topic general** de FCM para recibir notificaciones globales.
- **Envío de notificaciones de prueba** desde la propia aplicación mediante llamada al backend de prueba.
- **Gestión de sesiones persistentes** mediante almacenamiento local.
- **Navegación protegida** con control de acceso a las páginas principales.
- **Manejo de permisos de notificaciones** para Android 13+.

---

## Tecnologías utilizadas

- **.NET MAUI**
- **Firebase Cloud Messaging (FCM)** usando el plugin [`Plugin.Firebase.CloudMessaging`](https://github.com/CrossGeeks/Plugin.Firebase)
- **Azure Functions** para backend de autenticación y notificaciones
- **CommunityToolkit.Mvvm** para el patrón MVVM
- **Microsoft.Maui.Storage** para almacenamiento local de preferencias

---

## Pantallas de la aplicación

- **LoadingPage:** Verifica si el usuario está autenticado y redirige automáticamente.  
  <img src="https://github.com/user-attachments/assets/d9a91f44-a0f2-4f34-a02e-6c348c1b80ea" style="border:1px solid #ccc; border-radius:4px;" width="400"/>

- **LoginPage:** Formulario para introducir usuario y contraseña.  
  <img src="https://github.com/user-attachments/assets/8380943a-35e8-4fce-8dc8-5c944636cb20" style="border:1px solid #ccc; border-radius:4px;" width="400"/>

- **MainPage:** Página principal con botón para enviar notificaciones de prueba.  
  <img src="https://github.com/user-attachments/assets/7ace63ea-34a0-4fbf-9d28-d67c97b7204e" style="border:1px solid #ccc; border-radius:4px;" width="400"/>

- **ProfilePage:** Permite al usuario cerrar sesión.  
  <img src="https://github.com/user-attachments/assets/fde4179e-2a28-4873-b90d-4c46cd8630aa" style="border:1px solid #ccc; border-radius:4px;" width="400"/>



---

## Gestión de suscripciones y tokens FCM

### Flujo de suscripción y registro de tokens

1. **Obtención del token FCM:**
   - Al iniciar la app, se obtiene el token de notificaciones mediante `FirebaseCloudMessaging.GetTokenAsync()`.

2. **Registro del token vinculado al usuario:**
   - Tras el login, se envía el token FCM junto con el usuario al backend (Azure Functions).
   - El backend almacena el token asociado al usuario en su base de datos.

3. **Suscripción automática a topic general:**
   - Además del registro personalizado, la app suscribe automáticamente el dispositivo a un topic general (ej: `Bienvenido`) usando `SubscribeToTopicAsync()`.
   - Este topic permite notificaciones globales a todos los dispositivos suscritos.

4. **Envío de notificaciones personalizadas:**
   - Cuando se desea notificar a un usuario específico, el backend busca los tokens asociados y envía la notificación solo a esos dispositivos.
   - La Azure Function `sendnotifications` recibe el usuario destino, busca su token y envía la notificación personalizada.

---

## Guía paso a paso

[Guía MAUINotificationsApp.pdf](https://github.com/user-attachments/files/20690668/Guia.MAUINotificationsApp.pdf)

---

## Notas adicionales

- El proyecto sigue el patrón **MVVM** para separar lógica de presentación y de negocio.
- La navegación está protegida: solo usuarios autenticados pueden acceder a las páginas principales.
- El **almacenamiento local** se utiliza para gestionar la sesión del usuario de forma persistente.
- El backend controla el envío de notificaciones personalizadas gracias al registro de tokens asociados a cada usuario.

---

## Licencia

Este proyecto es un ejemplo educativo y puede ser adaptado libremente para tus propios experimentos y pruebas.
