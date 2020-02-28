#for install npm dependencies
FROM node:13.8-alpine AS jslib

#work in the app directory
WORKDIR /lib
COPY ./Application/wwwroot/lib ./



#install js dependencies
RUN npm install

#starting project
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine3.10 AS dotnet

#work in the app directory
WORKDIR /app
#copy project in docker
COPY ./Application ./

#copy js libs
COPY --from=jslib /lib/node_modules ./wwwroot/lib/node_modules

#restore dependencies
RUN dotnet restore

#install dotnet-ef for work with base
RUN dotnet tool install --global dotnet-ef
ENV PATH /root/.dotnet/tools:$PATH
#add migrations for work with database
RUN dotnet ef migrations add Initial

#add sqlite database
RUN dotnet ef database update

#project run
CMD dotnet run --urls http://0.0.0.0:5000

