<div id="top"></div>

<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/pippinmole/Helios">
    <img src="images/helium-miners.png" alt="Logo" width="40%" >
  </a>

<h3 align="center">Helios Monitor</h3>

  <p align="center">
    Monitor your Helium miners' uptime and get notified when they go down!
    <br />
    <a href="https://github.com/pippinmole/Helios"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://heliosmonitor.com/">View Demo</a>
    ·
    <a href="https://github.com/pippinmole/Helios/issues">Report Bug</a>
    ·
    <a href="https://github.com/pippinmole/Helios/issues">Request Feature</a>
  </p>
</div>

<!-- ABOUT THE PROJECT -->
## About The Project

<p align="center">
<img src="images/site-screenshot.png" align="center" alt="Logo" width="2862" >
</p>

This website tracks uptime and diagnoses network issues for [Helium devices](https://www.helium.com/). The backend is written in ASP.NET Core Razor pages (Server-Side Rendering).

<p align="right">(<a href="#top">back to top</a>)</p>

### Built With

* [Razor Pages](https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-7.0&tabs=visual-studio)
* [Serilog](https://serilog.net/)
* [reCAPTCHA](https://www.google.com/recaptcha/about/)
* [MongoDB](https://www.mongodb.com/)
* [Bootstrap](https://getbootstrap.com)
* [JQuery](https://jquery.com)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple example steps.

### Installation

_Below is an example of how you can instruct your audience on installing and setting up your app. This template doesn't rely on any external dependencies or services._

1. Clone the repository
   ```sh
   https://github.com/pippinmole/Helios.git
   ```
2. Restore packages
   ```sh
   dotnet restore
   ```
   3. Set up user secrets
   ```sh
    dotnet user-secrets init
    dotnet user-secrets set "ConnectionStrings:DatabaseConnectionString" "<Get this from your dashboard>"
    dotnet user-secrets set "HeliumOptions:TransactionAddress" "<Helium Address>"
    dotnet user-secrets set "MailSenderOptions:ApiKey" "<Get this from your MailGun dashboard>"
    dotnet user-secrets set "MailSenderOptions:Domain" "<The domain emails will come from>"
    dotnet user-secrets set "MailSenderOptions:FromName" "<The name emails will come from>"
    dotnet user-secrets set "RecaptchaSettings:SecretKey" "<Get this from your google dashboard>"
    dotnet user-secrets set "RecaptchaSettings:SiteKey" "<Get this from your google dashboard>"
    dotnet user-secrets set "Serilog:Datadog:ApiKey" "<Get this from Datadog dashboard>"
    ```
4. Build the solution
   ```sh
   dotnet build
   ```
5. Finally, run
   ```sh
   dotnet run
   ```

<p align="right">(<a href="#top">back to top</a>)</p>



## Usage

Once you have executed ``dotnet run`` , a website at https://localhost:7086/ will be available. This site allows you to:
1. Create an account and sign in
2. Add Helium miners to your 'watch list'
3. Upgrade your account type by paying ``HNT`` to a specified address
4. Run diagnostics on your Helium device (Port checking, etc.)

<p align="right">(<a href="#top">back to top</a>)</p>


<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Your Name - jonathan.ruffles03@gmail.com

Project Link: [https://github.com/pippinmole/Helios](https://github.com/pippinmole/Helios)

<p align="right">(<a href="#top">back to top</a>)</p>



[//]: # (<!-- ACKNOWLEDGMENTS -->)

[//]: # (## Acknowledgments)

[//]: # ()
[//]: # (Use this space to list resources you find helpful and would like to give credit to. I've included a few of my favorites to kick things off!)

[//]: # ()
[//]: # (* [Choose an Open Source License]&#40;https://choosealicense.com&#41;)

[//]: # (* [GitHub Emoji Cheat Sheet]&#40;https://www.webpagefx.com/tools/emoji-cheat-sheet&#41;)

[//]: # (* [Malven's Flexbox Cheatsheet]&#40;https://flexbox.malven.co/&#41;)

[//]: # (* [Malven's Grid Cheatsheet]&#40;https://grid.malven.co/&#41;)

[//]: # (* [Img Shields]&#40;https://shields.io&#41;)

[//]: # (* [GitHub Pages]&#40;https://pages.github.com&#41;)

[//]: # (* [Font Awesome]&#40;https://fontawesome.com&#41;)

[//]: # (* [React Icons]&#40;https://react-icons.github.io/react-icons/search&#41;)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[stars-shield]: https://img.shields.io/github/stars/pippinmole/Helios.svg?style=for-the-badge
[stars-url]: https://github.com/pippinmole/Helios/stargazers
[issues-shield]: https://img.shields.io/github/issues/pippinmole/Helios.svg?style=for-the-badge
[issues-url]: https://github.com/pippinmole/Helios/issues
[license-shield]: https://img.shields.io/github/license/pippinmole/Helios.svg?style=for-the-badge
[license-url]: https://github.com/pippinmole/Helios/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/jonathan-ruffles-b44b30196/
[product-screenshot]: images/screenshot.png
