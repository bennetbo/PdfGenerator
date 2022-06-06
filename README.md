# PdfGenerator

This web service generates PDF files with easy-to-understand and standard-compliant measurement parameters for software testing and custom usage.

## API Reference

#### Get all valid page size aliases

```http
  GET /pagesizes
```
returns a list of aliased `PageSize` like `A4`, `A0` or `LETTER`, ...

#### Generate a PDF document containing lorem-impsum sentences

##### by alias

```http
  GET /generate/${pagesize}/${pagecount}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `pagesize`      | `PageSize` | **Required**. Pagesize alias to use for file generation |
| `pagecount`     | `int`    | **Required**. Number of pages the document should have |

##### or by using explicit size arguments
```http
  GET /generate/${width}/${height}/${pagecount}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `width`      | `int` | **Required**. Page width to be used in document generation |
| `height`      | `int` | **Required**. Page height to be used in document generation |
| `pagecount`     | `int`    | **Required**. Number of pages the document should have |


### Contents

#### Get a PDF document with empty pages
```
  GET /generate/empty/${pagesize}/${pagecount}
```
```http
  GET /generate/empty/${width}/${height}/${pagecount}
```

#### Get a PDF document with gradient images
```http
  GET /generate/imaged/${pagesize}/${pagecount}
```
```http
  GET /generate/imaged/${width}/${height}/${pagecount}
```

#### Get a PDF document with an image with an adorable cat
```http
  GET /generate/imaged/cat/${pagesize}/${pagecount}
```
```http
  GET /generate/imaged/cat/${width}/${height}/${pagecount}
```

#### Get a PDF document with an image with an adorable cat
```http
  GET /generate/imaged/cat/${pagesize}/${pagecount}
```
```http
  GET /generate/imaged/cat/${width}/${height}/${pagecount}
```


## Commands
Run the following commands sequentially to resolve the application dependencies, build the application, and run the tests

### Restore Dependencies

To restore dependencies from NuGet

```powershell
  dotnet restore
```
### Build

To build the application and the test libary

```powershell
  dotnet build --no-restore
```

### Running Tests

To run tests, run the following command

```powershell
  dotnet test --no-build --verbosity normal
```

## Environment Variables

The application is configurable by customizing these environment variables:

| Parameter                     | Description                                             |
| :---------------------------- | :------------------------------------------------------ |
| `FILE_NAMING_SCHEMA`          | Naming schema used for creation of document filenames   | 

## Tech Stack

#### **Server:** Microsoft.AspNetCore, QuestPDF | **Client:** ... missing


## Badges

[![MIT License](https://img.shields.io/apm/l/atomic-design-ui.svg?)](https://github.com/tterb/atomic-design-ui/blob/master/LICENSEs)
