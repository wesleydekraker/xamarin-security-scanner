![header](https://github.com/infosupport/xamarin-security-scanner/blob/master/Docs/header.png?raw=true)

A tool to find security vulnerabilities in Xamarin.Android apps. It finds vulnerabilities by analyzing the source code (SAST).

It is inspired by and contains code from QARK (Quick Android Review Kit).

## Getting Started

The quickest way to get started is to use Docker.


```
git clone <project_url>
cd xamarin-security-scanner
docker build ./XamarinSecurityScanner -t xamarin-security-scanner
docker run -v <absolute_path_to_project>:/project xamarin-security-scanner
```

Another option is to install .NET Core 2.2, and run the following commands:

```
git clone <project_url>
cd xamarin-security-scanner
dotnet run --project .\XamarinSecurityScanner\XamarinSecurityScanner.App --path <path_to_project>
```

Example output:

![screenshot](https://github.com/infosupport/xamarin-security-scanner/blob/master/Docs/screenshot.png?raw=true)

## Usage

```
Usage: XamarinSecurityScanner.App [options]

Options:
  -p|--path <PATH>                Path to scan
  -t|--threshold <THRESHOLD>      Vulnerability threshold
  -e|--enable-logging             Enable logging
  -i|--ignore-file <IGNORE_FILE>  Path to ignore file
  -?|-h|--help                    Show help information
```

For more information on how to use the Xamarin Security Scanner, see the [configuration docs](/Docs/Configuration.md).

## Functionality

The tool reports the following issues:
- Certificate validation overwritten
- Permissions may not be enforced
- Unsafe cipher mode used
- External storage is used
- Hardcoded HTTP URL found
- JavaScript enabled in WebView
- JavascriptInterface is added to a WebView
- Logging was found
- Access to phone number
- WorldReadable file found
- Backups are enabled
- App has debugging enabled
- App supports outdated Android version
- App contains a private key

## Credits

Marco Kuiper (@marcofolio) - For the logo.
