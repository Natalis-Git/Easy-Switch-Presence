<p align="center">
  <img align="center" src="EasySwitchPresence/Resources/spLogo.ico" width="100" alt="logo">
  <h1 align="center">Easy Switch Presence</h1>
  <h4 align="center">A quick and easy way to set a presence to Discord for your Switch games</h4>
</p>


# Introduction
Easy Switch Presence is an application that lets users set a rich presence to Discord for Nintendo Switch. This app was built with convenience in mind- You do not have to take any special extra steps to get it up and running; All you need to do is boot up the app and pick a game. It also comes with a couple of settings to make things even simpler if you'd like.

Other info: Due to the lack of any public API from Nintendo, this is a "fake" rich presence app in which the users manually pick out the game they wish to display. As of now, only select supported games can be displayed (Over 60 games, more coming soon), however, eventually an option will be added that lets the user enter their own game to display if it is not supported already. 

Note: This app is only available on Windows for the forseeable future. That said, if enough people request it, this could change.

# Installation
For those looking to download and use the app, download the latest release version [here](https://github.com/Natalis-Git/Easy-Switch-Presence/releases/)

Instructions:
1. Download zip file of latest release on the [release page](https://github.com/Natalis-Git/Easy-Switch-Presence/releases/). It can be found under "Assets".
2. Right click -> extract it to wherever you'd like to store the app
3. Once extracted, go into the Easy Switch Presence folder and right click "SwitchPresence.exe" and then "Create Shortcut"
4. Put the shortcut wherever you'd like on your desktop or toolbar
5. Open the app and enjoy. **It may ask you to download some .netcore dependencies the first time you start it up**.
6. A FAQ will be made soon to answer any questions you may have about the app.


# Developer Notes

This app is primarily a personal project I decided to do for learning purposes; While working on it I was simultaneously learning the WPF UI framework and the MVVM design pattern. That said, even though this is mainly a personal project I decided to fancy up the UI a little and make it public to everyone. If people like my app and start using it i'd be happy to support and improve it. I also intend to keep updating the game roster regardless, since me and my friends use the app ourselves.

About the supported game roster: Because I wanted to make this app to be a convenient alternative to the normal way one has to set up a rich presence on discord, I ultimately decided to make the selection of games a fixed list of supported games that the user can choose from. For the moment there is no way for the user to set their own custom game if it is not natively supported by this app. However, just like with the general improvements I mentioned before, if people like the app and start using it I actually would really like to add in a custom game feature. For now, I will try to support as many of the most popular switch games that I can, however because of the way Discord handles Rich Presence, I can only support a maximum of 149 games at one time. That should be plenty however... the app currently supports almost every popular game on the switch and the count only reaches a little over 60 games. Also about the game roster: For the moment, if the user wants to get the newest games supported by the app, they have to download the latest version, however I am currently looking into making this simpler by only requiring a single file to be put into the app folder.

I would like to give special thanks and credit to github user *Lachee*, who made the C# implementation of Discord RPC which you can find [here](https://github.com/Lachee/discord-rpc-csharp)
