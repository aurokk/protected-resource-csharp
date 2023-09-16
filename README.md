# Beam

Beam это апишка-пустышка, в которую встроен мидлвар для авторизации через denji.

В качестве отправной точки взят
[IdentityServer4.AccessTokenValidation](https://github.com/IdentityServer/IdentityServer4.AccessTokenValidation).

Стек: C#, .NET7.

## Dependencies

1. `dotnet sdk >= 7.0.400`
2. `postgreSQL`
3. `docker & docker compose`

## Build & Run

1. Запустить апи: `Api/Properties/launchSettings.json` конфигурацию `Api`.

## Contributing

Изменения в проекте приветствуются в соответствии с [правилами](https://github.com/yaiam/.github/blob/main/CONTRIBUTING.md).