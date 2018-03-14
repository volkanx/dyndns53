![DynDns53 Logo](/assets/DynDns53Logo_200x116.png)

# DynDns53 [![Build Status](https://travis-ci.org/volkanx/dyndns53.svg?branch=master)](https://travis-ci.org/volkanx/dyndns53)

DynDns53 is a dynamic DNS tool using AWS Route 53. Your domains need to be hosted on AWS Route53. It has several clients:

* Angular 5
* .NET Core console application
* Docker Image running .NET Core application
* WPF Client
* .NET Framework Console application / Windows Service


## Prerequisites
To be able to use any of these clients, you'd need an IAM client user that has access to **route53:ListResourceRecordSets** and **route53:ChangeResourceRecordSets** actions. You can use the template below to give minimum required access to your user

```json
{
  "Version": "2012-10-17",
  "Statement": [
      {
          "Effect": "Allow",
          "Action": [
              "route53:ListResourceRecordSets",
              "route53:ChangeResourceRecordSets"
          ],
          "Resource": "arn:aws:route53:::hostedzone/{ZONE ID}"
      }
  ]
}
```

## NuGet Package
All the provided .NET clients use core library which is in NuGet. The library is called DynDns53.CoreLib and you can develop your own clients based on that.

```
Install-Package DynDns53.CoreLib
```

The library targets .NET Standard 2.0 so can be used in any .NET runtime compatible.


## Angular 5

The website for the client is here. You can simply enter your keys and domains here and start using it right away. The keys are stored locally on the browser so nobody will have access to them.

![](/assets/Angular5-Overview.png)

For more detial instructions visit [](http://dyndns53.myvirtualhome.net/) and click "Show Usage" to expand the information section.


## .NET Core Console Application
Main functionality is provided by core library. In order to use the console application you have to provide the necessary parameters. The usage is 

```
dotnet DynDns53.Client.DotNetCore.dll --AccessKey ACCESS_KEY --SecretKey SECRET_KEY --Domains zoneId1:domain1 zoneId2:domain2 [--Interval 300] [--IPChecker Custom]
```

* AccessKey: Mandatory. AWS IAM Account Access Key with Route53 access
* SecretKey: Mandatory. AWS IAM Account Secret Key with Route53 access
* Domains: Mandatory. Domains to update the IP address. Format: zoneId1:domain1 zoneId2:domain2
* Interval: Optional. Time to interval to run the updater. Default is 5 minutes (300 seconds)
* IPChecker: Optional. The service to use to get public IP. Default is Custom. Can be AWS, DynDns or Custom

## Docker Image
Docker image runs the .NET Core console application. Currently it only supports Linux containers.

To get the image, simply use:

```
docker pull volkanx/dyndns53
```

Running a container is very similar to running the .NET Core application:

```
docker run -d volkanx/dyndns53 --AccessKey {ACCESS KEY} --SecretKey {SECRET KEY} --Domains ZoneId1:Domain1 ZoneId2:Domain2 --Interval 300 --IPChecker AWS
```

The command above would run the container in daemon mode so that it can keep on updating the DNS every 5 minutes (300 seconds)

## WPF Client
This client already existed in the previous version. It's now updated to use the NuGet package. In order to use it, first create a copy of App.config.sample and rename it to app.config. Fill in the required fields such as keys and domains and run the GUI. It has an option to start at system startup so can run in the background and start automatically every time you restart. 


## Console Client / Windows Service
This application is also now using the NuGet package. Usage is similar to WPF client: Create a copy of config.sample, rename and replace the placeholders with actual values.

Application uses TopShelf which makes is dead simple to convert a console application into a Windows service. So by defualt you can run it as console application. Unlike .NET Core version, this doesn't use command line parameters. It gets the values from app.config file.

To install it as a service, just use the following command:

```
DynDns53.Client.WindowsService install
``` 

Similarly to uninstall:

```
DynDns53.Client.WindowsService uninstall
```

