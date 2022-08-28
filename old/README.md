# DetravLauncher

to build use `docker build -f DetravLauncher.Server/Dockerfile . -t detravlauncher:dev`


to run use 

```
sudo docker run --detach \
 --publish 8444:443 \
 --publish 8081:80 \
 --name launcher \
 --restart always \
 --volume /home/detrav/launcher/content:/app/content \
 detravlauncher:dev
```

