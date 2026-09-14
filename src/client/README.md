# GoFit Client

Angular CLI workspace (generated with Angular CLI version 18.2.12) holding two separate application projects that share one codebase:

- `projects/admin` — the admin app (exercise catalog, permission management).
- `projects/athlete` — the athlete app (workout plans, workout tracking, profile).
- `src/shared` — code shared between both apps (services, guards, interceptors, models, constants, generic UI components, environment config), imported via the `@gofit/shared/*` path alias.

There is no default project — every `ng`/`npm` command below must name `admin` or `athlete` explicitly.

## Development server

```
npm run start:admin      # https://localhost:4201/
npm run start:athlete    # https://localhost:4200/
```

Run both in separate terminals to work on the two apps at once. Each reloads automatically on source changes.

## Code scaffolding

Run `ng generate component component-name --project=admin` (or `--project=athlete`) to generate a new component in that app. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

```
npm run build:admin
npm run build:athlete
```

Build artifacts are stored in `dist/admin/` and `dist/athlete/` respectively.

## Running unit tests

```
npm run test:admin
npm run test:athlete
```

Runs each app's unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via a platform of your choice. To use this command, you need to first add a package that implements end-to-end testing capabilities.

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.
