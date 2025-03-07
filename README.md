# PowerGrip 💪

The Linux dashboard suite that will let you grasp the power!

PowerGrip is an open source system dashboard, system monitor and
security suite for Linux distributions aimed towards controlling
Linux servers remotely.

brought to you by *CH3CKMATE-2002*.

***

## 🎓 My Graduation Project &amp; A Gift to the World 🎁
PowerGrip isn’t just another open-source tool—it’s my graduation project,
built with passion, late nights, and way too much coffee.

I wanted to create something useful, powerful, and free for everyone,
rather than just another academic paper collecting dust.

This project is my gift to the Linux and open-source community—a way to give back to
the world of tech that has given me so much.

So whether you're a hobbyist, student, sysadmin, or just a curious hacker,
I hope you find PowerGrip useful!

And hey, if you do—consider contributing! 😉

***

## ⚠️ WARNING: *Not for Production!* ⚠️

**I repeat, DO NOT use PowerGrip in a production environment!**

At this stage, PowerGrip is a **fragile** and **incomplete** work-in-progress,
as many features are incomplete.

It’s an open-source project, and while it's making progress,
it’s *not* ready for real-world use just yet.

Using PowerGrip in a live system could result in unpredictable behavior, data loss,
or even complete chaos (we’re not *that* dramatic, but better safe than sorry).

Do yourself a favor, **DON’T** deploy it in a production environment until this
warning is removed. The project is a work in progress, and some features (like the IDS)
are still under development.

This warning will be updated once things are polished enough,
but until then, **consider yourself warned.**

Stay safe out there! 🛑

***

## 🎯 Features (So Far…)
### System Monitoring:
Keep an eye on CPU, memory, disk usage, and more.
- Review system health, battery if included, CPU and RAM.
- Check working peripheral devices and manage each.
- Identity system for the dashboard with JWT auth. (Supports many databases)
- Check system and dashboard logs in the log panel.
- Watch specific files and notify for changes, make backups and even protect. *(`/etc/*`)*
- Manage running processes and services.

### System Control:
- Manage server's file system.
- Open a terminal session in the browser.
- Install packages remotely.

### AI Powered Tools (WIP):
- Intrusion detection.
- Packet sniffing.
- Pre-attack prediction.

### Open-Source Goodness:
Built for the community, by the community (which, for now, is mostly just me 🥲).

More features are in the pipeline! Got suggestions? Open an issue or contribute!

## Requirements
- Linux (duh!)
- .NET for the backend (C# gang, rise up!!)
- Node.JS or Bun
- A web browser

## 🔧 Installation  

For now, setting up PowerGrip is a bit hands-on. Here’s how you can run it:  

```sh
git clone https://github.com/CH3CKMATE-2002/PowerGrip.git  
cd PowerGrip  
# Follow setup instructions here (coming soon!)  
```

## Screenshots 📸

(Coming Soon...)

## Contributing 🤝
Want to make PowerGrip better? Fork it, tweak it, and send a pull request!
Whether it’s fixing bugs, adding features, or just improving the README,
all contributions are welcome after being reviewed.

## License 📜
PowerGrip is licensed under the **GNU Affero General Public License v3 (AGPLv3)**.  

However, additional terms apply:  
🚨 **No Rebranding:** You cannot remove or alter PowerGrip’s branding or distribute a modified version under a different name.  
🚨 **No Unauthorized Selling:** You cannot sell PowerGrip as a product, but you **can** pre-install it in paid cloud services.  

For full details, see [`LICENSE`](./LICENSE) &amp; [`TERMS.md`](./TERMS.md).


## Got Questions? Any Feedback? 💬
Hit me up on GitHub Issues or Discussions.
I promise I won’t bite (unless your bug report is just "it's broken" with no details).

## Technical Info 🤖
#### Software & Libraries Used:
- Visual Studio Code.
- Bun
- Vite
- React
- TypeScript
- .NET 9
- ASP.NET Core
- EF Core