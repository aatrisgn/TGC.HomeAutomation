# default_repository
Default repository used for landingzone repositories

## Installing self-hosted agents

First, follow this guide: https://docs.github.com/en/actions/hosting-your-own-runners/managing-self-hosted-runners/adding-self-hosted-runners

Then, follow this guide: https://docs.github.com/en/actions/hosting-your-own-runners/managing-self-hosted-runners/configuring-the-self-hosted-runner-application-as-a-service

When hitting the point where you have to run ./config.sh, go to the section of runners on the specific repository to fetch a token and url. That is easier than manually locating the values.

### Re-installing.

Agents may be removed from GitHub if they aren't active enough by some definition. To mitigate it, I added a busy-job to run 4 times a day. Not sure if it works, though.

If the agents become idle, you need to re-configure if they have been idle for too long. This can be done by running the follow command:

- sudo ./svc.sh stop
- sudo ./svc.sh uninstall
- sudo ./configure.sh remove (If this cannot run without asking for token, delete the .runner folder and content in root of github runner)