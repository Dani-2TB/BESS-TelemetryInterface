# Development Environment Setup

This guide provides you with steps on how to setup your local development environment for **BESS-TelemetryInterface** which targets an embedded Raspberry Pi running raspbian.

> ** Note **
> The project has been developed primarily on Linux (Debian and Fedora)
> If you are on Windows, it is **strongly recommended** to use [WSL2 with Debian](https://learn.microsoft.com/en-us/windows/wsl/install). Providing steps for setting up WSL is outside the scope of this document.

## 1. Preliminary Steps

Before installing project dependencies, ensure your environment is correctly configured

### 1.1 Choose an environment

- **Preferred** Native Linux (Debian based distributions are recommended, but you can use any distribution you like)

- Make sure your system is up to date, on debian based distros do:

``` bash
sudo apt update && sudo apt upgrade
```

### 1.2 Install Tooling

#### **Git**: Version Control

Install with your package manager, for example on debian based distros with apt
``` bash
sudo apt install git
```

#### **Docker**: This project works in tandem with a data pipeline provided through a docker environment. The files and configuration of this environment are located [here](https://github.com/Dani-2TB/BESS-Docker-Environment). For instructions on installing the docker engine on your system you can follow the official [docker documentation](https://docs.docker.com/engine/install/) for your system. For instructions on debian go to [this link](https://docs.docker.com/engine/install/debian/)

#### **SQLite3**: Database

This project uses SQLite3 as a local database. You can install it on debian with:

``` bash
sudo apt install sqlite3
```

Dotnet might want to try and use a version of SQLite3 that comes bundled with some packages. This step might prevent any issues that may arise from this. You can continue with section 1.3 now, but if you face any issues regarding SQLite3, you can install your system's build dependencies. On debian install build-essential with:

``` bash
sudo apt install build-essential
```

This should fix any issues you might encounter later running the development build.

### 1.3 System Check

To quickly confirm all tooling is installed correctly you can run the following commands

``` bash
git --version
```

``` bash
docker --version
```

``` bash
sqlite3 --version
```

All og this commands should

### 1.4 Install Project Dependencies

#### Dotnet 9.0: 
This is an [ASP.NET](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-9.0) project using [Razor](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-9.0) pages, [Entity Framework](https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/intro?view=aspnetcore-9.0&tabs=visual-studio-code) and ASP's [Minimal API](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-9.0). It is recommended to install dotnet through their [installation scripts](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script)

After you aquire the script you might have to give it permissions with

``` bash
chmod +x dotnet-install.sh
```

Then you can install dotnet 9.0 with this command

``` bash
./dotnet-install.sh -Channel 9.0
```

This will install the latest version of dotnet 9.0 to your machine, afterwards you might want to setup your environment variables in path. If you are using bash, you can run this command to do so:

``` bash
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools' >> ~/.bashrc
```

If using another shell add this lines to your rc file. For zsh is as simple as replacing ```~/.bashrc``` with ```~/.zshrc``` on the previous command. Instructions for other shells are outside the scope of this document.

> **Note**: If you want to use dotnet's Razor [scaffolding](https://learn.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/page?view=aspnetcore-9.0&tabs=visual-studio), you will have to install the LTS version of dotnet. At the time, that is dotnet 8.0
> You can do so with the following command using the installation script ```./dotnet-install.sh -Channel 8.0```

## 2. Cloning The Repo

Navigate to a directory of your choosing to host the project files. This dev team uses one directory for hosting the project's repos.

Once there clone the repo with:

``` bash
git clone https://github.com/Dani-2TB/BESS-TelemetryInterface
```

And then:

``` bash
cd BESS-TelemetryInterface
```

## 3. Project Structure

The project structure can be found in it's corresponding documentation file: **TO DO**

## 4. Setting Up The Project

First install all dependencies for the dotnet project running:

``` bash
dotnet build
```

### 4.1 Running Migrations And Setting Up The Database

This project uses Entity Framework Core as the ORM. Initialize the database you will have to run migrations. You have two options:

- **Delete all migrations and create a new one:** Since this project does not depend on migrations for the initial setup, you can just remove the migrations inside the Migrations directory. And create a new one with:

``` bash
dotnet ef migration add InitialCreate
```

> **Note:** You can replace the name of the migration, but do follow the PascalCase naming convention.

- **Run migrations:** If you created your own initial migration, or wish to run the migrations included on this repo. Just run:

``` bash
dotnet ef database update
```

This will setup your database.

### 4.2 Setting Up The Telemetry Pipeline (optional)

At the moment, this project only requires the SQLite3 database. This database stores the configuration options for the Battery Energy Storage System (BESS) and its components. So to test the administration interface (Razor Pages) you don't *need* to setup the [BESS-Docker-Environment](https://github.com/Dani-2TB/BESS-Docker-Environment). But, to test and verify that the dashboard embedded on the Index of the user interface works correctly, you should follow the instructions provided in the repo for the environment.

### 4.3 Setting Up User Secrets (TODO)

For external communication, this project will implement an API using JWT, this token should be stored as a User Secret. For now it's not used, but you can read more about it [here](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-9.0&tabs=linux). If you are using VSCode, you can right click the .csproj file and "Manage User Secrets" to store the JWT secret. You can generate a key for it if you have openssl with this command:

``` bash
openssl rand -base64 32
```

This will generate the required JWT key for you to personally store. **Do not share this key with anyone**

## 5. Run The Project

Now that you have everything setup you can run the project using:

``` bash
dotnet run
```

This will run a development build of the project on a test server, but, any changes made will require you to kill the process and run the command again. For most changes, you can setup hot reload using ```dotnet watch``` like so

``` bash
dotnet watch run
```

This will auto reload the project after most changes, if the changes need more than a hot reload, the program will tell you and you can restart it with Ctrl-R

## 6. Troubleshooting

This section will provide troubleshooting steps for common problems that developers report. For now, the rest of the document should cover everything to get you setup.