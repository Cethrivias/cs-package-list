# C# Package List

Lists all packages installed in a project or solution

I created this project to teach myself how to write CLI Tools in **.NET**. Dont expect much.

---

## Installation

```shell
# Build and create a package
dotnet pack

# Install the package globally
dotnet tool install --global --add-source ./nupkg cs-package-list
```

---

## Removal

```shell
dotnet tool uninstall --global cs-package-list
```

---

## Usage

```shell
# Execute the tool in any project/solution you want
cs-package-list

# Lists packages excluding those that have "Microsoft" in their name
cs-package-list -e Microsoft

# For more details
cs-package-list --help
```