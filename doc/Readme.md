# Documentación

> Introduzca sus datos (de todo el equipo) en la siguiente tabla:

**Nombre** | **Grupo** | **Github**
--|--|--
Jorge Yero Salazar | C412 | [@jyeros](https://github.com/jyeros)
Jose Diego Menendez Del Cueto | C412 | [@Jose10go](https://github.com/Jose10go)
Jose Ariel Romero Acosta | C412 | [@JoseA132](https://github.com/JoseA132)

## Readme

La implementación del presente compilador de COOL fue hecha en C# sobre [.NetCore](https://dotnet.microsoft.com/) Para compilar el código COOL contenido en un fichero de entrada `.cl` y obtener su código MIPS en un archivo de salida con el mismo nombre pero extensión `.mips` se debe ejecutar el script `coolc.sh` que se encuentra en el directorio `src` pasando como parámetro el path del fichero de entrada, por ejemplo

```bash
    ./coolc.sh <path-to-cool-file>
```

En el ejemplo anterior se supone que la terminal se está ejecutando en el directorio `src` del proyecto, se debe sustituir `<path-to-cool-file>` por el path del código COOL `.cl`.

Para lograr ejecutar el proyecto es necesario tener instalado `.NetCore`, el proyecto fue hecho utilizando la versión [LTS 2.1](https://dotnet.microsoft.com/download/dotnet-core/2.1), se recomienda utilizar la misma, aunque una posterior podría funcionar, luego es necesario hacer `make`, lo cual instalará todos los paquetes **(nuget)** necesarios para la ejecución del proyecto.

Si desea ver el compilador en acción dentro de la carpeta test se ha incluido un proyecto `xunit` de `.Net Core` el cual puede correr ejecutando `make tests` en la carpeta raíz del proyecto, entre los test se encuentran desde test básicos probando las funcionalidades del compilador hasta test mucho más complejos utilizando algoritmos de ordenación, sub cadenas y aspectos de la `POO`. Para correr los test es necesario tener instalado `spim` y si se está ejecutando en Windows es necesario tener instalado `wsl` por lo que se necesita Windows 10 version 1703 o posterior y `spim` dentro del `wsl`. Estos se corrieron utilizando Ubuntu 18.04 como SO y dentro del `wsl`.

## Más Información

Para más información acerca de la implementación del compilador puede consultar el fichero `/doc/report.pdf`.
