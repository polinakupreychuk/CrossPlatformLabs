Vagrant.configure("2") do |config|

  config.vm.define "windows" do |windows|
    windows.vm.box = "StefanScherer/windows_2019"
    windows.vm.communicator = "winrm"
    
    windows.vm.provider "virtualbox" do |vb|
      vb.name = "VirtualWin"
      vb.gui = true
      vb.memory = "8048" 
      vb.cpus = 4 
      vb.customize ["modifyvm", :id, "--vram", "128"] 
      vb.customize ["modifyvm", :id, "--natdnshostresolver1", "on"] 
      vb.customize ["modifyvm", :id, "--natdnsproxy1", "on"] 
      vb.customize ["modifyvm", :id, "--clipboard", "bidirectional"] 
    end

    windows.vm.network "forwarded_port", guest: 5050, host: 5052, auto_correct: true
    windows.vm.network "forwarded_port", guest: 5000, host: 5003, auto_correct: true
    windows.vm.network "forwarded_port", guest: 3389, host: 33389, auto_correct: true
    windows.vm.network "forwarded_port", guest: 5985, host: 55985, auto_correct: true
    
    windows.winrm.username = "vagrant"
    windows.winrm.password = "vagrant"
    windows.winrm.transport = :negotiate
    windows.winrm.basic_auth_only = false
    
    windows.vm.provision "shell", inline: <<-SHELL
      Set-ExecutionPolicy Bypass -Scope Process -Force
      [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072
      iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
      
      choco install dotnet-sdk -y --no-progress
      
      refreshenv
      
      dotnet nuget add source http://10.0.2.2:5000/v3/index.json -n "BaGet"
      dotnet tool install -g DMazur --version 1.0.5 --add-source http://10.0.2.2:5000/v3/index.json
    SHELL
  end

  config.vm.define "ubuntu" do |ubuntu|
    ubuntu.vm.box = "bento/ubuntu-22.04"
    ubuntu.vm.hostname = "UbuntuVM"
    ubuntu.vm.network "forwarded_port", guest: 7031, host: 7031
    ubuntu.vm.network "forwarded_port", guest: 1433, host: 1433
    ubuntu.vm.network "forwarded_port", guest: 7106, host: 7106
    ubuntu.vm.network "private_network", ip: "192.168.63.51"
    ubuntu.vm.provider "virtualbox" do |vb|
      vb.name = "UbuntuVM"
      vb.gui = false
      vb.memory = "8048"
      vb.cpus = 4
    end
    ubuntu.vm.synced_folder ".", "/home/vagrant/project"
    ubuntu.ssh.insert_key = false

    ubuntu.vm.provision "shell", inline: <<-SHELL
      sudo apt-get remove -y 'dotnet*' 'aspnet*' || true
      wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
      sudo dpkg -i packages-microsoft-prod.deb
      rm packages-microsoft-prod.deb
      sudo apt-get update
      sudo apt-get install -y dotnet-sdk-6.0

      dotnet --info || { echo "Failed to install .NET SDK"; exit 1; }
    SHELL

    ubuntu.vm.provision "shell", privileged: false, inline: <<-SHELL
      dotnet nuget add source http://10.0.2.2:5000/v3/index.json -n "BaGet"
      dotnet tool install -g PKupreychuk --version 1.0.5 --add-source http://10.0.2.2:5000/v3/index.json

      export PATH="$PATH:$HOME/.dotnet/tools"
      echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.bashrc
    SHELL
  end


  config.vm.define "macos" do |macos|
    macos.vm.box = "ramsey/macos-catalina"
    macos.vm.hostname = "MacOSVM"
    
    macos.vm.provider "virtualbox" do |vb|
      vb.name = "MacOSVM"
      vb.gui = true
      vb.memory = "8048"
      vb.cpus = 4
    end

    macos.vm.provision "shell", inline: <<-SHELL
      /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
      brew update
      brew install --cask dotnet-sdk

      curl -I http://10.0.2.2:5000/v3/index.json || echo "NuGet server not reachable"
    SHELL

    macos.vm.provision "shell", privileged: false, inline: <<-SHELL
      dotnet nuget add source http://10.0.2.2:5000/v3/index.json -n "BaGet"
      dotnet tool install -g DMazur --version 1.0.5 --add-source http://10.0.2.2:5000/v3/index.json

      echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.zshrc
      export PATH="$PATH:$HOME/.dotnet/tools"
    SHELL
  end
end
