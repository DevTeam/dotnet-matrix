# Library logos

Most files are the official package icon, taken from the icon that the NuGet package itself
declares (`<icon>` for an embedded file, `<iconUrl>` when the package only links one):

| File | Package | Source |
|---|---|---|
| `autofac.png` | `Autofac` 9.3.1 | embedded icon |
| `catel.png` | `Catel.Core` 6.2.0 | embedded icon |
| `dry-ioc.png` | `DryIoc.dll` 5.4.3 | embedded icon |
| `grace.png` | `Grace` 7.2.1 | linked icon |
| `lamar.png` | `Lamar` 16.0.0 | linked icon, downscaled to 128x128 |
| `microsoft-di.png` | `Microsoft.Extensions.DependencyInjection` 10.0.10 | embedded icon |
| `mvvm-cross.png` | `MvvmCross` 10.1.2 | embedded icon |
| `ninject.png` | `Ninject` 3.3.6 | linked icon |
| `pure-di.png` | `Pure.DI` 2.5.2 | embedded icon |
| `singularity.png` | `Singularity` 0.18.0 | embedded icon, re-encoded at 128x128 |
| `spring.png` | `Spring.Core` 3.0.3 | embedded icon |
| `stashbox.png` | `Stashbox` 5.20.0 | embedded icon |
| `unity.png` | `Unity` 5.11.10 | linked icon, downscaled to 128x128 |
| `vs-mef.png` | `Microsoft.VisualStudio.Composition` 17.13.41 | embedded icon |
| `windsor.png` | `Castle.Windsor` 6.0.0 | embedded icon |
| `zen-ioc.png` | `ZenIoc` 1.0.1 | embedded icon |

The remaining files are neutral project-owned marks drawn for this comparison, because those
packages ship no icon of their own:

- `faster-ioc.svg` — `Faster.Ioc` declares no icon.
- `light-inject.svg` — `LightInject` declares no icon.
- `maestro.svg` — `Maestro` declares no icon.
- `simple-injector.svg` — `SimpleInjector` declares no icon.
- `structure-map.svg` — `StructureMap` declares no icon.
- `mef2.svg` — `System.Composition` ships the generic .NET package icon, byte-identical to the
  `Microsoft.Extensions.DependencyInjection` one, so a distinct mark is used instead.

Each drawn mark uses a light plate with dark lettering so it stays legible on light and dark
application backgrounds.

The names and logos remain the property of their respective owners and are used
only to identify the libraries in the comparison UI.
