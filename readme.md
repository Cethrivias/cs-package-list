# C# Package List

Lists all packages installed in a project or solution

I created this project to teach myself how to write CLI Tools in **.NET**. Dont expect much.

---

## Installation

```shell
# Build and create a package
dotnet pack

# Install the package globally
dotnet tool install --global --add-source ./nupkg Cli
```

---

## Removal

```shell
dotnet tool uninstall --global cli
```

---

## Usage

```shell
# Execute the tool in any project/solution you want
cs-package-list

cs-package-list -e Microsoft

cs-package-list --help
```