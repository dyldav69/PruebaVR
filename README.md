<div align="center">

# 🥽 MathScape

### Entorno inmersivo de Realidad Virtual para el fortalecimiento del aprendizaje del Precálculo

![Unity](https://img.shields.io/badge/Unity-6000.0.80f1-000000?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-Scripts-512BD4?style=for-the-badge&logo=csharp&logoColor=white)
![XR](https://img.shields.io/badge/XR%20Interaction%20Toolkit-3.1.3-1E88E5?style=for-the-badge)
![OpenXR](https://img.shields.io/badge/OpenXR-1.16.1-E53935?style=for-the-badge)
![Meta Quest](https://img.shields.io/badge/Meta%20Quest-2-0081FB?style=for-the-badge&logo=meta&logoColor=white)
![Uso](https://img.shields.io/badge/Uso-Acad%C3%A9mico-43A047?style=for-the-badge)

**🎓 Trabajo de grado · Fundación Universitaria Konrad Lorenz**

</div>

---

## 📖 Descripción

**MathScape** es un prototipo de alta fidelidad que busca fortalecer la comprensión conceptual y el aprendizaje significativo del **Precálculo** en estudiantes de primer y segundo semestre. Cada actividad relaciona de forma directa una mecánica de interacción en realidad virtual con un concepto matemático: 🔦 proyecciones y funciones, 🧗 máximos y mínimos, 📡 límites laterales.

| | |
| --- | --- |
| 👨‍💻 **Autores** | Angel Stiven Pinzón Sánchez y Dylan Gabriel Dávila Mojica |
| 👨‍🏫 **Director** | Ph.D. Jorge Luis Bacca Acosta |
| 🏛️ **Programa** | Ingeniería de Sistemas · Facultad de Matemáticas e Ingenierías |
| 🌊 **Metodología** | Cascada (Waterfall) |
| 📚 **Uso** | Exclusivamente académico |

## 🧭 Contenido

- [🔧 Tecnologías](#-tecnologías)
- [🎮 Actividades](#-actividades)
- [📁 Estructura del proyecto](#-estructura-del-proyecto)
- [🚀 Cómo ejecutar](#-cómo-ejecutar)
- [🎯 Controles](#-controles)
- [✅ Validación](#-validación)
- [📄 Documentación](#-documentación)
- [🙏 Créditos](#-créditos-y-recursos-de-terceros)

---

## 🔧 Tecnologías

| Componente | Versión |
| --- | --- |
| 🎮 Unity | 6000.0.80f1 (Unity 6) |
| 🤲 XR Interaction Toolkit | 3.1.3 |
| ⚙️ XR Plug-in Management | 4.5.4 |
| 🔌 OpenXR | 1.16.1 |
| 🥽 Oculus XR Plugin | 4.5.4 |
| 🧪 XR Mock HMD (simulación sin visor) | 1.5.0-exp.3 |
| 🎨 Universal Render Pipeline (URP) | 17.1.0 |
| 🧱 ProBuilder | 6.0.9 |
| 💻 Lenguaje | C# |

> 📱 **Dispositivo de prueba:** Meta Quest 2. También se puede probar sin visor con el simulador XR de Unity.

## 🎮 Actividades

La escena principal es `Assets/JuegoVR.unity`. Al iniciar, el estudiante ve un **menú 🏠** con tres botones (Nivel 1, 2 y 3) que lo trasladan al punto de inicio de cada actividad.

### 🔦 Actividad 1 · Escultura de sombras
*Funciones y sus representaciones*

El estudiante toma un objeto tridimensional con el controlador y lo rota hasta que su sombra, proyectada sobre un panel, coincide con la gráfica de la función de referencia.

- ✅ Cuando la rotación queda dentro de la tolerancia angular, el objeto se **fija**.
- 🔓 Se **desbloquea el paso al Nivel 2**.
- 🪧 Aparece un **cartel de completado**.

### 🧗 Actividad 2 · Sendero de la función
*Análisis de variación: máximos y mínimos*

El estudiante lee la misión del panel (por ejemplo, evaluar f(x) = −x² + 4x para x = 1, 2 y 3) y se desplaza por **teletransporte** entre plataformas rotuladas con valores de la función.

- 🚫 Las plataformas **no caen**: al pisar una plataforma incorrecta, el estudiante regresa al punto de inicio de la actividad.
- ⬇️ Si cae por debajo de la altura límite (−2 m por defecto), también regresa al punto de inicio.
- 💎 Debe recoger los **coleccionables** del recorrido (3 por defecto). Los ya recogidos se conservan tras un reinicio y, al reunirlos, aparece el **cuadro de respuesta**.
- 🪤 Los coleccionables falsos (objetos trampa) no cuentan y muestran el **cartel de pistas**.

### 📡 Actividad 3 · Sintonizar las antenas
*Límites laterales*

El estudiante agarra cada una de las dos antenas y la gira con el joystick mientras mantiene el gatillo sostenido. Cada antena gira de 0° a 180° y muestra sobre ella un valor de f(x) interpolado de una tabla configurable. La pantalla central compara ambas señales de forma continua:

- ✅ Si las dos antenas avanzaron y sus valores coinciden dentro de la tolerancia (0,1 por defecto), muestra **«LIMITE EXISTE»** con el valor del límite L (promedio de ambas señales).
- 📺 Si no coinciden, la señal se pierde y la pantalla vuelve a la **estática**.

### 🚪 Escenario

Las puertas corredizas se abren cuando el jugador entra en su zona de detección y se cierran cuando sale.

## 📁 Estructura del proyecto

```
📁 Assets/
├── 🎬 JuegoVR.unity          Escena principal del prototipo
├── 📁 Scripts/               Scripts propios en C#
│   └── 🖼️ Imagenes/          Texturas de los paneles de instrucciones y pistas
├── 📁 Videos/                Video de estática de la pantalla del Módulo 3
├── 📁 XR/, XRI/, Samples/    Configuración de XR y ejemplos del XR Interaction Toolkit
└── 📦 (paquetes de terceros) Ver la sección de créditos
```

### 📜 Scripts en uso

| Script | Función |
| --- | --- |
| 🏠 `MenuSelectorVR` | Traslada al jugador al punto de inicio de la actividad elegida y oculta el menú |
| 🔦 `ShadowObjectValidator` | Compara la rotación del objeto con la rotación objetivo (Actividad 1); al coincidir, lo fija, desbloquea el Nivel 2 y muestra el cartel |
| 🧗 `Activity2Manager` | Administra la Actividad 2: reinicio, detección de caídas, conteo de coleccionables y cuadro de respuesta |
| ⚠️ `Activity2Platform` | Detecta cuándo el jugador pisa una plataforma incorrecta y solicita el reinicio |
| 💎 `CollectibleObject` | Coleccionable de la Actividad 2 |
| 🪤 `FalseCollectibleObject` | Coleccionable falso (objeto trampa) que muestra el cartel de pistas |
| 📡 `AntennaSignalController` | Gira cada antena con el joystick y calcula el valor de f(x) mostrado |
| 📺 `TVLimitController` | Compara las dos antenas y muestra el resultado en la pantalla central |
| 🚪 `SlidingDoor` | Abre y cierra las puertas corredizas del escenario |

### 🗃️ Scripts de versiones anteriores

El repositorio conserva `ShadowObjectPlacement`, `AsteroidValidator`, `FallingPlatform` y `MostrarPistaAlAgarrar` (archivo `CollectiblePista.cs`). Pertenecen a versiones previas del diseño, como la caída de plataformas, y no están asignados a ningún objeto de la escena `JuegoVR`.

## 🚀 Cómo ejecutar

1. ⬇️ Instalar **Unity 6000.0.80f1** desde Unity Hub.
2. 📥 Clonar el repositorio y agregar la carpeta como proyecto en Unity Hub:
   ```bash
   git clone https://github.com/dyldav69/PruebaVR.git
   ```
3. ⏳ Abrir el proyecto y esperar a que Unity importe los paquetes.
4. 🎬 Abrir la escena `Assets/JuegoVR.unity`.
5. ▶️ Presionar **Play**:
   - 🖥️ **Sin visor:** el proyecto incluye el XR Device Simulator y el XR Interaction Simulator para probar la interacción con el teclado y el mouse.
   - 🥽 **Con Meta Quest 2:** conectar el visor y ejecutar la escena con el proveedor XR configurado en *Edit > Project Settings > XR Plug-in Management*.

> [!TIP]
> Si el programa no carga la pantalla de inicio al ejecutar, verifica que el XR Origin y los puntos de inicio estén asignados en el objeto `MenuManager` de la escena.

## 🎯 Controles

| Acción | Control |
| --- | --- |
| 🏠 Seleccionar un botón del menú | Apuntar con el controlador y confirmar |
| ✋ Agarrar un objeto o una antena | Gatillo (mantenerlo sostenido) |
| 🔄 Girar una antena | Joystick, eje horizontal, con la antena agarrada |
| 🧗 Desplazarse en la Actividad 2 | Teletransporte: apuntar a la plataforma y confirmar |

## ✅ Validación

Las pruebas funcionales de los casos de uso implementados las realizó el equipo de desarrollo, con el visor **Meta Quest 2** y con el **simulador XR de Unity**, sin participación de estudiantes.

> [!NOTE]
> El alcance del proyecto es el diseño y el prototipo de alta fidelidad. No se afirman resultados de aprendizaje en estudiantes.

## 📄 Documentación

El documento del trabajo de grado y sus anexos (diagramas de secuencia y de actividad de los diez casos de uso) se entregan por separado de este repositorio.

## 🙏 Créditos y recursos de terceros

El proyecto incluye recursos de terceros que conservan sus licencias originales, entre ellos: Unity Learn Course Library y los proyectos de retos (Challenges), SciFi Warehouse Kit, 3D Scifi Kit Starter Kit (Creepy Cat), Deep Space Skybox Pack, Basic Bedroom Pack (Mavi3D), Gems and Crystals (Aztech Games) y Sheep Run. Estos recursos no forman parte de la autoría de MathScape.

---

<div align="center">

🎓 **Fundación Universitaria Konrad Lorenz** · Bogotá, D.C., Colombia · 2026

</div>
